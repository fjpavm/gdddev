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
    class Audio
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

        #endregion

        #region Enums
        /// <summary>
        /// Sounds we use in this game.
        /// </summary>
        /// <returns>Enum</returns>
        public enum Sounds
        {
            MenuClick,
            GameBackgroundMusic,
        }//enum Sounds
        #endregion 

        #region Constructor
        /// <summary>
        /// Create sounds
        /// </summary>
        static Audio()
        {
            // Loading audio
             string dir = "Content\\Audio";
             audioEngine = new AudioEngine(
                 Path.Combine(dir, "DreamerAudio.xgs"));
             waveBank = new WaveBank(audioEngine, 
                 Path.Combine(dir, "Wave Bank.xwb"));
             if (waveBank != null)
                 soundBank = new SoundBank(audioEngine, 
                    Path.Combine(dir, "Sound Bank.xsb"));
        }
        #endregion

        #region Play and Stop
        /// <summary>
        /// Play
        /// </summary>
        /// <param name="soundName">Sound name</param>
        public static void Play(string soundName)
        {
          soundBank.PlayCue(soundName);
        } // Play(soundName)
        
        /// <summary>
        /// Play
        /// </summary>
        /// <param name="sound">Sound</param>
        public static void Play(Sounds sound)
        {
            Play(sound.ToString());
        } // Play(sound)

        /// <summary>
        /// Play click sound
        /// </summary>
        public static void PlayClickSound()
        {
            Play(Sounds.MenuClick);
        }

        /// <summary>
        /// Play background music
        /// </summary>
        public static void PlayBackgroundMusic()
        {
            Play(Sounds.GameBackgroundMusic);
        }
        #endregion
    }
}