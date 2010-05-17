// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedModelProcessor.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   Custom processor extends the builtin framework ModelProcessor class,
//   adding animation support.
//   -----------------------------------------------------------------------------
//   SkinnedModelProcessor.cs
//   Microsoft XNA Community Game Platform
//   Copyright (C) Microsoft Corporation. All rights reserved.
//   Amended by Olafur Thor Gunnarsson.
//   -----------------------------------------------------------------------------
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Content.Pipeline.Processors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    using Gdd.Game.Engine.Animation;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
    using Microsoft.Xna.Framework.Content.Pipeline.Processors;
    using System.Diagnostics;

    /// <summary>
    /// Custom processor extends the builtin framework ModelProcessor class,
    /// adding animation support.
    /// </summary>
    [ContentProcessor(DisplayName = "PipelineProcessors.AnimatedModelProcessor")]
    public class AnimatedModelProcessor : ModelProcessor
    {
        // Maximum number of bone matrices we can render using shader 2.0
        // in a single pass. If you change this, update SkinnedModelfx to match.
        #region Constants and Fields

        /// <summary>
        /// The max bones.
        /// </summary>
        private const int MaxBones = 49;

        /// <summary>
        /// The animation xml.
        /// </summary>
        private static XmlDocument animationXML;

        #endregion

        #region Public Methods

        /// <summary>
        /// The main Process method converts an intermediate format content pipeline
        /// NodeContent tree to a ModelContent object with embedded animation data.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            ValidateMesh(input, context, null);

            // Find the skeleton.
            BoneContent skeleton = MeshHelper.FindSkeleton(input);
            if (skeleton == null)
            {
                throw new InvalidContentException("Input skeleton not found.");
            }

            // We don't want to have to worry about different parts of the model being
            // in different local coordinate systems, so let's just bake everything.
            FlattenTransforms(input, skeleton);

            // Read the bind pose and skeleton hierarchy data.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > MaxBones)
            {
                throw new InvalidContentException(
                    string.Format("Skeleton has {0} bones, but the maximum supported is {1}.", bones.Count, MaxBones));
            }

            var bindPose = new List<Matrix>();
            var inverseBindPose = new List<Matrix>();
            var skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // Chain to the base ModelProcessor class so it can convert the model data.
            ModelContent model = base.Process(input, context);

            // Convert animation data to our runtime format.
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones, context, input.Identity.SourceFilename);

            // Store our custom animation data in the Tag property of the model.
            model.Tag = new SkinningData(animationClips, bindPose, inverseBindPose, skeletonHierarchy);

            return model;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes all the materials to use our skinned model effect.
        /// </summary>
        /// <param name="material">
        /// The material.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            var basicMaterial = material as BasicMaterialContent;

            if (basicMaterial == null)
            {
                throw new InvalidContentException(
                    string.Format(
                        "SkinnedModelProcessor only supports BasicMaterialContent, " + "but input mesh uses {0}.", 
                        material.GetType()));
            }

            var effectMaterial = new EffectMaterialContent();

            // Store a reference to our skinned mesh effect.
            string effectPath = string.Empty;

            if (animationXML != null)
            {
                // get all animations
                XmlNodeList shaderList = animationXML.GetElementsByTagName("shader");
                foreach (XmlNode shadernode in shaderList)
                {
                    effectPath = Path.GetFullPath(shadernode.InnerText.Trim());
                }
            }

            if (String.IsNullOrEmpty(effectPath))
            {
                // Store a reference to our skinned mesh effect.
                effectPath = Path.GetFullPath("Effects\\ShadowMap.fx");
            }

            effectMaterial.Effect = new ExternalReference<EffectContent>(effectPath);

            // Copy texture settings from the input
            // BasicMaterialContent over to our new material.
            if (basicMaterial.Texture != null)
            {
                effectMaterial.Textures.Add("Texture", basicMaterial.Texture);
            }

            // Chain to the base ModelProcessor converter.
            return base.ConvertMaterial(effectMaterial, context);
        }

        /// <summary>
        /// Comparison function for sorting keyframes into ascending time order.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The compare keyframe times.
        /// </returns>
        private static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }

        /// <summary>
        /// Bakes unwanted transforms into the model geometry,
        /// so everything ends up in the same coordinate system.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="skeleton">
        /// The skeleton.
        /// </param>
        private static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                {
                    continue;
                }

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }

        /// <summary>
        /// Checks whether a mesh contains skininng information.
        /// </summary>
        /// <param name="mesh">
        /// The mesh.
        /// </param>
        /// <returns>
        /// The mesh has skinning.
        /// </returns>
        private static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContent
        /// object to our runtime AnimationClip format.
        /// </summary>
        /// <param name="animation">
        /// The animation.
        /// </param>
        /// <param name="boneMap">
        /// The bone Map.
        /// </param>
        /// <param name="keyframeStart">
        /// The keyframe Start.
        /// </param>
        /// <param name="keyframeEnd">
        /// The keyframe End.
        /// </param>
        private static AnimationClip ProcessAnimation(
            AnimationContent animation, Dictionary<string, int> boneMap, int keyframeStart, int keyframeEnd)
        {
            // map of a list of keyframes
            var keyframes = new Dictionary<int, List<Keyframe>>();

             Debugger.Launch();

            // For each input animation channel.
            foreach (var channel in animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(
                        string.Format(
                            "Found animation for bone '{0}', " + "which is not part of the skeleton.", channel.Key));
                }

                int keyframeNr = 0;
                int count = 0;

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    if (count <= keyframeEnd && count >= keyframeStart)
                    {
                        if (!keyframes.ContainsKey(keyframeNr))
                        {
                            keyframes[keyframeNr] = new List<Keyframe>();
                        }

                        keyframes[keyframeNr++].Add(new Keyframe(boneIndex, keyframe.Time, keyframe.Transform));
                    }

                    count++;
                }
            }

            // Sort the merged keyframes by time.
            foreach (int i in keyframes.Keys)
            {
                keyframes[i].Sort(CompareKeyframeTimes);
            }

            if (keyframes.Count == 0)
            {
                throw new InvalidContentException("Animation has no keyframes.");
            }

            if (animation.Duration <= TimeSpan.Zero)
            {
                throw new InvalidContentException("Animation has a zero duration.");
            }

            return new AnimationClip(animation.Duration, keyframes);
        }

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContent
        /// object to our runtime AnimationClip format.
        /// </summary>
        /// <param name="animation">
        /// The animation.
        /// </param>
        /// <param name="boneMap">
        /// The bone Map.
        /// </param>
        private static AnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            var keyframes = new Dictionary<int, List<Keyframe>>();

            // For each input animation channel.
            foreach (var channel in animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(
                        string.Format(
                            "Found animation for bone '{0}', " + "which is not part of the skeleton.", channel.Key));
                }

                int keyframeNr = 0;

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    if (!keyframes.ContainsKey(keyframeNr))
                    {
                        keyframes[keyframeNr] = new List<Keyframe>();
                    }

                    keyframes[keyframeNr++].Add(new Keyframe(boneIndex, keyframe.Time, keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            foreach (int i in keyframes.Keys)
            {
                keyframes[i].Sort(CompareKeyframeTimes);
            }

            if (keyframes.Count == 0)
            {
                throw new InvalidContentException("Animation has no keyframes.");
            }

            if (animation.Duration <= TimeSpan.Zero)
            {
                throw new InvalidContentException("Animation has a zero duration.");
            }

            return new AnimationClip(animation.Duration, keyframes);
        }

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContentDictionary
        /// object to our runtime AnimationClip format.
        /// </summary>
        /// <param name="animations">
        /// The animations.
        /// </param>
        /// <param name="bones">
        /// The bones.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        private static Dictionary<string, AnimationClip> ProcessAnimations(
            AnimationContentDictionary animations, 
            IList<BoneContent> bones, 
            ContentProcessorContext context, 
            string source)
        {
            // Build up a table mapping bone names to indices.
            var boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                {
                    boneMap.Add(boneName, i);
                }
            }

            // Convert each animation in turn.
            Dictionary<string, AnimationClip> animationClips;
            animationClips = new Dictionary<string, AnimationClip>();

            string xmlFileName = source.Substring(0, source.IndexOf(".fbx")) + "Animation.xml";

            if (File.Exists(xmlFileName))
            {
                animationXML = new XmlDocument();
                animationXML.Load(xmlFileName);

                // get all animations
                XmlNodeList animationList = animationXML.GetElementsByTagName("animation");

                string name = string.Empty;
                int keyframestart = -1, keyframeend = -1;

                foreach (XmlNode animationNode in animationList)
                {
                    foreach (XmlNode child in animationNode.ChildNodes)
                    {
                        if (child.Name == "name")
                        {
                            name = child.InnerText;
                        }
                        else if (child.Name == "startframe")
                        {
                            keyframestart = int.Parse(child.InnerText);
                        }
                        else if (child.Name == "endframe")
                        {
                            keyframeend = int.Parse(child.InnerText);
                        }
                    }

                    foreach (var animation in animations)
                    {
                        AnimationClip processed = ProcessAnimation(animation.Value, boneMap, keyframestart, keyframeend);

                        animationClips.Add(name, processed);
                    }
                }
            }
            else
            {
                foreach (var animation in animations)
                {
                    AnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                    animationClips.Add(animation.Key, processed);
                }
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException("Input file does not contain any animations.");
            }

            return animationClips;
        }

        /// <summary>
        /// Makes sure this mesh contains the kind of data we know how to animate.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="parentBoneName">
        /// The parent Bone Name.
        /// </param>
        private static void ValidateMesh(NodeContent node, ContentProcessorContext context, string parentBoneName)
        {
            var mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(
                        null, 
                        null, 
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.", 
                        mesh.Name, 
                        parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(
                        null, null, "Mesh {0} has no skinning information, so it has been deleted.", mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
            {
                ValidateMesh(child, context, parentBoneName);
            }
        }

        #endregion
    }
}