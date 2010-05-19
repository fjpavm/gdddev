using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdd.Game.Engine.AI;

namespace Gdd.Game.Engine.Levels.Characters
{
    using Gdd.Game.Engine.AI;
    using Gdd.Game.Engine.AI.StateMachines;
    using Gdd.Game.Engine.Animation;
    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Flower : AIMonster
    {

         #region Constants and Fields

        /// <summary>
        /// The animation index.
        /// </summary>
        private int animationIndex;

        #endregion


        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bunny"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public Flower(Game game)
            : base(game)
        {
            this.ModelName = "mesh//flower V1";
            this.Position2D = new Vector2(0.0f, 0.0f);
            this.mass = 10.0f;
            this.life = 1;


            Animations = new[] { "Idle" }; 
        }

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
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL);
            int nrOfPointLights = -1;
            var pointLightPositions = new Vector3[4];
            var pointLightColors = new Vector4[4];
            if (this.adjacentSceneComponents != null)
            {
                foreach (PointLight p in this.adjacentSceneComponents)
                {
                    nrOfPointLights++;
                    pointLightColors[nrOfPointLights] = p.Color.ToVector4();
                    pointLightPositions[nrOfPointLights] = p.Position3D;
                    if (nrOfPointLights == 3)
                    {
                        break;
                    }
                }
            }

            ShaderManager.SetValue(
                "InverseTransposeWorld", Matrix.Invert(Matrix.Transpose(Matrix.CreateTranslation(this.Position3D))));
            ShaderManager.SetValue("PointLightColors", pointLightColors);
            ShaderManager.SetValue("PointLightPositionsW", pointLightPositions);
            ShaderManager.SetValue("PointLightCount", nrOfPointLights);

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

            this.World = this.Rotation * this.Translation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.gridCellSize = 1.8f;
            
            base.LoadContent();
                        
            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.ANIMATEDMODEL, "AnimatedModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.ANIMATEDMODEL;
            this.DefaultTechnique = "AnimatedModelTechnique";
            this.skinningData = (SkinningData)this.ObjectModel.Tag;

            // currentClip = skinningData.AnimationClips[animations[animationIndex]];
            this.AnimationPlayer.StartClip();

            // Loading Bunny State Machines
            this.AnimationStateMachine = new FlowerAnimationStateMachine(this);
            this.MonsterStateMachine = new FlowerStateMachine(this);
        }

        #endregion
    }
}
