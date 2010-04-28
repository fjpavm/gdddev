﻿using Gdd.Game.Engine.Scenes;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedModel.cs" company="GDD">
//   Game Design and Development
// </copyright>
// <summary>
//   The animated model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System.Linq;
    using System.Xml.Serialization;

    using FarseerGames.FarseerPhysics.Collisions;

    using Animation;

    using Characters;

    using Physics;

    using Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The animated model.
    /// </summary>
    public class AnimatedModel : StaticModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedModel"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public AnimatedModel(Game game)
            : base(game)
        {
            this.modelName = "mesh\\HeroAll";
            Direction = DIRECTION.RIGHT;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedModel"/> class.
        /// </summary>
        protected AnimatedModel()
            : this(null)
        {
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// The current physics vertices.
        /// </summary>
        [XmlIgnore]
        public Vertices currentPhysicsVertices { get; protected set; }

        /// <summary>
        /// The skinning data.
        /// </summary>
        [XmlIgnore]
        public SkinningData skinningData { get; protected set; }

        /// <summary>
        /// Gets or sets AnimationPlayer.
        /// </summary>
        protected ModelAnimationPlayer AnimationPlayer { get; set; }

        private Matrix OffsetMatrix;

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        // flip from left to right or right to left
        public void Flip(){
            if (Direction == DIRECTION.RIGHT)
            {
                Direction = DIRECTION.LEFT;
                this.YawRotation = 1.5f * MathHelper.Pi;
            }
            else{
                 Direction = DIRECTION.RIGHT;
                 this.YawRotation = 0.5f * MathHelper.Pi;
            }
        }

        /// <summary>
        /// The draw with technique.
        /// </summary>
        /// <param name="effect">
        /// The effect.
        /// </param>
        /// <param name="technique">
        /// The technique.
        /// </param>
        public override void DrawWithEffect(ShaderManager.EFFECT_ID effect, string technique)
        {
            int count = 0;
            // code from Riemers XNA tutorial
            foreach (ModelMesh mesh in this.ObjectModel.Meshes)
            {
                foreach (ModelMeshPart mmp in mesh.MeshParts)
                {
                    mmp.Effect.Dispose();
                    mmp.Effect = ShaderManager.GetEffect(effect).Clone(scene.Game.GraphicsDevice);
                }
                
                foreach (Effect e in mesh.Effects)
                {     
                    e.CurrentTechnique = e.Techniques[technique];
                    e.Parameters["Texture"].SetValue(this.ModelTextures[count++]);
                    e.Parameters["life"].SetValue(Hero.GetHeroLife());
                    e.Parameters["ID"].SetValue(this.ID);
                    e.Parameters["World"].SetValue(this.World);
                    e.Parameters["Bones"].SetValue(this.AnimationPlayer.GetSkinTransforms());
                    e.CommitChanges();
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!this.AnimationPlayer.IsStopped)
            {
                this.AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }

            if (AnimationPlayer.CurrentKeyframe != 0) {
                this.currentPhysicsVertices = AnimationPlayer.CurrentClip.vertices[(int)Direction][AnimationPlayer.CurrentKeyframe];
                
                Matrix m = this.PhysicsGeometry.Matrix;
                this.PhysicsGeometry.SetVertices(this.currentPhysicsVertices);
                this.PhysicsGeometry.Matrix = m * OffsetMatrix;

                this.aabb = this.PhysicsGeometry.AABB;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.ObjectModel = this.Game.Content.Load<Model>(this.modelName);

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "Effects\\AnimatedModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            this.DefaultTechnique = "AnimatedModelTechnique";

            ModelToVertices.TransformAnimatedModel(this, this.Game);

            this.AnimationPlayer = new ModelAnimationPlayer((SkinningData)this.ObjectModel.Tag);

            this.AnimationPlayer.SetClip(((SkinningData)this.ObjectModel.Tag).AnimationClips.Values.First());
            this.AnimationPlayer.StartClip();
            this.AnimationPlayer.StepClip();
            
            this.currentPhysicsVertices = this.AnimationPlayer.CurrentClip.vertices[(int)Direction][this.AnimationPlayer.CurrentKeyframe];
 
            this.PhysicsVertices = this.currentPhysicsVertices;

            LoadCommonContent();
            
            OffsetMatrix = Matrix.CreateTranslation(new Vector3(-offset, 0.0f));

            this.PhysicsBody.Mass = 70.0f;
            this.PhysicsBody.MomentOfInertia = float.MaxValue;
            this.PhysicsGeometry.FrictionCoefficient = 10.0f;
            this.PhysicsBody.IsStatic = false;
             
        }

        #endregion
    }
}