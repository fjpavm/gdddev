// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameGUI.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The game gui.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.GUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The game gui.
    /// </summary>
    public class GameGUI : DrawableSceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The action rotate.
        /// </summary>
        private readonly GameAction actionRotate;

        /// <summary>
        /// The scale action.
        /// </summary>
        private readonly GameAction actionScale;

        /// <summary>
        /// The button models.
        /// </summary>
        private readonly List<Button> buttonModels;

        /// <summary>
        /// The input manager.
        /// </summary>
        private readonly InputManager inputManager;

        /// <summary>
        /// The pixel.
        /// </summary>
        private readonly Texture2D pixel;

        /// <summary>
        /// The model.
        /// </summary>
        private StaticModel model;

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private MouseState previousMouseState;

        /// <summary>
        /// The previous world coordinates.
        /// </summary>
        private Vector3 previousWorldCoordinates;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameGUI"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public GameGUI(Game game)
            : base(game)
        {
            this.inputManager = new InputManager();
            this.actionRotate = new GameAction("actionRotate");
            this.inputManager.MapToKey(this.actionRotate, Keys.LeftShift);
            this.actionScale = new GameAction("actionScale");
            this.inputManager.MapToKey(this.actionScale, Keys.LeftControl);

            this.buttonModels = new List<Button>();
            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));

            this.pixel = new Texture2D(this.Game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            this.pixel.SetData(new[] { Color.White });

            var button = new Button(
                this.Game,
                "iconcubebutton",
                "iconcubebutton", 
                0, 
                61 * 0, 
                61, 
                61, 
                "mesh\\box", 
                GeometryType.Rectangle);
            this.buttonModels.Add(button);

            button = new Button(
                this.Game,
                "iconspherebutton",
                "iconspherebutton", 
                0, 
                61 * 1, 
                61, 
                61,
                "mesh\\sphere", 
                GeometryType.Circle);
            this.buttonModels.Add(button);

            button = new Button(
                this.Game,
                "iconprismbutton",
                "iconprismbutton", 
                0, 
                61 * 2, 
                61, 
                61,
                "mesh\\tent", 
                GeometryType.Polygon);
            this.buttonModels.Add(button);
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
            if (this.spriteBatch == null)
            {
                this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            }

            this.spriteBatch.Begin();

            foreach (Button t in this.buttonModels)
            {
                t.Draw();
            }

            this.spriteBatch.End();
        }

        /// <summary>
        /// The generate circle.
        /// </summary>
        /// <param name="centerX">
        /// The center x.
        /// </param>
        /// <param name="centerY">
        /// The center y.
        /// </param>
        /// <param name="radius">
        /// The radius.
        /// </param>
        /// <returns>
        /// </returns>
        public List<Vector2> GenerateCircle(float centerX, float centerY, float radius)
        {
            var listCirclePoints = new List<Vector2>();

            float fidelity = MathHelper.ToRadians(0.8f);
            for (float angle = 0; angle < MathHelper.Pi * 2; angle += fidelity)
            {
                float x = (float)Math.Cos(angle) * radius;
                float y = (float)Math.Sin(angle) * radius;

                listCirclePoints.Add(new Vector2(x + centerX, y + centerY));
            }

            return listCirclePoints;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            this.inputManager.Update();

            // Get the current state of the Mouse
            MouseState currentMouseState = Mouse.GetState();
            Vector3 currentWorldCoordinates = this.scene.Camera.ScreenToWorld(currentMouseState.X, currentMouseState.Y);

            // Control mouse out of bounds)
            if (currentMouseState.X < 0 || currentMouseState.X > this.Game.Window.ClientBounds.Width ||
                currentMouseState.Y < 0 || currentMouseState.Y > this.Game.Window.ClientBounds.Height)
            {
                return;
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                this.previousMouseState.LeftButton != ButtonState.Pressed)
            {
                foreach (Button t in
                    this.buttonModels.Where(t => t.IsIntersecting(currentMouseState.X, currentMouseState.Y)))
                {
                    this.model = new StaticModel(this.Game)
                        {
                            ModelName = t.GuiModelName, 
                            GeometryType = t.GeometryType, 
                            Position2D = new Vector2(currentWorldCoordinates.X, currentWorldCoordinates.Y) 
                        };
                    this.model.DrawOrder = int.MaxValue-2;
                    this.scene.AddComponent(this.model);
                    this.model.Initialize();
                    this.model.PhysicsBody.IsStatic = true;
                    this.model.PhysicsGeometry.CollisionEnabled = false;
                }
            }
            else if (this.model != null)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed &&
                    this.previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    this.model.Position2D = new Vector2(currentWorldCoordinates.X, currentWorldCoordinates.Y);
                }

                if (this.actionScale.IsPressed)
                {
                    float diffX = this.model.Scale.X * (1+(currentWorldCoordinates.X - this.previousWorldCoordinates.X));
                    float diffY = this.model.Scale.Y * (1+(currentWorldCoordinates.Y - this.previousWorldCoordinates.Y));

                    if (diffX > 5.0f)
                    {
                        diffX = 5.0f;
                    }
                    else if (diffX < 0.5f)
                    {
                        diffX = 0.5f;
                    }

                    if (diffY > 5.0f)
                    {
                        diffY = 5.0f;
                    }
                    else if (diffY < 0.5f)
                    {
                        diffY = 0.5f;
                    }

                    this.model.Scale = new Vector2(diffX, diffY);
                }

                if (this.actionRotate.IsPressed)
                {
                    float diffX = currentWorldCoordinates.X - this.previousWorldCoordinates.X;
                    this.model.PhysicsBodyRotation += diffX;
                }
            }

            if (this.model != null && currentMouseState.LeftButton == ButtonState.Released)
            {
                Button intersectingButton =
                    (from b in this.buttonModels
                     where b.IsIntersecting(currentMouseState.X, currentMouseState.Y)
                     select b).SingleOrDefault();

                if (intersectingButton == null)
                {
                    // make the body not static and set the model to null
                    this.model.PhysicsBody.IsStatic = false;
                    this.model.PhysicsGeometry.CollisionEnabled = true;
                    this.model = null;
                }
                else
                {
                    // the object has been "returned" to the menu

                    // remove the model from the scene and the physicssimulator
                    this.scene.PhysicsSimulator.Remove(this.model.PhysicsBody);
                    this.scene.PhysicsSimulator.Remove(this.model.PhysicsGeometry);
                    this.scene.RemoveComponent(this.model);
                }
            }

            this.previousMouseState = currentMouseState;
            this.previousWorldCoordinates = currentWorldCoordinates;
        }

        #endregion
    }
}