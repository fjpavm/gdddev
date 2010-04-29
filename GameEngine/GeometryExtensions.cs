// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeometryExtensions.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The geometry extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The geometry extensions.
    /// </summary>
    public static class GeometryExtensions
    {
        #region Public Methods

        /// <summary>
        /// Determines whether this Ray intersects a specified Plane.
        /// </summary>
        /// <param name="ray">
        /// The Ray instance.
        /// </param>
        /// <param name="plane">
        /// The Plane with which to calculate this Ray's intersection.
        /// </param>
        /// <returns>
        /// Returns intersection coordinates or null.
        /// </returns>
        public static Vector3? IntersectsAt(this Ray ray, Plane plane)
        {
            float? distance = ray.Intersects(plane);
            if (!distance.HasValue)
            {
                return null;
            }

            return ray.Position + (ray.Direction * distance);
        }

        /// <summary>
        /// Determines whether this Plane intersects a specified Ray.
        /// </summary>
        /// <param name="plane">
        /// The Plane instance.
        /// </param>
        /// <param name="ray">
        /// The Ray with which to calculate this Plane's intersection.
        /// </param>
        /// <returns>
        /// Returns intersection coordinates or null.
        /// </returns>
        public static Vector3? IntersectsAt(this Plane plane, Ray ray)
        {
            return ray.IntersectsAt(plane);
        }

        #endregion
    }
}