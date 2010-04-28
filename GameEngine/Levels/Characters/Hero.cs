// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hero.cs" company="GDD">
//   Game Design and Development
// </copyright>
// <summary>
//   The hero class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Characters
{
    using Animation;

    using Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Scenes.Lights;

    /// <summary>
    /// The hero class.
    /// </summary>
    public class Hero : AnimatedModel
    {
        #region Constants and Fields

        /// <summary>
        /// The life.
        /// </summary>
        private static float life = 1.0f;
        private static Vector3 HeroPosition;
        private static DIRECTION HeroDirection;
        
        private string[] animations = new string[]{"idle", "Act", "Move", "Reach", "Death"};
        private int animationIndex = 0;
        private Vector2 jumpImpulse = new Vector2(0.0f, 4.0f);

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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        protected Hero()
            : this(null)
        {
        }

        #endregion

        #region Public Methods

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

        public static Vector3 GetHeroPosition(){
            return HeroPosition;
        }

        public static DIRECTION GetHeroDirection(){
            return HeroDirection;
        }

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

        KeyboardState lastState;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                this.PhysicsBody.ApplyImpulse(ref jumpImpulse);
                //this.AnimationPlayer.PauseClip();
            }
            /*else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                this.AnimationPlayer.ResumeClip();
            }*/

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                life -= 0.001f;
                if (life < 0.0f)
                {
                    life = 0.0f;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                life += 0.001f;
                if (life > 1.0f)
                {
                    life = 1.0f;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.V) && !lastState.IsKeyDown(Keys.V))
            {
                animationIndex++;
                if (animationIndex >= animations.Length)
                    animationIndex = 0;

                //currentClip = skinningData.AnimationClips[animations[animationIndex]];
                this.AnimationPlayer.SetClip(skinningData.AnimationClips[animations[animationIndex]]);
                this.AnimationPlayer.StartClip();
            }
            lastState = Keyboard.GetState();

            this.World = this.Rotation * this.Translation;
            HeroPosition = Position3D;
            HeroDirection = Direction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            gridCellSize = 10.0f;
            base.LoadContent();

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "AnimatedModel", this.Game);
            DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            DefaultTechnique = "AnimatedModelTechnique";
            skinningData = (SkinningData)ObjectModel.Tag;
                        
            //currentClip = skinningData.AnimationClips[animations[animationIndex]];

            AnimationPlayer.SetClip(skinningData.AnimationClips[animations[animationIndex]]);
            AnimationPlayer.StartClip();

            this.PhysicsBody.Mass = 10.0f;
        }
        #endregion
    }
}