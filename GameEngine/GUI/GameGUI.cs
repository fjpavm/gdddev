using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gdd.Game.Engine.Levels;
using Gdd.Game.Engine.Scenes;
using Gdd.Game.Engine;
using Gdd.Game.Engine.Scenes.Lights;
using Gdd.Game.Engine.Common;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;

namespace Gdd.Game.Engine.GUI
{
    public class GameGUI
    {
        private Microsoft.Xna.Framework.Game game;        
        List<Button> buttonModels;
        List<Button> buttonConfirmations;
        SpriteBatch spriteBatch;
        Button previousSelectedButtun;
        MouseState previousMouseState;
        Vector2 startPoint, endPoint;
        Texture2D pixel;
        
        private StaticModel model;

        private Scene scene;

        public GameGUI(Microsoft.Xna.Framework.Game game)
        {
            this.game = game;
            buttonModels = new List<Button>();
            buttonConfirmations = new List<Button>();
            spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));

            pixel = new Texture2D(this.game.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);

            Button button = new Button(this.game, "button_apply_normal", "button_apply_selected", 0, 33 * 0, 88, 33, "tmpCube", GeometryType.Rectangle);
            buttonModels.Add(button);

            button = new Button(this.game, "button_apply_normal", "button_apply_selected", 0, 33 * 1, 88, 33, "cube", GeometryType.Rectangle);
            buttonModels.Add(button);

            button = new Button(this.game, "button_apply_normal", "button_apply_selected", 0, 33 * 2, 88, 33, "sphere", GeometryType.Circle);
            buttonModels.Add(button);

            button = new Button(this.game, "button_apply_normal", "button_apply_selected", 0, 33 * 3, 88, 33, "cone", GeometryType.Polygon);
            buttonModels.Add(button);

            button = new Button(this.game, "button_ok", 0, this.game.Window.ClientBounds.Height - (33 * 2), 100, 33, ButtonConfirmType.OK);
            buttonConfirmations.Add(button);

            button = new Button(this.game, "button_cancel", 0, this.game.Window.ClientBounds.Height - 33, 100, 33, ButtonConfirmType.CANCEL);
            buttonConfirmations.Add(button);

            //this.sceneManager = new SceneManager(this.game);
            //if (scene == null)
            //{
            //    this.scene = new Scene(this.game) { Visible = true, Enabled = true, MainGameScene = true };
            //    this.scene.Camera = new Camera(this.game, new Vector3(0.0f, 0.0f, 30.0f)) { FieldOfView = 70.0f };
            //    this.scene.Light = new DirectionalLight(this.game)
            //    {
            //        Position3D = new Vector3(0.0f, 0.0f, 10.0f),
            //        Color = Color.CornflowerBlue
            //    };

