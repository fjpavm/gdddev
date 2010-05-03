using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gdd.Game.Engine.Render;
using FarseerGames.FarseerPhysics.Collisions;
using Gdd.Game.Engine.Render.Shadow;
using FarseerGames.FarseerPhysics.Factories;

namespace Gdd.Game.Engine.Physics
{
    using Animation;
    using Levels;
    using Render.Shadow;
    using FarseerGames.FarseerPhysics.Dynamics;

    public class ModelToVertices
    {
        /// <summary>
        /// The Rendertarget
        /// </summary>
        private static RenderTarget2D renderTarget;

        ///
        /// The width and height of the texture
        /// 
        public static Vector2 TextureSize = new Vector2(256, 256);

        /// <summary>
        /// The DepthBuffer
        /// </summary>
        private static DepthStencilBuffer depthBuffer;

        private static Vector3 physicsLookat = new Vector3(0.0f, 0.0f, 0.0f);
        private static Vector3 physicsPos = new Vector3(0.0f, 0.0f, 150.0f);

        public static Vertices TransformStaticModel(StaticModel model, Microsoft.Xna.Framework.Game game)
        {
            renderTarget = GfxComponent.CreateCustomRenderTarget(game.GraphicsDevice, 1, SurfaceFormat.Color, MultiSampleType.None, (int)TextureSize.X, (int)TextureSize.Y);
            depthBuffer = GfxComponent.CreateDepthStencil(renderTarget);
            Texture2D tex;
            Vertices v = GetVertices(RenderToTarget(model, null, game, out tex), model.ObjectModel.Meshes[0].BoundingSphere);
            renderTarget.Dispose();
            renderTarget = null;
            return v;
        }

        public static void TransformAnimatedModel(AnimatedModel model, Microsoft.Xna.Framework.Game game)
        {
            SkinningData skinningData = (SkinningData)model.ObjectModel.Tag;

            renderTarget = GfxComponent.CreateCustomRenderTarget(game.GraphicsDevice, 1, SurfaceFormat.Color, MultiSampleType.None, (int)TextureSize.X, (int)TextureSize.Y);
            depthBuffer = GfxComponent.CreateDepthStencil(renderTarget);
            Texture2D tex;            
            Color[] reverseTexture = new Color[(int)(TextureSize.X * TextureSize.Y)];
            Color[] originalTexture = new Color[(int)(TextureSize.X * TextureSize.Y)];

            Vertices vertices;

            ModelAnimationPlayer player = new ModelAnimationPlayer(skinningData);
            foreach(AnimationClip clip in skinningData.AnimationClips.Values){
                clip.vertices = new Dictionary<int, Vertices>[]{new Dictionary<int, Vertices>(), new Dictionary<int, Vertices>()};
                //clip.textures = new Dictionary<int, Texture2D>[]{new Dictionary<int, Texture2D>(), new Dictionary<int, Texture2D>()};
                
        /*        clip.bodies = new Dictionary<int, Body>[] { new Dictionary<int, Body>(), new Dictionary<int, Body>() };
                clip.geometries = new Dictionary<int, Geom>[] { new Dictionary<int, Geom>(), new Dictionary<int, Geom>() };
                */
                player.SetClip(clip);
                bool hasGoneThrough = false;
                int lastKeyFrame = 0;


                while(!hasGoneThrough){
                    RenderToTarget(model, player, game, out tex);
                    vertices = GetVertices(tex, model.ObjectModel.Meshes[0].BoundingSphere);

                    if (vertices.Count != 1)
                    {
                         clip.vertices[(int)AnimatedModel.DIRECTION.RIGHT][player.CurrentKeyframe] = GetVertices(tex, model.ObjectModel.Meshes[0].BoundingSphere);
                        
                        // flip the texture and the vertices
                        tex.GetData<Color>(originalTexture);
                        int left = 0;
                        for (int right = (int)TextureSize.X - 1; right >= 0; right--, left++)
                        {
                            for (int j = 0; j < TextureSize.Y; j++)
                            {
                                reverseTexture[left + j * (int)TextureSize.X] = originalTexture[right + j * (int)TextureSize.X];
                            }
                        }

                        tex.SetData<Color>(reverseTexture);
                        vertices = GetVertices(tex, model.ObjectModel.Meshes[0].BoundingSphere);

                        clip.vertices[(int)AnimatedModel.DIRECTION.LEFT][player.CurrentKeyframe] = GetVertices(tex, model.ObjectModel.Meshes[0].BoundingSphere);                    
                        lastKeyFrame = player.CurrentKeyframe;                        
                    }
                    player.StepClip();
                    hasGoneThrough = player.CurrentKeyframe == 1 && lastKeyFrame != 0;
                }
            }
        }

