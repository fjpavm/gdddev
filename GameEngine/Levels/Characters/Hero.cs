using System;
using FarseerGames.FarseerPhysics.Collisions;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hero.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The hero class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Characters
{
    using Gdd.Game.Engine.Animation;
    using Gdd.Game.Engine.Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The hero class.
    /// </summary>
    public class Hero : AnimatedModel
    {
        #region Constants and Fields

        /// <summary>
        /// The animations.
        /// </summary>
        private readonly string[] animations = new[] { "idle", "Act", "Move", "Reach", "Death" };

        /// <summary>
        /// The hero direction.
        /// </summary>
        private static ModelDirection HeroModelDirection;

        /// <summary>
        /// The hero position.
        /// </summary>
        private static Vector3 HeroPosition;

        /// <summary>
        /// The life.
        /// </summary>
        private static float life = 1.0f;

        /// <summary>
        /// The target life.
        /// </summary>
        private static float targetLife = 1.0f;

        /// <summary>
        /// The aabb.
        /// </summary>
        public static Geom HeroGeometry;

        /// <summary>
        /// The animation index.
        /// </summary>
        private int animationIndex;

        /// <summary>
        /// The jump impulse.
        /// </summary>
        private Vector2 jumpImpulse = new Vector2(0.0f, 750.0f);

        /// <summary>
        /// The last state.
        /// </summary>
        private KeyboardState lastState;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public Hero(Game game)
            : base(game)
        {
            this.ModelName = "mesh//hero";
            this.Position2D = new Vector2(0.0f, 0.0f);
            this.mass = 20.0f;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get hero direction.
        /// </summary>
        /// <returns>
        /// </returns>
        public static ModelDirection GetHeroDirection()
        {
            return HeroModelDirection;
        }

        /// <summary>
        /// The get hero life.
        /// </summary>
        /// <returns>
        /// The get hero life.
        /// </returns>
        public static float GetHeroLife()
        {
            return life;
        }

        public static void DecreaseLife()
        {
            targetLife -= 0.1f;

            if (targetLife < 0.0f)
                targetLife = 0.0f;
        }

        public static void IncreaseLife()
        {
            targetLife += 0.1f;
            if (targetLife > 1.0f)
                targetLife = 1.0f;
        }

        /// <summary>
        /// The get hero position.
        /// </summary>
        /// <returns>
        /// </returns>
        public static Vector3 GetHeroPosition()
        {
            return HeroPosition;
        }


        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL);

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
            ShaderManager.SetCurrentEffect(effect);

            // todo - set some variables associated with this effect
            base.DrawWithEffect(effect, technique);
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
            
            if(targetLife < life){
                life -= 0.5f * gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (targetLife > life)
                    life = targetLife;
            }
            else if(targetLife > life){
                life += 0.5f * gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (targetLife < life)
                    life = targetLife;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && Math.Abs(PhysicsBody.LinearVelocity.Y) <= 0.03f)
            {
                this.PhysicsBody.ApplyImpulse(ref this.jumpImpulse);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.O) && lastState.IsKeyUp(Keys.O))
            {
                IncreaseLife();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.L) && lastState.IsKeyUp(Keys.L))
            {
                DecreaseLife();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.V) && !this.lastState.IsKeyDown(Keys.V))
            {
                this.animationIndex++;
                if (this.animationIndex >= this.animations.Length)
                {
                    this.animationIndex = 0;
                }

                // currentClip = skinningData.AnimationClips[animations[animationIndex]];
                this.AnimationPlayer.SetClip(this.skinningData.AnimationClips[this.animations[this.animationIndex]]);
                this.AnimationPlayer.StartClip();
            }

            this.lastState = Keyboard.GetState();

            this.World = this.Rotation * this.Translation;
            HeroPosition = this.Position3D;
            HeroModelDirection = this.ModelDirection;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            gridCellSize = 5.0f;
            base.LoadContent();

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "AnimatedModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            this.DefaultTechnique = "AnimatedModelTechnique";
            this.skinningData = (SkinningData)this.ObjectModel.Tag;

            // currentClip = skinningData.AnimationClips[animations[animationIndex]];
            this.AnimationPlayer.SetClip(this.skinningData.AnimationClips[this.animations[this.animationIndex]]);
            this.AnimationPlayer.StartClip();

            HeroGeometry = PhysicsGeometry;
        }

        #endregion
    }
}