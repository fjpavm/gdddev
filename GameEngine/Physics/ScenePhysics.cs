// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScenePhysics.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The scene physics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Physics
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FarseerGames.FarseerPhysics;
    using FarseerGames.FarseerPhysics.Collisions;
    using FarseerGames.FarseerPhysics.Dynamics;
    using FarseerGames.FarseerPhysics.Factories;

    using Levels;

    using Scenes;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The scene physics.
    /// </summary>
    public class ScenePhysics : SceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The components.
        /// </summary>
        private readonly List<StaticModel> components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenePhysics"/> class.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        public ScenePhysics(Scene scene)
            : base(scene.Game)
        {
            this.scene = scene;
            this.components = new List<StaticModel>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
        /*    base.Initialize();
            foreach (DrawableSceneComponent dsc in this.scene.DrawableSceneComponents)
            {
                var sm = dsc as StaticModel;
                if (sm != null)
                {
                    this.components.Add(sm);
                }
            }*/

        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
         //   base.Update(gameTime);

//             var dt = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
//             SceneManager.physicsSimulator.Update(dt);
//           

            //FieldInfo fi = typeof(Body).GetField("_previousPosition", BindingFlags.Instance | BindingFlags.NonPublic);
         /*   foreach (StaticModel staticModel in this.components)
            {

                if (!(staticModel is Ground))
                {
                    
                    //Vector2 prevPos = (Vector2)fi.GetValue(staticModel.PhysicsBody);
                    //staticModel.Position2D += staticModel.PhysicsBody.Position - prevPos;
                }
            }
<<<<<<< TREE

            //foreach (StaticModel staticModel in this.components)
            //{
            ////    if (!(staticModel is Ground))
            //    {
            //        foreach (StaticModel otherModel in this.components)
            //        {
            //            if (staticModel != otherModel)
            //            {
            //                var b = staticModel.PhysicsGeometry.Collide(otherModel.PhysicsGeometry);
            //                if (b)
            //                {
            //                    //staticModel.PhysicsBody.IsStatic = true;
            //                    int i = 0;
            //                }
            //            }
            //        }
            //    }
            //}

            foreach (StaticModel staticModel in this.components)
=======
            */
/*            foreach (StaticModel staticModel in this.components)
>>>>>>> MERGE-SOURCE
            {
                if (!(staticModel is Ground))
                {
                    foreach (StaticModel otherModel in this.components)
                    {
                        if (staticModel != otherModel)
                        {
                            var b = staticModel.PhysicsGeometry.Collide(otherModel.PhysicsGeometry);
                            if (b)
                            {
                               // staticModel.PhysicsBody.IsStatic = true;
                                int i = 0;
                            }
                        }
                    }
                }
            }
<<<<<<< TREE


=======
            */

            /*
            foreach (StaticModel sm in this.components)
            {
                Vector2 diff = sm.Position2D - sm.PrevPos2D;
                
                sm.PhysicsBody.LinearVelocity = diff / dt;
            }
             
            this.physicsSimulator.Update(dt);

            /*
            foreach (StaticModel sm in this.components)
            {
                var newPos = sm.PhysicsBody.Position;
                var p = new Plane(-Vector3.UnitZ, -10f);
                var new3d = this.scene.Camera.Unproject((int)newPos.X, (int)newPos.Y).IntersectsAt(p);
                if (new3d.HasValue)
                {
                    sm.Position2D = new Vector2(new3d.Value.X, new3d.Value.Y);                    
                }
            }
            */
        }

        #endregion
    }
}