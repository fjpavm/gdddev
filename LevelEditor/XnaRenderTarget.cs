// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XnaRenderTarget.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The xna render target.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The xna render target.
    /// </summary>
    internal sealed class XnaRenderTarget : Control
    {
        #region Properties

        /// <summary>
        /// Gets or sets LevelEditorPane.
        /// </summary>
        public LevelEditorPane LevelEditorPane { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The is input char.
        /// </summary>
        /// <param name="charCode">
        /// The char code.
        /// </param>
        /// <returns>
        /// The is input char.
        /// </returns>
        protected override bool IsInputChar(char charCode)
        {
            return true;
        }

        /// <summary>
        /// The is input key.
        /// </summary>
        /// <param name="keyData">
        /// The key data.
        /// </param>
        /// <returns>
        /// The is input key.
        /// </returns>
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        /// <summary>
        /// The on got focus.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (this.LevelEditorPane != null)
            {
                this.LevelEditorPane.Resume();
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// The on lost focus.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (this.LevelEditorPane != null && !this.LevelEditorPane.IsPreviewRunning)
            {
                this.LevelEditorPane.Pause();
            }

            base.OnLostFocus(e);
        }

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
        }

        #endregion
    }
}