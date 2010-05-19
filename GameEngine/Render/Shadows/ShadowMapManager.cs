using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gdd.Game.Engine.Levels;

// code taken from ShadowMapping example from Microsoft
// Effect code taken from http://www.docstoc.com/docs/18464364/Shadowing-in-XNA

namespace Gdd.Game.Engine.Render.Shadow
{
    using Scenes;
    using Levels;
    using Common;

    public class ShadowMapManager
    {

        public static Texture2D CalculateShadowMapForScene(GraphicsDevice graphicsDevice, RenderTarget2D renderTarget, DepthStencilBuffer depthBuffer, Vector3 lightDirection, BoundingSphere bounds, int SceneId)
        {
            DepthStencilBuffer old = SetupShadowMap(graphicsDevice, ref renderTarget, ref depthBuffer);

            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.SHADOWMAP);
            ShaderManager.SetValue("LightProj", CalcLightProjection(bounds, lightDirection, graphicsDevice.Viewport));
            ShaderManager.SetValue("LightView", Matrix.CreateLookAt(lightDirection * bounds.Radius + bounds.Center, bounds.Center, Vector3.Up));

            foreach (DrawableSceneComponent
                dsc in ObjectManager.drawableSceneComponents[SceneId])
            {
                if (!dsc.Visible)
                    continue;

                if (dsc is AnimatedModel)
                    dsc.DrawWithEffect(ShaderManager.EFFECT_ID.SHADOWMAP, "ShadowMapRenderAnimation");
                else
                    dsc.DrawWithEffect(ShaderManager.EFFECT_ID.SHADOWMAP, "ShadowMapRenderStatic");
            }
            ResetGraphicsDevice(graphicsDevice, old);

            return renderTarget.GetTexture();
        }

        public static void ResetGraphicsDevice(GraphicsDevice graphicsDevice, DepthStencilBuffer old)
        {
            // Set render target back to the back buffer
            graphicsDevice.SetRenderTarget(0, null);
            // Reset the depth buffer
            graphicsDevice.DepthStencilBuffer = old;
            // Reset the CullMode and AlphaBlendEnable 
            // that the previous effect changed
            graphicsDevice.RenderState.CullMode =
                CullMode.CullCounterClockwiseFace;
            graphicsDevice.RenderState.AlphaBlendEnable = true;

            // Render our scene
            graphicsDevice.Clear(Color.CornflowerBlue);
        }

        public static DepthStencilBuffer SetupShadowMap(GraphicsDevice graphicsDevice, ref RenderTarget2D renderTarget, ref DepthStencilBuffer depthBuffer)
        {	
            //    System.Console.Out.Write(gameTime.TotalGameTime);
            // Set the depth buffer function that best fits our stencil type
            // and projection (a reverse projection would use GreaterEqual)
            graphicsDevice.RenderState.DepthBufferFunction =
                CompareFunction.LessEqual;

            // Set Render Target for shadow map
            graphicsDevice.SetRenderTarget(0, renderTarget);
            // Cache the current depth buffer
            DepthStencilBuffer old = graphicsDevice.DepthStencilBuffer;
            // Set our custom depth buffer
            graphicsDevice.DepthStencilBuffer = depthBuffer;

            // Render the shadow map
            graphicsDevice.Clear(Color.Black);

            return old;
        }

        public static DepthStencilBuffer SetupShadowMap(GraphicsDevice graphicsDevice, ref RenderTarget2D renderTarget, ref DepthStencilBuffer depthBuffer, Color clearColor)
        {
            //    System.Console.Out.Write(gameTime.TotalGameTime);
            // Set the depth buffer function that best fits our stencil type
            // and projection (a reverse projection would use GreaterEqual)
            graphicsDevice.RenderState.DepthBufferFunction =
                CompareFunction.LessEqual;

            // Set Render Target for shadow map
            graphicsDevice.SetRenderTarget(0, renderTarget);
            // Cache the current depth buffer
            DepthStencilBuffer old = graphicsDevice.DepthStencilBuffer;
            // Set our custom depth buffer
            graphicsDevice.DepthStencilBuffer = depthBuffer;

            // Render the shadow map
            graphicsDevice.Clear(clearColor);

            return old;
        }

        public static Matrix CalcLightProjection(BoundingSphere bounds, Vector3 lightDir, Viewport viewport)
        {
            Vector3 temp = lightDir;

            // The angle between the tangent and the center of the bounds
            // is half of our field of view
            float near = temp.Length() - bounds.Radius;
            float far = temp.Length() + bounds.Radius;

            // If the light actually gets into the scene, the projection
            // matrix could throw an exception.  These clamping operations
            // prevent that
            near = MathHelper.Max(near, 0.01f);

            if (near == far)
            {
                return Matrix.Identity;
            }

            return Matrix.CreateOrthographic(0.25f*bounds.Radius, 2*bounds.Radius, near, far);
        }

        private static double AngleBetweenVectors(Vector3 first, Vector3 second)
        {
            float dot = Vector3.Dot(first, second);
            float magnitude = first.Length() * second.Length();
            return Math.Acos(dot / magnitude);
        }

    }
}
