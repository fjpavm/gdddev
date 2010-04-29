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

    using Gdd.Game.Engine;
    using Gdd.Game.Engine.Scenes;
    using Gdd.Game.Engine.Scenes.Lights;

    /// <summary>
    /// The level editor form.
    /// </summary>
    public partial class LevelEditorForm : Form
    {
        #region Constants and Fields

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
            this.tsmiPreviewStop.Visible = false;
            this.tsmiPreviewToolStripStop.Visible = false;
            this.tsmiFileOpen.Image = Resources.openHS;
            this.tsmiFileSave.Image = Resources.saveHS;
            this.tsmiPreviewRun.Image = Resources.PlayHS;
            this.tsmiPreviewStop.Image = Resources.StopHS;
            this.tsmiPreviewToolStripRun.Image = Resources.PlayHS;
            this.tsmiPreviewToolStripStop.Image = Resources.StopHS;
            this.InitializeMenuItems(this.tsddbShapes, typeof(DrawableSceneComponent));
            this.InitializeMenuItems(this.tsddbLights, typeof(Light));
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
        /// <param name="toolStripDropDownButton">
        /// The tool strip drop down button.
        /// </param>
        /// <param name="baseType">
        /// The base type.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private void InitializeMenuItems(ToolStripDropDownButton toolStripDropDownButton, Type baseType)
        {
            IEnumerable<ToolStripMenuItem> menuItems = from subType in baseType.GetSubTypes()
                                                       select
                                                           new ToolStripMenuItem(
                                                           subType.Name, null, this.ToolbarItemClick, subType.Name)
                                                               {
                                                                  Tag = subType 
                                                               };
            foreach (ToolStripMenuItem menuItem in menuItems)
            {
                toolStripDropDownButton.DropDownItems.Add(menuItem);
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
            this.toolStripToolbar.Enabled = false;
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
            this.toolStripToolbar.Enabled = true;
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

            var type = toolStripItem.Tag as Type;
            if (type == null)
            {
                return;
            }

            object component = Activator.CreateInstance(type, this.levelEditorPane);
            if (component is SceneComponent)
            {
                this.levelEditorPane.AddComponent((SceneComponent)component);
            }
        }

        #endregion
    }
}