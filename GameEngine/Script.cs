// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Script.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The script.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine
{
    using System.Reflection;
    using System.Text;

    using CSScriptLibrary;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The script.
    /// </summary>
    public abstract class Script
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Script"/> class.
        /// </summary>
        protected Script()
        {
            this.Enabled = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="scriptText">
        /// The script text.
        /// </param>
        /// <returns>
        /// Compiled script instance.
        /// </returns>
        public static Script Load(string scriptText)
        {
            if (string.IsNullOrEmpty(scriptText))
            {
                return null;
            }

            Assembly assembly = CSScript.LoadCode(scriptText);

            var asmHelper = new AsmHelper(assembly);
            return asmHelper.TryCreateObject("*") as Script;
        }

        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="scriptText">
        /// The script text.
        /// </param>
        /// <returns>
        /// </returns>
        public static Script Load(string[] scriptText)
        {
            if (scriptText == null || scriptText.Length == 0)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            foreach (string line in scriptText)
            {
                stringBuilder.AppendLine(line);
            }

            return Load(stringBuilder.ToString());
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion
    }
}