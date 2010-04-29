using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gdd.Game.Engine.Levels;

namespace Gdd.Game.Engine.GUI
{
    public enum TextureType { Normal, Selected }
    public enum ButtonType { ModelButton, ConfirmButton }
    public enum ButtonConfirmType { OK, CANCEL }
    public class Button
    {
        SpriteFont font;
        SpriteBatch spriteBatch;
        Microsoft.Xna.Framework.Game game;
        Texture2D button_texture;
        Texture2D button_texture_normal;
        Texture2D button_texture_selected;

        private ButtonType bType;
        private ButtonConfirmType bConfirmType;
        public StaticModel.GEOMETRY_TYPE GeometryType { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
//        public GUIModel GuiModel { get; set; }
        public string GuiModelName { get; set; }
        public ButtonType ButtonType { get { return bType; } set { bType = value; } }
        public ButtonConfirmType ButtonConfirmType { get { return bConfirmType; } set { bConfirmType = value; } }
       
        public Button(Microsoft.Xna.Framework.Game game, string normal_image, string selected_image, int x, int y, int w, int h, string gui_model_name, StaticModel.GEOMETRY_TYPE geoType)
        {
            this.game = game;

            spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));
            font = this.game.Content.Load<SpriteFont>("Font");
            button_texture = this.game.Content.Load<Texture2D>(@"Textures\" + normal_image);
            button_texture_normal = button_texture;
            button_texture_selected = this.game.Content.Load<Texture2D>(@"Textures\" + selected_image);

            this.bType = ButtonType.ModelButton;
            this.GeometryType = geoType;

//             this.GuiModel = null;
//             this.GuiModel = new GUIModel(this.game) { Name = gui_model_name, geoType = geoType };
//             this.GuiModel.Initialize();
             this.GuiModelName = gui_model_name;

            X = x;
            Y = y;
            Width = w;
            Height = h;

        }
        public Button(Microsoft.Xna.Framework.Game game, string normal_image, int x, int y, int w, int h, ButtonConfirmType bConfirmType)
        {
            this.game = game;

            spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));
            font = this.game.Content.Load<SpriteFont>("Font");
            button_texture = this.game.Content.Load<Texture2D>(@"Textures\" + normal_image);

            this.bType = ButtonType.ConfirmButton;
            this.bConfirmType = bConfirmType;
//            this.GuiModel = null;

            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public void Draw()
        {
            if (spriteBatch == null)
                spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));
            spriteBatch.Draw(button_texture, new Rectangle(X, Y, Width, Height), Color.AntiqueWhite);
        }

        public bool IsIntersecting(int x, int y)
        {
            bool isIntersec = false;

            if (X < x && Y < y && x < X + Width && y < Y + Height)
            {
                isIntersec = true;
            }

            return isIntersec;
        }

        public void ChangeImage(TextureType type)
        {
            switch (type)
            {
                case TextureType.Normal:
                    this.button_texture = this.button_texture_normal;
                    break;
                case TextureType.Selected:
                    this.button_texture = this.button_texture_selected;
                    break;
                default:
                    break;
            }
        }
    }
}
