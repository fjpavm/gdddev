using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Gdd.Game.Engine.Menu
{
    using Scenes;

    public class MainMenu : Scenes.Scene
    {
        #region Constant(常量)

        // Rectangles used in pictures(图片所使用的矩形)
        static readonly Rectangle
            MainMenuLogoRect = new Rectangle(0, 0, 512, 110),
            MainMenuPlayRect = new Rectangle(0, 110, 512, 38),
            //  MainMenuLoadRect = new Rectangle(0, 148, 512, 38),
            //  MainMenuConfigureRect = new Rectangle(0, 186, 512, 38),
            MainMenuExitRect = new Rectangle(0, 224, 512, 38);


        SpriteBatch spriteBatch;

        #endregion

        #region Define variables(字段)

        // Define the textures(定义纹理)
        Texture2D backgroundTexture, menuTexture;

        // Define the game resolution(定义游戏分辨率)
        int width, height;

        // Define current gamepad and keyboard states(定义键盘和手柄按键状态)
        KeyboardState keyboard;
        GamePadState gamePad, gamePad2;

        // Remember up, down, start and back buttons for the menu.(为菜单记住手柄和按键状态)
        bool gamePadUp = false,
             gamePadDown = false,
             gamePad2Up = false,
             gamePad2Down = false;

        // Remember up, down, start and back buttons for the menu.(记住菜单中的按键状态)
        bool remUpPressed = false,
             remDownPressed = false,
             remSpaceOrStartPressed = false,
             remEscOrBackPressed = false;

        // Currently selected menu item.(现在的菜单选项)
        int currentMenuItem = 0;

        #endregion

        #region Sprite Rendering Helper Classes(渲染Sprite辅助类)

        // Create a sprite list(创建sprite列表)
        class SpriteToRender
        {
            public Texture2D texture;
            public Rectangle rect;
            public Rectangle? sourceRect;
            public Color color;

            public SpriteToRender(Texture2D setTexture, Rectangle setRect,
                Rectangle? setSourceRect, Color setColor)
            {
                texture = setTexture;
                rect = setRect;
                sourceRect = setSourceRect;
                color = setColor;
            }
        }
        List<SpriteToRender> sprites = new List<SpriteToRender>();

        // Define RenderSprite function(定义RenderSprite函数)
        public void RenderSprite(Texture2D texture, Rectangle rect, Rectangle? sourceRect, Color color)
        {
            sprites.Add(new SpriteToRender(texture, rect, sourceRect, color));
        }

        // Draw out the options in menu(画出菜单中的选项)
        public void RenderSprite(Texture2D texture, int x, int y, Rectangle? sourceRect, Color color)
        {
            RenderSprite(texture, new Rectangle(x, y, sourceRect.Value.Width, sourceRect.Value.Height),
                sourceRect, color);
        }

        // Draw out the LOGO in menu(画出菜单中的LOGO)
        public void RenderSprite(Texture2D texture, int x, int y, Rectangle? sourceRect)
        {
            RenderSprite(texture, new Rectangle(x, y, sourceRect.Value.Width, sourceRect.Value.Height),
                sourceRect, Color.White);
        }

        // Draw all sprites(画出所有的sprites)
        public void DrawSprites()
        {
            // No need to render if we got no sprites this frame(如果此帧没有sprites则无需渲染)
            if (sprites.Count == 0)
                return;

            // Start rendering sprites(开始渲染sprites)
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            // Render all sprites(渲染所有sprites)
            foreach (SpriteToRender sprite in sprites)
                spriteBatch.Draw(sprite.texture,
                    // Rescale to fit resolution(按比例缩放适应分辨率)
                    new Rectangle(
                    sprite.rect.X * width / 1024,
                    sprite.rect.Y * height / 768,
                    sprite.rect.Width * width / 1024,
                    sprite.rect.Height * height / 768),
                    sprite.sourceRect, sprite.color);
            spriteBatch.End();

            // Kill list of remembered sprites(清空sprites列表)
            sprites.Clear();
        }
        #endregion

        public MainMenu(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this.TopMost = true;
        }

        public override void Initialize()
        {
            // Setup game the game resolution(设置游戏分辨率)
            width = this.Game.GraphicsDevice.Viewport.Width;
            height = this.Game.GraphicsDevice.Viewport.Height;

            base.Initialize();
            LoadContent();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.(创建一个新的SpriteBatch用于渲染贴图textures.)
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            // Loading pictures(载入图片)
            backgroundTexture = this.Game.Content.Load<Texture2D>("MenuContent\\MainMenuBackground");
            //backgroundTexture = this.Game.Content.Load<Texture2D>("MenuContent\\MainMenuBackgroundLowRes");
            menuTexture = this.Game.Content.Load<Texture2D>("MenuContent\\MainMenu");

        }

        public override void Update(GameTime gameTime)
        {
            // Remember gamepad and keyboard states in last time(为菜单界面记住上次的键盘和手柄状态)
            remUpPressed =
                gamePad.DPad.Up == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y > 0.5f ||
                keyboard.IsKeyDown(Keys.Up);
            remDownPressed =
                gamePad.DPad.Down == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y < -0.5f ||
                keyboard.IsKeyDown(Keys.Down);
            remSpaceOrStartPressed =
                gamePad.Buttons.Start == ButtonState.Pressed ||
                gamePad.Buttons.A == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.LeftControl) ||
                keyboard.IsKeyDown(Keys.RightControl) ||
                keyboard.IsKeyDown(Keys.Space) ||
                keyboard.IsKeyDown(Keys.Enter);
            remEscOrBackPressed =
                gamePad.Buttons.Back == ButtonState.Pressed ||
                keyboard.IsKeyDown(Keys.Escape);

            // Get current gamepad and keyboard states(获取现在的手柄和键盘的状态)
            gamePad = GamePad.GetState(PlayerIndex.One);
            gamePad2 = GamePad.GetState(PlayerIndex.Two);
            keyboard = Keyboard.GetState();

            gamePadUp = gamePad.DPad.Up == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y > 0.5f;
            gamePadDown = gamePad.DPad.Down == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y < -0.5f;
            gamePad2Up = gamePad2.DPad.Up == ButtonState.Pressed ||
                gamePad2.ThumbSticks.Left.Y > 0.5f;
            gamePad2Down = gamePad2.DPad.Down == ButtonState.Pressed ||
                gamePad2.ThumbSticks.Left.Y < -0.5f;

            // Menu control(菜单控制)
            if ((keyboard.IsKeyDown(Keys.Down) ||
                    gamePadDown) &&
                    remDownPressed == false)
            {
                currentMenuItem = (currentMenuItem + 1) % 2;
                //soundBank.PlayCue("MenuClick");
                Audio.PlayClickSound();
            } // if (keyboard.IsKeyDown)
            else if ((keyboard.IsKeyDown(Keys.Up) ||
                    gamePadUp) &&
                    remUpPressed == false)
            {
                currentMenuItem = (currentMenuItem + 1) % 2;
                //soundBank.PlayCue("MenuClick");
                Audio.PlayClickSound();
            } // else if
            else if ((keyboard.IsKeyDown(Keys.Space) ||
                    keyboard.IsKeyDown(Keys.LeftControl) ||
                    keyboard.IsKeyDown(Keys.RightControl) ||
                    keyboard.IsKeyDown(Keys.Enter) ||
                    gamePad.Buttons.A == ButtonState.Pressed ||
                    gamePad.Buttons.Start == ButtonState.Pressed)) // ||
                // Back or Escape exits our game on Xbox 360 and Windows(退出游戏)
                //    keyboard.IsKeyDown(Keys.Escape) ||
                //    gamePad.Buttons.Back == ButtonState.Pressed) &&
                //    remSpaceOrStartPressed == false &&
                //    remEscOrBackPressed == false))
            {

                // Quit app.(退出程序)
                if (currentMenuItem == 1)
                //    keyboard.IsKeyDown(Keys.Escape) ||
                //    gamePad.Buttons.Back == ButtonState.Pressed)
                {
                    this.Game.Exit();
                }
                else if (currentMenuItem == 0)
                {
                    //Run game
                    this.Visible = false;
                    SceneManager.ChangeScene(Scenes.SceneManager.SCENE_ID.MAIN_GAME);
                    
                    // ohh, cant stand this music
                    //Audio.PlayBackgroundMusic();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw backgroud(显示背景)
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, width, height), Color.LightGray);
            spriteBatch.End();

            // Draw menu(显示菜单)
            RenderSprite(menuTexture,
                    512 - MainMenuLogoRect.Width / 2, 150, MainMenuLogoRect);
            RenderSprite(menuTexture,
                    512 - MainMenuPlayRect.Width / 2, 350, MainMenuPlayRect,
                    currentMenuItem == 0 ? Color.Orange : Color.White);
            //  RenderSprite(menuTexture,
            //          512 - MainMenuLoadRect.Width / 2, 350, MainMenuLoadRect,
            //          currentMenuItem == 1 ? Color.Orange : Color.White);
            //  RenderSprite(menuTexture,
            //          512 - MainMenuConfigureRect.Width / 2, 400, MainMenuConfigureRect,
            //          currentMenuItem == 2 ? Color.Orange : Color.White);
            RenderSprite(menuTexture,
                    512 - MainMenuExitRect.Width / 2, 450, MainMenuExitRect,
                    currentMenuItem == 1 ? Color.Orange : Color.White);

            // Draw everything on screen.(在屏幕上画出所有东西)
            DrawSprites();

            base.Draw(gameTime);
        }

    }
}
