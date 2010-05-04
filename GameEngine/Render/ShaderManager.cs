// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShaderManager.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The shader manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Render
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class ShaderManager
    {
        public enum EFFECT_ID { NONE, OVERLAY, MODEL, LIGHTING, SHADOW, SHADOWMAP, SIMPLE, POINTLIGHT, ANIMATEDMODEL, STATICMODEL, PHYSICS, BACKGROUND, TEXTURE};
        #region Constants and Fields

        /// <summary>
        /// The current effect.
        /// </summary>
        private static Effect currentEffect;

        /// <summary>
        /// The effect dictionary.
        /// </summary>
        private static Dictionary<EFFECT_ID, Effect> effectDictionary;

        #endregion

        #region Public Methods

        /// <summary>
        /// The add effect.
        /// </summary>
        /// <param name="ID">
        /// The id.
        /// </param>
        /// <param name="effectName">
        /// The effect name.
        /// </param>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        public static void AddEffect(EFFECT_ID ID, string effectName, Microsoft.Xna.Framework.Game game)
        {
            if (effectDictionary == null)
            {
                effectDictionary = new Dictionary<EFFECT_ID, Effect>();
            }

            if (effectDictionary.ContainsKey(ID))
            {
                return;
            }

            bool found = Enum.GetValues(typeof(EFFECT_ID)).Cast<EFFECT_ID>().Any(i => i == ID);

            if (!found)
            {
                throw new Exception("Effect ID (" + ID + ") not found in EFFECT_IDS enum");
            }

            if (!effectName.StartsWith("Effects\\"))
                effectName = "Effects\\" + effectName;

            Effect e = game.Content.Load<Effect>(effectName);

            effectDictionary.Add(ID, e);
        }

        public static void AddEffect(EFFECT_ID ID, Effect effect)
        {
            if (effectDictionary == null)
                effectDictionary = new Dictionary<EFFECT_ID, Effect>();

            if (effectDictionary.ContainsKey(ID))
                return;

            bool found = Enum.GetValues(typeof(EFFECT_ID)).Cast<EFFECT_ID>().Any(i => i == ID);

            if (!found)
                throw new Exception("Effect ID (" + ID.ToString() + ") not found in EFFECT_IDS enum");

            if (effect == null)
                throw new ArgumentNullException("Effect must not be null");

            effectDictionary.Add(ID, effect);
        }

        /// <summary>
        /// The begin.
        /// </summary>
        public static void Begin()
        {
            currentEffect.Begin();
        }

        /// <summary>
        /// The commit changes.
        /// </summary>
        public static void CommitChanges()
        {
            currentEffect.CommitChanges();
        }

        /// <summary>
        /// The end.
        /// </summary>
        public static void End()
        {
            currentEffect.End();
        }

        /// <summary>
        /// The get boolean value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The get boolean value.
        /// </returns>
        public static bool GetBooleanValue(string name)
        {
            return currentEffect.Parameters[name].GetValueBoolean();
        }

        /// <summary>
        /// The get current effect.
        /// </summary>
        /// <returns>
        /// </returns>
        public static Effect GetCurrentEffect()
        {
            return currentEffect;
        }

        /// <summary>
        /// The get current effect graphics device.
        /// </summary>
        /// <returns>
        /// </returns>
        public static GraphicsDevice GetCurrentEffectGraphicsDevice()
        {
            return currentEffect.GraphicsDevice;
        }

        /// <summary>
        /// The get effect.
        /// </summary>
        /// <param name="ID">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static Effect GetEffect(EFFECT_ID ID)
        {
            return effectDictionary[ID];
        }

        /// <summary>
        /// The get effect passes.
        /// </summary>
        /// <returns>
        /// </returns>
        public static EffectPassCollection GetEffectPasses()
        {
            return currentEffect.CurrentTechnique.Passes;
        }

        /// <summary>
        /// The set current effect.
        /// </summary>
        /// <param name="ID">
        /// The id.
        /// </param>
        public static void SetCurrentEffect(EFFECT_ID ID)
        {
            currentEffect = ID == EFFECT_ID.NONE ? null : effectDictionary[ID];
        }

        /// <summary>
        /// The set current technique.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public static void SetCurrentTechnique(string name)
        {
            currentEffect.CurrentTechnique = currentEffect.Techniques[name];
        }

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetValue(string name, object value)
        {
            if (currentEffect == null)
            {
                return;
            }

            if (value is bool)
            {
                currentEffect.Parameters[name].SetValue((bool)value);
            }
            else if (value is bool[])
            {
                currentEffect.Parameters[name].SetValue((bool[])value);
            }
            else if (value is int)
            {
                currentEffect.Parameters[name].SetValue((int)value);
            }
            else if (value is int[])
            {
                currentEffect.Parameters[name].SetValue((int[])value);
            }
            else if (value is float)
            {
                currentEffect.Parameters[name].SetValue((float)value);
            }
            else if (value is float[])
            {
                currentEffect.Parameters[name].SetValue((float[])value);
            }
            else if (value is Matrix)
            {
                currentEffect.Parameters[name].SetValue((Matrix)value);
            }
            else if (value is Matrix[])
            {
                currentEffect.Parameters[name].SetValue((Matrix[])value);
            }
            else if (value is Quaternion)
            {
                currentEffect.Parameters[name].SetValue((Quaternion)value);
            }
            else if (value is Quaternion[])
            {
                currentEffect.Parameters[name].SetValue((Quaternion[])value);
            }
            else if (value is string)
            {
                currentEffect.Parameters[name].SetValue((string)value);
            }
            else if (value is Texture)
            {
                currentEffect.Parameters[name].SetValue((Texture)value);
            }
            else if (value is Vector2)
            {
                currentEffect.Parameters[name].SetValue((Vector2)value);
            }
            else if (value is Vector2[])
            {
                currentEffect.Parameters[name].SetValue((Vector2[])value);
            }
            else if (value is Vector3)
            {
                currentEffect.Parameters[name].SetValue((Vector3)value);
            }
            else if (value is Vector3[])
            {
                currentEffect.Parameters[name].SetValue((Vector3[])value);
            }
            else if (value is Vector4)
            {
                currentEffect.Parameters[name].SetValue((Vector4)value);
            }
            else if (value is Vector4[])
            {
                currentEffect.Parameters[name].SetValue((Vector4[])value);
            }
        }

        #endregion
    }
}