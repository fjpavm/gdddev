using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gdd.Game.Engine.Levels;
using Gdd.Game.Engine.Render;


namespace Gdd.Game.Engine.GUI
{
    public enum ModelSituation { Drawing, NotDrawing, Drew }
    public class GUIModel : StaticModel
    {
        #region Constants and Fields

        /// <summary>
        /// The model.
        /// </summary>
        private Model model;

        /// <summary>
        /// The name.
        /// </summary>
        private string name;

        private ModelSituation modelSituation;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelModel"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public GUIModel(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this.ModelSituation = ModelSituation.NotDrawing;
            this.DefaultEffectID = ShaderManager.EFFECT_ID.STATICMODEL;
            this.DefaultTechnique = "StaticModelTechnique";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                if (this.isLoaded)
                {
                    this.LoadContent();
                    this.isChanged = true;
                }
            }
        }

        public ModelSituation ModelSituation
        {
            get { return modelSituation; }
            set { this.modelSituation = value; }
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
            base.Draw(gameTime);
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                this.modelName = this.Name;                
            }

            base.LoadContent();

            this.PhysicsBody.IsStatic = false;
        }

        /// <summary>
        /// The draw model.
        /// </summary>
        /// <param name="effectId">
        /// The effect id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        private void DrawModel(ShaderManager.EFFECT_ID effectId, string name)
        {
            this.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

            // code from Riemers XNA tutorial
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (ModelMeshPart mmp in mesh.MeshParts)
                {
                    mmp.Effect = ShaderManager.GetEffect(effectId);
                }

                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique = e.Techniques[name];
                }

                mesh.Draw();
            }
        }

        #endregion
    }
}
