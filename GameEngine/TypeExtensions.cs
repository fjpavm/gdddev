// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The type extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        #region Public Methods

        /// <summary>
        /// The get sub types.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<Type> GetSubTypes(this Type type)
        {
            if (type == null || string.IsNullOrEmpty(type.Assembly.Location))
            {
                return null;
            }

            DirectoryInfo directoryInfo = Directory.GetParent(type.Assembly.Location);

            if (directoryInfo == null)
            {
                return null;
            }

            return
                from assembly in
                    from fileName in Directory.GetFiles(directoryInfo.FullName, "*.dll", SearchOption.AllDirectories)
                    select Assembly.LoadFile(fileName)
                from t in assembly.GetTypes()
                where t.Inherits(type)
                select t;
        }

        /// <summary>
        /// The inherits.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="baseType">
        /// The base type.
        /// </param>
        /// <returns>
        /// The inherits.
        /// </returns>
        public static bool Inherits(this Type type, Type baseType)
        {
            if (type.BaseType == baseType)
            {
                return true;
            }

            if (type.BaseType == null)
            {
                return false;
            }

            return type.BaseType.Inherits(baseType);
        }

        #endregion
    }
}