            //    SceneManager.AddScene(this.scene);
            //    SceneManager.SetCurrentScene(this.scene);
            //}
            //game.Components.Add(this.sceneManager);
            //SceneManager.Initialize();

        }

        public void LoadContent()
        {

        }

        public void Draw()
        {
            if (spriteBatch == null)
                spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Begin();
           /* if (previousSelectedButtun == null || previousSelectedButtun.GuiModel.ModelSituation == ModelSituation.NotDrawing)
            {*/
                for (int i = 0; i < buttonModels.Count; i++)
                {
                    buttonModels[i].Draw();
                }
            /*}
            else
            {
                if (previousSelectedButtun.GuiModel.ModelSituation == ModelSituation.Drew)
                    for (int i = 0; i < buttonConfirmations.Count; i++)
                    {
                        buttonConfirmations[i].Draw();
                    }
            }*/

            if (startPoint.X != 0 && startPoint.Y != 0 && endPoint.X != 0 && endPoint.Y != 0)
            {
                ///****************************************************************
                ///remove scale...
                //// calculate the distance between the two vectors
                //float distance = Vector2.Distance(startPoint, endPoint);

                //// calculate the angle between the two vectors
                //float angle = (float)Math.Atan2((double)(endPoint.Y - startPoint.Y),
                //    (double)(endPoint.X - startPoint.X));

                //// stretch the pixel between the two vectors
                //spriteBatch.Draw(pixel,
                //    startPoint,
                //    null,
                //    Color.White,
                //    angle,
                //    Vector2.Zero,
                //    new Vector2(distance, 1),
                //    SpriteEffects.None,
                //    0);
                ////*****************************************************************
               
                // Oli and Yigit took this stuff out
                /*
                if (!this.scene.IsGameComponentAddAlready(previousSelectedButtun.GuiModel))
                    this.scene.AddComponent(previousSelectedButtun.GuiModel);
                if (previousSelectedButtun.GuiModel.ModelSituation != ModelSituation.Drew)
                    previousSelectedButtun.GuiModel.ModelSituation = ModelSituation.Drawing;
                 
                  */
                ////********************************************
                //Vector3 clicked = ScreenToWorld((int)((endPoint.X + startPoint.X) / 2), (int)((endPoint.Y + startPoint.Y) / 2));
                //previousSelectedButtun.GuiModel.Position2D = new Vector2(clicked.X, clicked.Y);
                //DrawableAABB box = previousSelectedButtun.GuiModel.aabb;

                //Vector3 vStartPoint = ScreenToWorld((int)startPoint.X, (int)startPoint.Y);
                //Vector3 vEndPoint = ScreenToWorld((int)endPoint.X, (int)endPoint.Y);
                //float minX = 0, maxX = 0;
                //float minY = 0, maxY = 0;

                //minX = vEndPoint.X;
                //maxX = vStartPoint.X;
                //if (vStartPoint.X < vEndPoint.X)
                //{
                //    minX = vStartPoint.X;
                //    maxX = vEndPoint.X;
                //}

                //minY = vEndPoint.Y;
                //maxY = vStartPoint.Y;
                //if (vStartPoint.Y < vEndPoint.Y)
                //{
                //    minY = vStartPoint.Y;
                //    maxY = vEndPoint.Y;
                //}
                /*Vector3 clicked = ScreenToWorld((int)(endPoint.X ), (int)((endPoint.Y )));
                previousSelectedButtun.GuiModel.Position2D = new Vector2(clicked.X, clicked.Y);
                 */
                //DrawableAABB box = previousSelectedButtun.GuiModel.aabb;
            }
            spriteBatch.End();
        }

        public List<Vector2> generateCircle(float centerX, float centerY, float radius)
        {
            List<Vector2> listCirclePoints = new List<Vector2>();

            float fidelity = MathHelper.ToRadians(0.8f);
            for (float angle = 0; angle < MathHelper.Pi * 2; angle += fidelity)
            {
                float X = (float)Math.Cos(angle) * radius;
                float Y = (float)Math.Sin(angle) * radius;

                listCirclePoints.Add(new Vector2(X + centerX, Y + centerY));

            }
            return listCirclePoints;
        }

        public void Update()
        {
            //Get the current state of the Mouse
            MouseState currentMouseState = Mouse.GetState();

            // Control mouse out of bounds
            if (currentMouseState.X < 0 || currentMouseState.X > this.game.Window.ClientBounds.Width ||
                currentMouseState.Y < 0 || currentMouseState.Y > this.game.Window.ClientBounds.Height)
                return;

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                for (int i = 0; i < buttonModels.Count; i++)
                {
                    if (buttonModels[i].IsIntersecting(currentMouseState.X, currentMouseState.Y))
                    {
                        Vector3 position = ScreenToWorld((int)(currentMouseState.X), (int)(currentMouseState.Y));
                        model = new StaticModel(this.game) { ModelName = buttonModels[i].GuiModelName, GeometryType = buttonModels[i].GeometryType, Position2D = new Vector2(position.X, position.Y)};
                        model.Initialize();

                        model.PhysicsGeometry.CollisionEnabled = false;

                        this.scene.AddComponent(model);
                    }
                }
            }
            else if (model != null && currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector3 newPos = ScreenToWorld((int)(currentMouseState.X), (int)(currentMouseState.Y));
                model.Position2D = new Vector2(newPos.X, newPos.Y);
            }

            if(model != null && currentMouseState.LeftButton == ButtonState.Released){
               
                Button intersectingButton = (from b in buttonModels where b.IsIntersecting(currentMouseState.X, currentMouseState.Y) select b).SingleOrDefault<Button>();

                if (intersectingButton == null)
                {
                    // make the body not static and set the model to null
                    model.PhysicsBody.IsStatic = false;
                    model.PhysicsGeometry.CollisionEnabled = true;

                    model = null;
                }
                else {     
                    // the object has been "returned" to the menu
                    // remove the model from the scene and the physicssimulator

                    this.scene.PhysicsSimulator.Remove(model.PhysicsBody);
                    this.scene.PhysicsSimulator.Remove(model.PhysicsGeometry);

                    this.scene.RemoveComponent(model);
                }
                
            }
