namespace Gdd.Game.LevelEditor
{
    partial class LevelEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditorForm));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEditLevelProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreviewRun = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreviewStop = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageLevelEditor = new System.Windows.Forms.TabPage();
            this.tabPageLevelScript = new System.Windows.Forms.TabPage();
            this.textBoxLevelScript = new System.Windows.Forms.TextBox();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.toolStripToolbar = new System.Windows.Forms.ToolStrip();
            this.tsddbShapes = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsddbLights = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripPreview = new System.Windows.Forms.ToolStrip();
            this.tsmiPreviewToolStripRun = new System.Windows.Forms.ToolStripButton();
            this.tsmiPreviewToolStripStop = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageLevelEditor.SuspendLayout();
            this.tabPageLevelScript.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStripToolbar.SuspendLayout();
            this.toolStripPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(401, 341);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiEdit,
            this.tsmiPreview});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFileOpen,
            this.tsmiFileSave});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "File";
            // 
            // tsmiFileOpen
            // 
            this.tsmiFileOpen.Name = "tsmiFileOpen";
            this.tsmiFileOpen.Size = new System.Drawing.Size(152, 22);
            this.tsmiFileOpen.Text = "Open";
            this.tsmiFileOpen.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // tsmiFileSave
            // 
            this.tsmiFileSave.Name = "tsmiFileSave";
            this.tsmiFileSave.Size = new System.Drawing.Size(152, 22);
            this.tsmiFileSave.Text = "Save";
            this.tsmiFileSave.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEditLevelProperties});
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(39, 20);
            this.tsmiEdit.Text = "Edit";
            // 
            // tsmiEditLevelProperties
            // 
            this.tsmiEditLevelProperties.Name = "tsmiEditLevelProperties";
            this.tsmiEditLevelProperties.Size = new System.Drawing.Size(157, 22);
            this.tsmiEditLevelProperties.Text = "Level Properties";
            this.tsmiEditLevelProperties.Click += new System.EventHandler(this.LevelPropertiesToolStripMenuItem_Click);
            // 
            // tsmiPreview
            // 
            this.tsmiPreview.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPreviewRun,
            this.tsmiPreviewStop});
            this.tsmiPreview.Name = "tsmiPreview";
            this.tsmiPreview.Size = new System.Drawing.Size(60, 20);
            this.tsmiPreview.Text = "Preview";
            // 
            // tsmiPreviewRun
            // 
            this.tsmiPreviewRun.Name = "tsmiPreviewRun";
            this.tsmiPreviewRun.Size = new System.Drawing.Size(152, 22);
            this.tsmiPreviewRun.Text = "Run";
            this.tsmiPreviewRun.Click += new System.EventHandler(this.RunToolStripMenuItem_Click);
            // 
            // tsmiPreviewStop
            // 
            this.tsmiPreviewStop.Name = "tsmiPreviewStop";
            this.tsmiPreviewStop.Size = new System.Drawing.Size(152, 22);
            this.tsmiPreviewStop.Text = "Stop";
            this.tsmiPreviewStop.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(3, 3);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.propertyGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControl);
            this.splitContainer.Size = new System.Drawing.Size(626, 373);
            this.splitContainer.SplitterDistance = 207;
            this.splitContainer.TabIndex = 3;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(207, 373);
            this.propertyGrid.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageLevelEditor);
            this.tabControl.Controls.Add(this.tabPageLevelScript);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(415, 373);
            this.tabControl.TabIndex = 2;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPageLevelEditor
            // 
            this.tabPageLevelEditor.Controls.Add(this.pictureBox);
            this.tabPageLevelEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageLevelEditor.Name = "tabPageLevelEditor";
            this.tabPageLevelEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLevelEditor.Size = new System.Drawing.Size(407, 347);
            this.tabPageLevelEditor.TabIndex = 0;
            this.tabPageLevelEditor.Text = "Designer";
            this.tabPageLevelEditor.UseVisualStyleBackColor = true;
            // 
            // tabPageLevelScript
            // 
            this.tabPageLevelScript.Controls.Add(this.textBoxLevelScript);
            this.tabPageLevelScript.Location = new System.Drawing.Point(4, 22);
            this.tabPageLevelScript.Name = "tabPageLevelScript";
            this.tabPageLevelScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLevelScript.Size = new System.Drawing.Size(407, 346);
            this.tabPageLevelScript.TabIndex = 1;
            this.tabPageLevelScript.Text = "Script";
            this.tabPageLevelScript.UseVisualStyleBackColor = true;
            // 
            // textBoxLevelScript
            // 
            this.textBoxLevelScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLevelScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLevelScript.Location = new System.Drawing.Point(6, 6);
            this.textBoxLevelScript.Multiline = true;
            this.textBoxLevelScript.Name = "textBoxLevelScript";
            this.textBoxLevelScript.Size = new System.Drawing.Size(395, 359);
            this.textBoxLevelScript.TabIndex = 0;
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(632, 404);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(632, 429);
            this.toolStripContainer.TabIndex = 4;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStripToolbar);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStripPreview);
            // 
            // toolStripToolbar
            // 
            this.toolStripToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbShapes,
            this.tsddbLights});
            this.toolStripToolbar.Location = new System.Drawing.Point(3, 0);
            this.toolStripToolbar.Name = "toolStripToolbar";
            this.toolStripToolbar.Size = new System.Drawing.Size(121, 25);
            this.toolStripToolbar.TabIndex = 0;
            // 
            // tsddbShapes
            // 
            this.tsddbShapes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbShapes.Name = "tsddbShapes";
            this.tsddbShapes.Size = new System.Drawing.Size(57, 22);
            this.tsddbShapes.Text = "Shapes";
            // 
            // tsddbLights
            // 
            this.tsddbLights.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbLights.Name = "tsddbLights";
            this.tsddbLights.Size = new System.Drawing.Size(52, 22);
            this.tsddbLights.Text = "Lights";
            // 
            // toolStripPreview
            // 
            this.toolStripPreview.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPreviewToolStripRun,
            this.tsmiPreviewToolStripStop});
            this.toolStripPreview.Location = new System.Drawing.Point(125, 0);
            this.toolStripPreview.Name = "toolStripPreview";
            this.toolStripPreview.Size = new System.Drawing.Size(142, 25);
            this.toolStripPreview.TabIndex = 1;
            // 
            // tsmiPreviewToolStripRun
            // 
            this.tsmiPreviewToolStripRun.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPreviewToolStripRun.Image")));
            this.tsmiPreviewToolStripRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiPreviewToolStripRun.Name = "tsmiPreviewToolStripRun";
            this.tsmiPreviewToolStripRun.Size = new System.Drawing.Size(48, 22);
            this.tsmiPreviewToolStripRun.Text = "Run";
            this.tsmiPreviewToolStripRun.Click += new System.EventHandler(this.RunToolStripMenuItem_Click);
            // 
            // tsmiPreviewToolStripStop
            // 
            this.tsmiPreviewToolStripStop.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPreviewToolStripStop.Image")));
            this.tsmiPreviewToolStripStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiPreviewToolStripStop.Name = "tsmiPreviewToolStripStop";
            this.tsmiPreviewToolStripStop.Size = new System.Drawing.Size(51, 22);
            this.tsmiPreviewToolStripStop.Text = "Stop";
            this.tsmiPreviewToolStripStop.Click += new System.EventHandler(this.StopToolStripMenuItem_Click);
            // 
            // LevelEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "LevelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LevelEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageLevelEditor.ResumeLayout(false);
            this.tabPageLevelScript.ResumeLayout(false);
            this.tabPageLevelScript.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStripToolbar.ResumeLayout(false);
            this.toolStripToolbar.PerformLayout();
            this.toolStripPreview.ResumeLayout(false);
            this.toolStripPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileSave;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageLevelEditor;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEditLevelProperties;
        private System.Windows.Forms.TabPage tabPageLevelScript;
        private System.Windows.Forms.TextBox textBoxLevelScript;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreview;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreviewRun;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreviewStop;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStripToolbar;
        private System.Windows.Forms.ToolStripDropDownButton tsddbShapes;
        private System.Windows.Forms.ToolStripDropDownButton tsddbLights;
        private System.Windows.Forms.ToolStrip toolStripPreview;
        private System.Windows.Forms.ToolStripButton tsmiPreviewToolStripRun;
        private System.Windows.Forms.ToolStripButton tsmiPreviewToolStripStop;
    }
}