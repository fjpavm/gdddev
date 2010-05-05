// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEditorForm.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level editor form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The level editor form.
    /// </summary>
    internal sealed partial class LevelEditorForm : Form
    {
        #region Constants and Fields

        /// <summary>
        /// The button entities.
        /// </summary>
        private Dictionary<ToolStripItem, LevelEntityTypeBinding> buttonEntities;

        /// <summary>
        /// The category buttons.
        /// </summary>
        private Dictionary<string, ToolStripDropDownButton> categoryButtons;

        /// <summary>
        /// The level editor pane.
        /// </summary>
        private LevelEditorPane levelEditorPane;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEditorForm"/> class.
        /// </summary>
        public LevelEditorForm()
        {
            this.InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.numericUpDownCamX.Maximum = decimal.MaxValue;
            this.numericUpDownCamX.Minimum = decimal.MinValue;
            this.numericUpDownCamY.Maximum = decimal.MaxValue;
            this.numericUpDownCamY.Minimum = decimal.MinValue;
            this.SetPreviewControls(false);
            this.tsmiFileOpen.Image = Resources.openHS;
            this.tsmiFileSave.Image = Resources.saveHS;
            this.tsmiPreviewContinue.Image = Resources.PlayHS;
            this.tsmiPreviewPause.Image = Resources.PauseHS;
            this.tsmiPreviewRun.Image = Resources.PlayHS;
            this.tsmiPreviewStop.Image = Resources.StopHS;
            this.tsmiPreviewToolStripContinue.Image = Resources.PlayHS;
            this.tsmiPreviewToolStripPause.Image = Resources.PauseHS;
            this.tsmiPreviewToolStripRun.Image = Resources.PlayHS;
            this.tsmiPreviewToolStripStop.Image = Resources.StopHS;
            this.InitializeMenuItems();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get draw surface.
        /// </summary>
        /// <returns>
        /// Returns the handle of the drawing surface.
        /// </returns>
        public IntPtr GetDrawSurface()
        {
            return this.xnaRenderTarget.Handle;
        }

        /// <summary>
        /// The set level editor pane.
        /// </summary>
        /// <param name="game">
        /// The Game instance.
        /// </param>
        public void SetLevelEditorPane(LevelEditorPane game)
        {
            this.levelEditorPane = game;
            this.levelEditorPane.CameraPositionChanged += this.LevelEditorPane_CameraPositionChanged;
            this.levelEditorPane.SelectedComponentChanged += this.LevelEditorPane_SelectedComponentChanged;
            this.levelEditorPane.SelectedComponentPropertyChanged +=
                this.LevelEditorPane_SelectedComponentPropertyChanged;
            this.levelEditorPane.LevelComponentsChanged += this.LevelEditorPane_LevelComponentsChanged;
            this.xnaRenderTarget.LevelEditorPane = game;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on form closed.
        /// </summary>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.levelEditorPane.Exit();
        }

        /// <summary>
        /// The button set cam position_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonSetCamPosition_Click(object sender, EventArgs e)
        {
            var cameraPosition = new Vector2((float)this.numericUpDownCamX.Value, (float)this.numericUpDownCamY.Value);
            this.levelEditorPane.SetCameraPosition(cameraPosition);
        }

        /// <summary>
        /// The combo box level components_ selected value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ComboBoxLevelComponents_SelectedValueChanged(object sender, EventArgs e)
        {
            this.levelEditorPane.SelectedComponent = this.comboBoxLevelComponents.SelectedItem as SceneComponent;
        }

        /// <summary>
        /// The continue tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ContinueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.levelEditorPane.IsPreviewRunning)
            {
                this.tsmiPreviewContinue.Enabled = false;
                this.tsmiPreviewToolStripContinue.Enabled = false;
                this.tsmiPreviewPause.Enabled = true;
                this.tsmiPreviewToolStripPause.Enabled = true;
                this.levelEditorPane.ResumeUpdate();
            }
        }

        /// <summary>
        /// The initialize menu items.
        /// </summary>
        private void InitializeMenuItems()
        {
            this.categoryButtons = new Dictionary<string, ToolStripDropDownButton>();
            this.buttonEntities = new Dictionary<ToolStripItem, LevelEntityTypeBinding>();

            foreach (LevelEntityTypeBinding levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings)
            {
                var category =
                    levelEntityTypeBinding.LevelEntityType.GetCustomAttributes(
                        typeof(LevelEntityCategoryAttribute), false).FirstOrDefault() as LevelEntityCategoryAttribute;
                if (category != null)
                {
                    ToolStripDropDownButton categoryButton;
                    if (!this.categoryButtons.TryGetValue(category.Category, out categoryButton))
                    {
                        categoryButton = new ToolStripDropDownButton(category.Category);
                        this.categoryButtons.Add(category.Category, categoryButton);
                    }

                    var menuItem = new ToolStripMenuItem(
                        levelEntityTypeBinding.SceneComponentType.Name, 
                        null, 
                        this.ToolbarItem_Click, 
                        levelEntityTypeBinding.SceneComponentType.Name);
                    this.buttonEntities.Add(menuItem, levelEntityTypeBinding);
                    categoryButton.DropDownItems.Add(menuItem);
                }
            }

            foreach (var pair in this.categoryButtons.OrderBy(cb => cb.Key))
            {
                this.toolStripToolbar.Items.Add(pair.Value);
            }
        }

        /// <summary>
        /// The level editor pane_ camera position changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LevelEditorPane_CameraPositionChanged(object sender, CameraPositionChangedEventArgs e)
        {
            this.numericUpDownCamX.Value = (decimal)e.CameraPosition.X;
            this.numericUpDownCamY.Value = (decimal)e.CameraPosition.Y;
        }

        /// <summary>
        /// The level editor pane_ level components changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void LevelEditorPane_LevelComponentsChanged(object sender, EventArgs e)
        {
            this.SetComboBoxItems();
        }

        /// <summary>
        /// The level editor pane_ selected block changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void LevelEditorPane_SelectedComponentChanged(object sender, SelectedComponentChangedEventArgs e)
        {
            if (e.SelectedComponent != null)
            {
                this.SetPropertyGridObject(e.SelectedComponent);
            }
            else
            {
                this.SetPropertyGridObject(this.levelEditorPane.Level);
            }
        }

        /// <summary>
        /// The level editor pane_ selected component property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void LevelEditorPane_SelectedComponentPropertyChanged(object sender, EventArgs e)
        {
            this.propertyGrid.Refresh();
        }

        /// <summary>
        /// The level properties tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void LevelPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetPropertyGridObject(this.levelEditorPane.Level);
        }

        /// <summary>
        /// The open tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Level file (*.lvl)|*.lvl" };
            DialogResult dr = dialog.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.levelEditorPane.LoadLevel(dialog.FileName);
                this.SetComboBoxItems();
                this.textBoxLevelScript.Lines = this.levelEditorPane.Level.Script;
                this.tabControl.SelectedTab = this.tabPageLevelEditor;
                this.xnaRenderTarget.Focus();
            }
        }

        /// <summary>
        /// The pause tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.levelEditorPane.IsPreviewRunning)
            {
                this.tsmiPreviewContinue.Enabled = true;
                this.tsmiPreviewToolStripContinue.Enabled = true;
                this.tsmiPreviewPause.Enabled = false;
                this.tsmiPreviewToolStripPause.Enabled = false;
                this.levelEditorPane.PauseUpdate();
            }
        }

        /// <summary>
        /// The property grid_ property value changed.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != null && e.ChangedItem.Label.Equals("Name", StringComparison.InvariantCulture))
            {
                if (string.IsNullOrEmpty((string)e.ChangedItem.Value) ||
                    this.levelEditorPane.Level.Components.Any(
                        c =>
                        c != this.propertyGrid.SelectedObject &&
                        c.Name.Equals((string)e.ChangedItem.Value, StringComparison.InvariantCulture)))
                {
                    var changedItem = this.propertyGrid.SelectedObject as SceneComponent;
                    if (changedItem != null)
                    {
                        changedItem.Name = (string)e.OldValue;
                    }
                }

                this.comboBoxLevelComponents.DisplayMember = string.Empty;
                this.comboBoxLevelComponents.DisplayMember = "Name";
            }
        }

        /// <summary>
        /// The run tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetPreviewControls(true);
            this.tsmiPreviewContinue.Enabled = false;
            this.tsmiPreviewToolStripContinue.Enabled = false;
            this.tsmiPreviewPause.Enabled = true;
            this.tsmiPreviewToolStripPause.Enabled = true;
            try
            {
                this.levelEditorPane.RunPreview();
            }
            catch (Exception)
            {
                this.SetPreviewControls(false);
                throw;
            }
        }

        /// <summary>
        /// The save tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "Level file (*.lvl)|*.lvl" };
            DialogResult dr = dialog.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.levelEditorPane.Level.Script = this.textBoxLevelScript.Lines;
                this.levelEditorPane.SaveLevel(dialog.FileName);
            }
        }

        /// <summary>
        /// The set combo box items.
        /// </summary>
        private void SetComboBoxItems()
        {
            IEnumerable<object> items =
                new[] { (object)this.levelEditorPane.Level }.Concat(
                    this.levelEditorPane.Level.Components.Cast<object>());
            this.comboBoxLevelComponents.Items.Clear();
            this.comboBoxLevelComponents.Items.AddRange(items.ToArray());
        }

        /// <summary>
        /// The set preview controls.
        /// </summary>
        /// <param name="preview">
        /// The preview.
        /// </param>
        private void SetPreviewControls(bool preview)
        {
            this.propertyGrid.Enabled = !preview;
            this.tsmiPreviewContinue.Visible = preview;
            this.tsmiPreviewToolStripContinue.Visible = preview;
            this.tsmiPreviewPause.Visible = preview;
            this.tsmiPreviewToolStripPause.Visible = preview;
            this.tsmiPreviewRun.Visible = !preview;
            this.tsmiPreviewToolStripRun.Visible = !preview;
            this.tsmiPreviewStop.Visible = preview;
            this.tsmiPreviewToolStripStop.Visible = preview;
            this.toolStripToolbar.Enabled = !preview;
        }

        /// <summary>
        /// The set property grid object.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void SetPropertyGridObject(object obj)
        {
            this.propertyGrid.HiddenProperties = null;
            this.propertyGrid.SelectedObject = obj;
            var sceneComponent = obj as SceneComponent;
            if (sceneComponent == null)
            {
                return;
            }

            this.comboBoxLevelComponents.SelectedItem = sceneComponent;
            Type sceneComponentType = sceneComponent.GetType();
            Type levelEntityType = (from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                    where levelEntityTypeBinding.SceneComponentType == sceneComponentType
                                    select levelEntityTypeBinding.LevelEntityType).FirstOrDefault();
            if (levelEntityType == null)
            {
                return;
            }

            IEnumerable<string> hiddenProperties =
                (from sceneComponentProperty in sceneComponentType.GetProperties() select sceneComponentProperty.Name).
                    Except(from levelEntityProperty in levelEntityType.GetProperties() select levelEntityProperty.Name);
            this.propertyGrid.HiddenProperties = hiddenProperties.ToArray();
            this.propertyGrid.Refresh();
        }

        /// <summary>
        /// The stop tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetPreviewControls(false);
            this.levelEditorPane.StopPreview();
            this.xnaRenderTarget.Focus();
        }

        /// <summary>
        /// The tab control_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab == this.tabPageLevelScript)
            {
                this.textBoxLevelScript.Lines = this.levelEditorPane.Level.Script;
            }
            else
            {
                this.levelEditorPane.Level.Script = this.textBoxLevelScript.Lines;
            }
        }

        /// <summary>
        /// The toolbar item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void ToolbarItem_Click(object sender, EventArgs e)
        {
            var toolStripItem = sender as ToolStripItem;
            if (toolStripItem == null)
            {
                return;
            }

            LevelEntityTypeBinding typeBinding;
            if (!this.buttonEntities.TryGetValue(toolStripItem, out typeBinding))
            {
                return;
            }

            object component = Activator.CreateInstance(typeBinding.SceneComponentType, this.levelEditorPane);
            var sceneComponent = component as SceneComponent;
            if (sceneComponent == null)
            {
                return;
            }

            this.xnaRenderTarget.Focus();
            SceneComponent existingComponent = (from existing in this.levelEditorPane.Level.Components
                                                where
                                                    existing.GetType() == typeBinding.SceneComponentType &&
                                                    !string.IsNullOrEmpty(existing.Name) &&
                                                    existing.Name.StartsWith(
                                                        typeBinding.SceneComponentType.Name, 
                                                        true, 
                                                        CultureInfo.InvariantCulture)
                                                orderby existing.Name descending
                                                select existing).FirstOrDefault();
            var sb = new StringBuilder();
            sb.Append(typeBinding.SceneComponentType.Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture));
            sb.Append(typeBinding.SceneComponentType.Name.Substring(1, typeBinding.SceneComponentType.Name.Length - 1));
            if (existingComponent != null)
            {
                string last = existingComponent.Name.Substring(
                    typeBinding.SceneComponentType.Name.Length, 
                    existingComponent.Name.Length - typeBinding.SceneComponentType.Name.Length);
                int number;
                if (int.TryParse(last, NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
                {
                    sb.Append(number + 1);
                }
            }
            else
            {
                sb.Append("1");
            }

            sceneComponent.Name = sb.ToString();
            this.SetPropertyGridObject(sceneComponent);
            this.levelEditorPane.AddComponent(sceneComponent);
            this.SetComboBoxItems();
            this.comboBoxLevelComponents.SelectedItem = sceneComponent;
        }

        #endregion
    }
}