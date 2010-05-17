// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEntityBindingAttribute.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level entity binding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;

    /// <summary>
    /// The level entity binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LevelEntityBindingAttribute : Attribute
    {
        #region Constants and Fields

        /// <summary>
        /// The class name.
        /// </summary>
        private readonly string className;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEntityBindingAttribute"/> class.
        /// </summary>
        /// <param name="className">
        /// The class name.
        /// </param>
        public LevelEntityBindingAttribute(string className)
        {
            this.className = className;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets ClassName.
        /// </summary>
        public string ClassName
        {
            get
            {
                return this.className;
            }
        }

        #endregion
    }
}