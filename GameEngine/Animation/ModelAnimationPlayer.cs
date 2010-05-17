#region File Description

//-----------------------------------------------------------------------------

// AnimationPlayer.cs

//

// Microsoft XNA Community Game Platform

// Copyright (C) Microsoft Corporation. All rights reserved.

// amended by Olafur Thor Gunnarsson to implement step/stop and resume animations

//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#endregion

namespace Gdd.Game.Engine.Animation
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class ModelAnimationPlayer
    {
        #region Fields
        
        // Information about the currently playing animation clip.
        AnimationClip currentClipValue;
        TimeSpan currentTimeValue;
        Boolean isStopped;
        Boolean runOnce;

        // Current animation transform matrices.
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;


        // Backlink to the bind pose and skeleton hierarchy data.
        SkinningData skinningDataValue;

        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public ModelAnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            isStopped = true;
            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }

        public void SetClip(AnimationClip clip)
        {
            currentClipValue = clip;
        }


        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip()
        {
            SetupAnimationRun();

            runOnce = false;
        }

        public void RunOnce()
        {
            SetupAnimationRun();

            runOnce = true;
        }

        public void RunUntilEnd(){
            runOnce = true;
        }

        private void SetupAnimationRun()
        {
            if (currentClipValue == null)
                throw new ArgumentNullException("Please set the currentClipValue with the SetClip function");

            isStopped = false;

            currentTimeValue = currentClipValue.Keyframes[0][0].Time;
            CurrentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        /// <summary>
        /// Stops the Clip
        /// </summary>
        public void PauseClip()
        {
            if (currentClipValue == null)
                throw new ArgumentNullException("clip");

            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            isStopped = true;
        }

        /// <summary>
        /// Stops the Clip
        /// </summary>
        public void StopClip()
        {
            if (currentClipValue == null)
                throw new ArgumentNullException("clip");

            currentTimeValue = CurrentClip.Keyframes[0][0].Time;
            CurrentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);

            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();

            isStopped = true;
        }

        public void ResumeClip()
        {
            isStopped = false;
        }
        
        /// <summary>
        /// Steps the Clip
        /// </summary>
        public void StepClip()
        {
            if (currentClipValue == null)
                throw new ArgumentNullException("clip");

            if (CurrentKeyframe >= currentClipValue.Keyframes.Count - 1)
            {
                currentTimeValue = CurrentClip.Keyframes[0][0].Time;
                CurrentKeyframe = 0;
            }

            foreach (Keyframe k in currentClipValue.Keyframes[CurrentKeyframe])
            {
                // Use this keyframe.
                boneTransforms[k.Bone] = k.Transform;
            }

            CurrentKeyframe++;

            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();
            
            isStopped = true;
        }

        public bool IsAtTheStart()
        {
            return CurrentKeyframe == 1;
        }

        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime, Matrix rootTransform)
        {
            if (isStopped)
            {
                time = TimeSpan.Zero;
                return;
            }

            if ( CurrentKeyframe > CurrentClip.Keyframes.Count || currentTimeValue >= CurrentClip.Keyframes[CurrentClip.Keyframes.Count-1][0].Time)
            {
                if (!runOnce)
                {
                    CurrentKeyframe = 1;
                    currentTimeValue = CurrentClip.Keyframes[CurrentKeyframe][0].Time;

                    skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
                }
                else
                {
                    isStopped = true;
                }
            }            

            UpdateBoneTransforms();
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
            
            currentTimeValue += new TimeSpan((long)(time.Ticks * 0.8f));
            
        }

        /// <summary>
        /// Helper used by the Update method to refresh the BoneTransforms data.
        /// </summary>
        public void UpdateBoneTransforms()
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                    "AnimationPlayer.Update was called before StartClip");

            for (int i = 0; i < CurrentClip.Keyframes.Count; i++)
            {
                foreach (Keyframe k in CurrentClip.Keyframes[i])
                {
                    if (k.Time > currentTimeValue)
                    {
                        CurrentKeyframe = i;
                        i = CurrentClip.Keyframes.Count;
                        break;
                    }

                    boneTransforms[k.Bone] = k.Transform;
                }
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                        worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                       worldTransforms[bone];
            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        /// <summary>
        /// Gets if the animation has stopped
        /// </summary>
        public bool IsStopped
        {
            get { return isStopped; }
        }

        /// <summary>
        /// Gets the current keyframe
        /// </summary>
        public int CurrentKeyframe
        {
            get;
            private set;
        }
    }
}


