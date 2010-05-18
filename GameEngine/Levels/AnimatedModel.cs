using Gdd.Game.Engine.Scenes;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedModel.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The animated model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System.Linq;

    using FarseerGames.FarseerPhysics.Collisions;

    using Gdd.Game.Engine.Animation;
    using Gdd.Game.Engine.Common;
    using Gdd.Game.Engine.Levels.Characters;
    using Gdd.Game.Engine.Physics;
    using Gdd.Game.Engine.Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The animated model.
    /// </summary>
    public class AnimatedModel : StaticModel
    {
        #region Constants and Fields

        /// <summary>
        /// The offset matrix.
        /// </summary>
        private Matrix OffsetMatrix;

        #endregion

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
            this.modelName = "mesh\\Hero";
            this.ModelDirection = ModelDirection.Right;
            this.AddOffset = false;
            this.mass = 10.0f;
            this.Dead = false;
            this.Scale = Vector2.One;
        }

        #endregion

        #region Properties

        public bool Dead { get; protected set; }

        /// <summary>
        /// The current physics vertices.
        /// </summary>
        public Vertices currentPhysicsVertices { get; protected set; }

        /// <summary>
        /// The skinning data.
        /// </summary>
        public SkinningData skinningData { get; protected set; }

        /// <summary>
        /// Gets or sets AnimationPlayer.
        /// </summary>
        protected ModelAnimationPlayer AnimationPlayer { get; set; }

        /// <summary>
        /// Gets or sets the animation names
        /// </summary>
        public string[] Animations { get; protected set; }

        private string currentAnimation;

        public string CurrentAnimation { 
            get{
                return currentAnimation;
            } 

            set{
                currentAnimation = value;
            }
        }



        #endregion

        #region Public Methods

        public void ChangeAnimation(string animation, bool runOnce, bool interupt){
            if (interupt && animation != CurrentAnimation || this.AnimationPlayer.IsStopped)
            {
                CurrentAnimation = animation;
                this.AnimationPlayer.SetClip(this.skinningData.AnimationClips[CurrentAnimation]);

                if (runOnce)
                    this.AnimationPlayer.RunOnce();
                else
                    this.AnimationPlayer.StartClip();
            }
        }

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
                    mmp.Effect = ShaderManager.GetEffect(effect).Clone(this.scene.Game.GraphicsDevice);
                }

                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique = e.Techniques[technique];
                    e.Parameters["Texture"].SetValue(this.ModelTextures[count++]);
                    e.Parameters["life"].SetValue(Hero.GetHeroLife());
                    e.Parameters["World"].SetValue(this.World);
                    e.Parameters["Bones"].SetValue(this.AnimationPlayer.GetSkinTransforms());
                    e.CommitChanges();
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// The flip.
        /// </summary>
        public void Flip()
        {
            if (this.ModelDirection == ModelDirection.Right)
            {
                this.ModelDirection = ModelDirection.Left;
                this.YawRotation = 1.5f * MathHelper.Pi;
            }
            else
            {
                this.ModelDirection = ModelDirection.Right;
                this.YawRotation = 0.5f * MathHelper.Pi;
            }
        }

        public virtual void Die(){
            Dead = true;
            ChangeAnimation("Death", true, true);
        }

        public void Walk(){
            if (CurrentAnimation != "Death")
                ChangeAnimation("Walk", true, true);
        }

        public void Idle()
        {
            if(CurrentAnimation != "Death")
                ChangeAnimation("Idle", false, true);
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
                
                // disgusting fix, the model animation player does some strange things....
                if (this.AnimationPlayer.CurrentKeyframe > 2)
                {
                    this.currentPhysicsVertices =
                        this.AnimationPlayer.CurrentClip.vertices[(int)this.ModelDirection][
                            this.AnimationPlayer.CurrentKeyframe];

                    Matrix m = this.PhysicsGeometry.Matrix;
                    this.PhysicsGeometry.SetVertices(this.currentPhysicsVertices);
                    this.PhysicsGeometry.Matrix = m; 

                    this.aabb = this.PhysicsGeometry.AABB;
                }                
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            GddModel gddModel = ModelManager.LoadModel(this.modelName, this.Game, this.ScaleMatrix * this.Rotation);
            this.ObjectModel = gddModel.Model;
            this.ModelTextures = gddModel.Textures;

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "Effects\\AnimatedModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            this.DefaultTechnique = "AnimatedModelTechnique";
            this.AnimationPlayer = new ModelAnimationPlayer(this.ObjectModel.Tag as SkinningData);

            this.skinningData = (SkinningData)this.ObjectModel.Tag;

            this.AnimationPlayer.SetClip((this.ObjectModel.Tag as SkinningData).AnimationClips.Values.First());
            this.AnimationPlayer.StartClip();
            this.AnimationPlayer.StepClip();

            this.currentPhysicsVertices =
                this.AnimationPlayer.CurrentClip.vertices[(int)this.ModelDirection][this.AnimationPlayer.CurrentKeyframe];

            this.PhysicsVertices = this.currentPhysicsVertices;

            this.LoadCommonContent();

            this.PhysicsBody.Mass = 70.0f;
            this.PhysicsBody.MomentOfInertia = float.MaxValue;
            this.PhysicsGeometry.FrictionCoefficient = 10.0f;
            this.PhysicsBody.IsStatic = false;
        }

        #endregion
    }
}