        private static Vertices GetVertices(Texture2D texture, BoundingSphere bs)
        {
            bs.Center.X = bs.Center.Z;
            bs.Radius *= 1.5f;
            Vertices textureVertices = new Vertices();



            // do marching squares
            Vertices v = new Vertices();
            v.AddRange(MarchingSquare.DoMarch(texture).ToArray());
            
            // transform the vectors
            for(int i = 0; i<v.Count; i++)                
            {
                v[i] = new Vector2(v[i].X * 2 * bs.Radius / (texture.Width - 1) + bs.Center.X - bs.Radius, (texture.Height - 1 - v[i].Y) * 2 * bs.Radius / (texture.Height - 1) + bs.Center.Y - bs.Radius);
            }


            List<Vector2> points = new List<Vector2>(v);

            for (int i = 3; i < points.Count; i++)
            {
                if (isColinear(points[i - 2], points[i - 1], points[i]))
                {
                    points.Remove(points[i - 1]);
                    i--;
                }
            }

            v.Clear();
            v.AddRange(points.ToArray());

            return v; 
        }

        private static bool isColinear(Vector2 p1, Vector2 p2, Vector2 p3){

            float triangleArea = 0.5f*(p1.X * (p2.Y - p3.Y) - p2.X * (p1.Y - p3.Y) + p3.X*(p1.Y - p2.Y));
            float delta = 0.01f;

            return Math.Abs(triangleArea) < delta;
        }

        // this method was taken from http://karnbianco.co.uk/blog/2009/08/18/pseudo-2d-convex-hull-generation/
        public static float AngleBetweenVectors(Vector2 a, Vector2 b)
        {
            float angle = (float)Math.Atan2((double)(a.Y - b.Y),
                                             (double)(a.X - b.X));
            return angle;
        }

        private static Texture2D RenderToTarget(StaticModel model, ModelAnimationPlayer player, Microsoft.Xna.Framework.Game game, out Texture2D Sillouette)
        {
            DepthStencilBuffer old = ShadowMapManager.SetupShadowMap(game.GraphicsDevice, ref renderTarget, ref depthBuffer);

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.PHYSICS, "PhysicsRenderer", game);
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.PHYSICS);
            ShaderManager.SetValue("World", model.Rotation);
                        
            Matrix m = Matrix.Identity;
            m.M33 = 0;
            m.M43 = 0.5f;
            ShaderManager.SetValue("Projection", m);
            BoundingSphere bs = model.ObjectModel.Meshes[0].BoundingSphere;
            bs.Center.X = bs.Center.Z;
            bs.Radius *= 1.5f;

            Matrix view;
            view = Matrix.CreateTranslation(-bs.Center) * Matrix.CreateScale(1 / bs.Radius, 1 / bs.Radius, 1);
                
            ShaderManager.SetValue("Projection", m);
            ShaderManager.SetValue("View", view);
            ShaderManager.SetValue("usingBones", player != null);

            if(player != null){
                ShaderManager.SetValue("Bones", player.GetSkinTransforms());
            }

            ShaderManager.CommitChanges();
            List<Effect> effects = new List<Effect>();

            game.GraphicsDevice.Clear(Color.Black);

            foreach(ModelMesh mesh in model.ObjectModel.Meshes){
                effects.Clear();

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    effects.Add(part.Effect);
                    part.Effect = ShaderManager.GetCurrentEffect();
                }
                mesh.Draw();
                for(int i = 0; i<mesh.MeshParts.Count; i++)
                {
                    mesh.MeshParts[i].Effect = effects[i];
                }            
            }

            ShadowMapManager.ResetGraphicsDevice(game.GraphicsDevice, old);

            Color[] textureColors = new Color[(int)(TextureSize.X * TextureSize.Y)];
            renderTarget.GetTexture().GetData<Color>(textureColors);

            Sillouette = new Texture2D(game.GraphicsDevice, (int)TextureSize.X, (int)TextureSize.Y);
            Sillouette.SetData<Color>(textureColors);
            return renderTarget.GetTexture();
        }
    }
}
