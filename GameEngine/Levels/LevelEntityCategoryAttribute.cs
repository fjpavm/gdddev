// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEntityCategoryAttribute.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level entity category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;

    /// <summary>
    /// The level entity category.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LevelEntityCategoryAttribute : Attribute
    {
        #region Constants and Fields

        /// <summary>
        /// The category.
        /// </summary>
        private readonly string category;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEntityCategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        public LevelEntityCategoryAttribute(string category)
        {
            this.category = category;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Category.
        /// </summary>
        public string Category
        {
            get
            {
                return this.category;
            }
        }

        #endregion
    }
}