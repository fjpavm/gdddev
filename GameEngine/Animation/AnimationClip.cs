using Microsoft.Xna.Framework.Graphics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames;
#region File Description

//-----------------------------------------------------------------------------

// AnimationClip.cs

//

// Microsoft XNA Community Game Platform

// Copyright (C) Microsoft Corporation. All rights reserved.

//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

namespace Gdd.Game.Engine.Animation
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Content;
    using FarseerGames.FarseerPhysics.Dynamics;
   

    /// <summary>
    /// An animation clip is the runtime equivalent of the
    /// Microsoft.Xna.Framework.Content.Pipeline.Graphics.AnimationContent type.
    /// It holds all the keyframes needed to describe a single animation.
    /// </summary>
    public class AnimationClip
    {
        /// <summary>
        /// Constructs a new animation clip object.
        /// </summary>
        public AnimationClip(TimeSpan duration, Dictionary<int, List<Keyframe>> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }

        /// <summary>
        /// Private constructor for use by the XNB deserializer.
        /// </summary>
        private AnimationClip()
        {
        }

        /// <summary>
        /// Gets the total length of the animation.
        /// </summary>
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


        /// <summary>
        /// Gets a combined list containing all the keyframes for all bones,
        /// sorted by time.
        /// </summary>
        [ContentSerializer]
        public Dictionary<int, List<Keyframe>> Keyframes { get; private set; }

        [ContentSerializerIgnore]
        public Dictionary<int, Vertices>[] vertices{ get; set; }
    }
}