/*
            if (previousSelectedButtun != null && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                //startPoint = new Vector2();
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    startPoint = new Vector2(currentMouseState.X, currentMouseState.Y);
                }

                //endPoint = new Vector2();
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    endPoint = new Vector2(currentMouseState.X, currentMouseState.Y);
                }
                else
                {
                    endPoint = new Vector2();
                }

                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    startPoint = new Vector2();
                    endPoint = new Vector2();
                }
            }

            if (previousSelectedButtun != null && currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                if (previousSelectedButtun.GuiModel.ModelSituation == ModelSituation.Drawing)
                    previousSelectedButtun.GuiModel.ModelSituation = ModelSituation.Drew;
            }

            Button selectedButtun = null;

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (previousSelectedButtun == null || previousSelectedButtun.GuiModel.ModelSituation == ModelSituation.NotDrawing)
                {
                    for (int i = 0; i < buttonModels.Count; i++)
                    {
                        if (buttonModels[i].isIntersec(currentMouseState.X, currentMouseState.Y))
                        {
                            selectedButtun = buttonModels[i];
                        }
                    }
                    if (selectedButtun != null)
                    {
                        for (int i = 0; i < buttonModels.Count; i++)
                        {
                            buttonModels[i].ChangeImage(TextureType.Normal);
                            buttonModels[i].GuiModel = null;
                        }
                        selectedButtun.ChangeImage(TextureType.Selected);
                        previousSelectedButtun = selectedButtun;
                        previousSelectedButtun.GuiModel = null;
                        previousSelectedButtun.GuiModel = new GUIModel(this.game) { Name = previousSelectedButtun.GuiModelName };
                        previousSelectedButtun.GuiModel.Initialize();

                        startPoint = new Vector2();
                        endPoint = new Vector2();
                    }
                }

                if (previousSelectedButtun != null && previousSelectedButtun.GuiModel.ModelSituation == ModelSituation.Drew)
                {
                    for (int i = 0; i < buttonConfirmations.Count; i++)
                    {
                        if (buttonConfirmations[i].isIntersec(currentMouseState.X, currentMouseState.Y))
                        {
                            selectedButtun = buttonConfirmations[i];
                        }
                    }
                    
                    if (selectedButtun != null)
                    {
                        if (selectedButtun.ButtonConfirmType == ButtonConfirmType.CANCEL)
                        {
                            this.scene.RemoveComponent(previousSelectedButtun.GuiModel);
                        }

                        previousSelectedButtun.ChangeImage(TextureType.Normal);
                        previousSelectedButtun.GuiModel = null;
                        previousSelectedButtun.GuiModel = new GUIModel(this.game) { Name = previousSelectedButtun.GuiModelName };
                        previousSelectedButtun.GuiModel.Initialize();
                        previousSelectedButtun = null;

                        startPoint = new Vector2();
                        endPoint = new Vector2();
                    }
                }
            }
            */
            previousMouseState = currentMouseState;
        }

        private Vector3 ScreenToWorld(int x, int y)
        {
            var p = new Plane(-Vector3.UnitZ, -10f);

            Vector3? point = this.scene.Camera.Unproject(x, y).IntersectsAt(p);
            return point ?? Vector3.Zero;
        }

        public Scene Scene { get { return scene; } set { this.scene = value; } }


        internal void Stop()
        {
            
        }

        internal void Run()
        {
            
        }
    }
}
