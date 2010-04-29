namespace Gdd.Game.Engine.Levels.Characters
{

    using Animation;

    using Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Scenes.Lights;

    public class Bunny: AI.AIMonster
    {
        #region Constants and Fields

        /// <summary>
        /// The life.
        /// </summary>
        private static float life;

        private string[] animations = new string[]{"idle", "Death", "Skating"};
        private int animationIndex = 0;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bunny"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public Bunny(Game game)
            : base(game)
        {
            this.ModelName = "mesh//rabbit";
            this.Position2D = new Vector2(0.0f, 0.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bunny"/> class.
        /// </summary>
        protected Bunny()
            : this(null)
        {
        }

        #endregion

        #region Public Methods

        public override void Draw(GameTime gameTime)
        {
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL);
            int nrOfPointLights = -1;
            Vector3[] pointLightPositions = new Vector3[4];
            Vector4[] pointLightColors = new Vector4[4];
            if (adjacentSceneComponents != null)
            {
                foreach (PointLight p in adjacentSceneComponents)
                {
                    nrOfPointLights++;
                    pointLightColors[nrOfPointLights] = p.Color.ToVector4();
                    pointLightPositions[nrOfPointLights] = p.Position3D;
                    if (nrOfPointLights == 3)
                        break;
                }
            }

            ShaderManager.SetValue("InverseTransposeWorld", Matrix.Invert(Matrix.Transpose(Matrix.CreateTranslation(Position3D))));
            ShaderManager.SetValue("PointLightColors", pointLightColors);
            ShaderManager.SetValue("PointLightPositionsW", pointLightPositions);
            ShaderManager.SetValue("PointLightCount", nrOfPointLights);

            base.Draw(gameTime);
            //DrawWithEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "AnimatedModelTechnique");
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
          
            this.World = this.Rotation * this.Translation;

            if(Keyboard.GetState().IsKeyDown(Keys.I) && !lastState.IsKeyDown(Keys.I))
            {
                animationIndex++;
                if (animationIndex >= animations.Length)
                    animationIndex = 0;

                //currentClip = skinningData.AnimationClips[animations[animationIndex]];
                this.AnimationPlayer.SetClip(skinningData.AnimationClips[animations[animationIndex]]);
                this.AnimationPlayer.StartClip();
            }
            lastState = Keyboard.GetState();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            gridCellSize = 7.0f;
            base.LoadContent();

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "AnimatedModel", this.Game);
            DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            DefaultTechnique = "AnimatedModelTechnique";
            skinningData = (SkinningData)ObjectModel.Tag;
                        
            //currentClip = skinningData.AnimationClips[animations[animationIndex]];

  //          AnimationPlayer.SetClip(currentClip);
  //          AnimationPlayer.StartClip();

            // Loading Bunny State Machines
            // some test patrol points
            Vector2 patrol1 = new Vector2(-1000000.0f,0.0f);
            Vector2 patrol2 = new Vector2(10,0);
            AnimationStateMachine = new Gdd.Game.Engine.AI.StateMachines.BunnyAnimationStateMachine(this);
            MonsterStateMachine = new Gdd.Game.Engine.AI.StateMachines.BunnyStateMachine(this, patrol1, patrol2); 
        }
        #endregion
    }
}
