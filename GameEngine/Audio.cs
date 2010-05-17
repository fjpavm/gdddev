using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;


namespace Gdd.Game.Engine
{
    public class Audio
    {
        #region Variables
        /// <summary>
        /// Sound stuff for XAct
        /// </summary>
        static AudioEngine audioEngine;

        /// <summary>
        /// Wave bank
        /// </summary>
        static WaveBank waveBank;

        /// <summary>
        /// Sound bank
        /// </summary>
        static SoundBank soundBank;

        /// <summary>
        /// Background music cue, used for several methods here.
        /// </summary>
        static Cue backgroundMusic;

        /// <summary>
        /// Menu click music cue, used for several methods here.
        /// </summary>
        static Cue menuClick;
        #endregion

        #region Constructor
        /// <summary>
        /// Create sounds
        /// </summary>
        static Audio()
        {
            // Loading audio
            audioEngine = new AudioEngine("Content\\Audio\\DreamerAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            // Get background music cue
            backgroundMusic = soundBank.GetCue("GameBackgroundMusic");
            menuClick = soundBank.GetCue("MenuClick");

        }
        #endregion

        #region Play

        /// <summary>
        /// Play click sound
        /// </summary>
        public static void PlayClickSound()
        {
            if (!menuClick.IsPrepared)
            {
                menuClick = soundBank.GetCue("MenuClick");
            }
            menuClick.Play();
        }

        /// <summary>
        /// Play background music
        /// </summary>
        public static void PlayBackgroundMusic()
        {
            if (!backgroundMusic.IsPrepared)
            {
                backgroundMusic = soundBank.GetCue("GameBackgroundMusic");
            }
            backgroundMusic.Play();
        }
        #endregion

        #region Stop, Pause and Resume background music
        /// <summary>
        /// Stop background music
        /// </summary>
        public static void StopBackgroundMusic()
        {
            if (backgroundMusic.IsPlaying)
            {
                backgroundMusic.Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Pause background music
        /// </summary>
        public static void PauseBackgroundMusic()
        {
            if (backgroundMusic.IsPlaying)
            {
                backgroundMusic.Pause();
            }
        }

        /// <summary>
        /// Resume background music
        /// </summary>
        public static void ResumeBackgroundMusic()
        {
            if (backgroundMusic.IsPaused)
            {
                backgroundMusic.Resume();
            }
        }
        #endregion

        #region Play Background Music Repeated
        /// <summary>
        /// Play Background Music Repeated
        /// </summary>
        public static void RepeatPlayBackgroundMusic()
        {
            if (backgroundMusic.IsStopped)
            {
                PlayBackgroundMusic();
            }
        }
        #endregion
    }
}