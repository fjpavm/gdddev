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
    using System.Linq;
    using System.Windows.Forms;

    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes;

    /// <summary>
    /// The level editor form.
    /// </summary>
    public partial class LevelEditorForm : Form
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
            this.tsmiPreviewStop.Visible = false;
            this.tsmiPreviewToolStripStop.Visible = false;
            this.tsmiFileOpen.Image = Resources.openHS;
            this.tsmiFileSave.Image = Resources.saveHS;
            this.tsmiPreviewRun.Image = Resources.PlayHS;
            this.tsmiPreviewStop.Image = Resources.StopHS;
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
            return this.pictureBox.Handle;
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
            this.levelEditorPane.SelectedComponentChanged += this.LevelEditorPane_SelectedComponentChanged;
            this.levelEditorPane.SelectedComponentPropertyChanged +=
                this.LevelEditorPane_SelectedComponentPropertyChanged;
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
                        this.ToolbarItemClick, 
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
                this.propertyGrid.SelectedObject = e.SelectedComponent;
            }
            else
            {
                this.propertyGrid.SelectedObject = this.levelEditorPane.Level;
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
            this.propertyGrid.SelectedObject = this.levelEditorPane.Level;
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
                this.textBoxLevelScript.Lines = this.levelEditorPane.Level.Script;
                this.tabControl.SelectedTab = this.tabPageLevelEditor;
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
            this.levelEditorPane.RunPreview();
            this.propertyGrid.Enabled = false;
            this.tsmiPreviewRun.Visible = false;
            this.tsmiPreviewToolStripRun.Visible = false;
            this.tsmiPreviewStop.Visible = true;
            this.tsmiPreviewToolStripStop.Visible = true;
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
            this.levelEditorPane.StopPreview();
            this.propertyGrid.Enabled = true;
            this.tsmiPreviewRun.Visible = true;
            this.tsmiPreviewToolStripRun.Visible = true;
            this.tsmiPreviewStop.Visible = false;
            this.tsmiPreviewToolStripStop.Visible = false;
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
        private void ToolbarItemClick(object sender, EventArgs e)
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
            if (component is SceneComponent)
            {
                this.propertyGrid.SelectedObject = component;
                this.levelEditorPane.AddComponent((SceneComponent)component);
            }
        }

        #endregion
    }
}