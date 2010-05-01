// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEditorPane.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is the main type for your game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    using Gdd.Game.Engine;
    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
    using Keys = Microsoft.Xna.Framework.Input.Keys;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LevelEditorPane : Game
    {
        #region Constants and Fields

        /// <summary>
        /// The draw surface.
        /// </summary>
        private readonly IntPtr drawSurface;

        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The delete selected block.
        /// </summary>
        private GameAction deleteSelectedBlock;

        /// <summary>
        /// The invoke selected content property changed.
        /// </summary>
        private InvokeTimer invokeSelectedContentPropertyChanged;

        /// <summary>
        /// The is dragging.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// The levelEditorScene.
        /// </summary>
        private LevelEditorScene levelEditorScene;

        /// <summary>
        /// The level preview scene.
        /// </summary>
        private LevelScene levelPreviewScene;

        /// <summary>
        /// The new level block.
        /// </summary>
        private SceneComponent newComponent;

        /// <summary>
        /// The previous mouse state.
        /// </summary>
        private MouseState previousMouseState;

        /// <summary>
        /// The selected block.
        /// </summary>
        private SceneComponent selectedComponent;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEditorPane"/> class.
        /// </summary>
        /// <param name="drawSurface">
        /// The draw surface.
        /// </param>
        public LevelEditorPane(IntPtr drawSurface)
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.drawSurface = drawSurface;
            Mouse.WindowHandle = this.drawSurface;
            this.previousMouseState = Mouse.GetState();

            Control parent = Control.FromHandle(this.drawSurface);
            parent.SizeChanged += this.Parent_SizeChanged;
            this.SetWindowSize();

            this.graphics.PreparingDeviceSettings += this.Graphics_PreparingDeviceSettings;
            Control.FromHandle(this.Window.Handle).VisibleChanged += this.GameLevelEditor_VisibleChanged;

            SceneManager.Construct(this);
        }

        #endregion

        #region Events

        /// <summary>
        /// The selected block changed.
        /// </summary>
        public event EventHandler<SelectedComponentChangedEventArgs> SelectedComponentChanged;

        /// <summary>
        /// The selected component property changed.
        /// </summary>
        public event EventHandler SelectedComponentPropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets Level.
        /// </summary>
        public Level Level
        {
            get
            {
                return this.levelEditorScene.CurrentLevel;
            }
        }

        /// <summary>
        /// Gets or sets SelectedComponent.
        /// </summary>
        public SceneComponent SelectedComponent
        {
            get
            {
                return this.selectedComponent;
            }

            set
            {
                this.selectedComponent = value;
                this.InvokeSelectedComponentChanged(new SelectedComponentChangedEventArgs(this.selectedComponent));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add block.
        /// </summary>
        /// <param name="component">
        /// The level block.
        /// </param>
        public void AddComponent(SceneComponent component)
        {
            this.newComponent = component;
            this.SelectedComponent = null;
            this.InvokeSelectedComponentChanged(new SelectedComponentChangedEventArgs(component));
            this.levelEditorScene.AddComponent(component);
        }

        /// <summary>
        /// The load level.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public void LoadLevel(string fileName)
        {
            this.levelEditorScene.LoadFile(fileName);
            this.SelectedComponent = null;
        }

        /// <summary>
        /// The run preview.
        /// </summary>
        public void RunPreview()
        {
            this.levelPreviewScene.EnableScripts = true;
            this.levelPreviewScene.EnablePhysics = true;
            this.levelPreviewScene.CurrentLevel = (Level)this.levelEditorScene.CurrentLevel.Clone();
            SceneManager.SetCurrentScene(this.levelPreviewScene);
        }

        /// <summary>
        /// The save level.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public void SaveLevel(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var serializer = new LevelSerializer();
                serializer.Serialize(fileStream, this.levelEditorScene.CurrentLevel);
            }
        }

        /// <summary>
        /// The stop preview.
        /// </summary>
        public void StopPreview()
        {
            this.levelPreviewScene.EnableScripts = false;
            this.levelPreviewScene.EnablePhysics = false;
            SceneManager.SetCurrentScene(this.levelEditorScene);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            SceneManager.Draw(gameTime);

            this.spriteBatch.Begin();
            var font = this.Content.Load<SpriteFont>("Courier");
            string str = String.Format("Mouse X: {0}, Y: {1}", Mouse.GetState().X, Mouse.GetState().Y);
            this.spriteBatch.DrawString(font, str, Vector2.Zero, Color.White);
            this.spriteBatch.End();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.levelEditorScene = new LevelEditorScene(this) { MainGameScene = true };
            this.deleteSelectedBlock = new GameAction("deleteSelectedBlock", GameActionBehavior.DetectInitialPressOnly);
            this.levelEditorScene.InputManager.MapToKey(this.deleteSelectedBlock, Keys.Delete);
            SceneManager.AddScene(this.levelEditorScene);

            this.levelPreviewScene = new LevelScene(this) { MainGameScene = true };
            SceneManager.AddScene(this.levelPreviewScene);

            SceneManager.SetCurrentScene(this.levelEditorScene);

            this.invokeSelectedContentPropertyChanged = new InvokeTimer(this)
                {
                    Interval = 200, 
                    Target = this, 
                    TargetMethod =
                        this.GetType().GetMethod(
                            "InvokeSelectedComponentPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic)
                };
            this.Components.Add(this.invokeSelectedContentPropertyChanged);

            this.SelectedComponent = null;
            SceneManager.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.spriteBatch);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            SceneManager.Update(gameTime);

            MouseState mouseState = Mouse.GetState();

            if (this.newComponent != null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    this.SelectedComponent = this.newComponent;
                    this.newComponent = null;
                }
                else if (mouseState.RightButton == ButtonState.Pressed)
                {
                    this.levelEditorScene.CurrentLevel.Components.Remove(this.SelectedComponent);
                    this.newComponent = null;
                }
                else
                {
                    Vector3 world = this.ScreenToWorld(mouseState.X, mouseState.Y);
                    this.newComponent.Position2D = new Vector2(world.X, world.Y);
                    this.invokeSelectedContentPropertyChanged.Invoke(EventArgs.Empty);
                }
            }
            else
            {
                if (this.IsMouseInViewport(mouseState.X, mouseState.Y))
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && !this.isDragging)
                    {
                        Vector3 clicked = this.ScreenToWorld(mouseState.X, mouseState.Y);
                        this.SelectedComponent = this.levelEditorScene.CurrentLevel.GetBlockAt(clicked);
                    }
                }
            }

            if (this.SelectedComponent != null)
            {
                if (this.deleteSelectedBlock.IsPressed)
                {
                    this.levelEditorScene.CurrentLevel.Components.Remove(this.SelectedComponent);
                    this.SelectedComponent = null;
                }
                else
                {
                    this.isDragging = mouseState.LeftButton == ButtonState.Pressed &&
                                      this.IsMouseInViewport(mouseState.X, mouseState.Y);
                    if (this.isDragging && this.previousMouseState.X != mouseState.X &&
                        this.previousMouseState.Y != mouseState.Y)
                    {
                        this.SelectedComponent.Position2D = new Vector2(
                            this.ScreenToWorld(mouseState.X, mouseState.Y).X, 
                            this.ScreenToWorld(mouseState.X, mouseState.Y).Y);
                        this.invokeSelectedContentPropertyChanged.Invoke(EventArgs.Empty);
                    }
                }
            }

            this.previousMouseState = mouseState;
            base.Update(gameTime);
        }

        /// <summary>
        /// Occurs when the original gamewindows' visibility changes and makes sure it stays invisible
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void GameLevelEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (Control.FromHandle(this.Window.Handle).Visible)
            {
                Control.FromHandle(this.Window.Handle).Visible = false;
            }
        }

        /// <summary>
        /// Event capturing the construction of a draw surface and makes sure this gets redirected to
        /// a predesignated drawsurface marked by pointer drawSurface
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = this.drawSurface;
        }

        /// <summary>
        /// The invoke selected block changed.
        /// </summary>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void InvokeSelectedComponentChanged(SelectedComponentChangedEventArgs e)
        {
            EventHandler<SelectedComponentChangedEventArgs> handler = this.SelectedComponentChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// The invoke selected component property changed.
        /// </summary>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        /// ReSharper disable UnusedMember.Local
        private void InvokeSelectedComponentPropertyChanged(EventArgs e)
        {
            // ReSharper restore UnusedMember.Local
            EventHandler handler = this.SelectedComponentPropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// The is mouse in viewport.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// Returns true if the mouse is in the viewport.
        /// </returns>
        private bool IsMouseInViewport(int x, int y)
        {
            return x >= 0 && y >= 0 && x <= this.GraphicsDevice.Viewport.Width &&
                   y <= this.GraphicsDevice.Viewport.Height;
        }

        /// <summary>
        /// The parent_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The EventArgs.
        /// </param>
        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            this.SetWindowSize();
        }

        /// <summary>
        /// The screen to world.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// Returns the world coordinate.
        /// </returns>
        private Vector3 ScreenToWorld(int x, int y)
        {
            var p = new Plane(-Vector3.UnitZ, -10f);

            Vector3? point = this.levelEditorScene.Camera.Unproject(x, y).IntersectsAt(p);
            return point ?? Vector3.Zero;
        }

        /// <summary>
        /// The set window size.
        /// </summary>
        private void SetWindowSize()
        {
            Control parent = Control.FromHandle(this.drawSurface);
            this.graphics.PreferredBackBufferHeight = parent.ClientRectangle.Height;
            this.graphics.PreferredBackBufferWidth = parent.ClientRectangle.Width;
            this.graphics.ApplyChanges();
            if (this.levelEditorScene != null)
            {
                this.levelEditorScene.Camera.AspectRatio = this.GraphicsDevice.Viewport.AspectRatio;
            }
        }

        #endregion
    }
}