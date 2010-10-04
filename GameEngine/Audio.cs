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
        /// Random generator for multioption sounds
        /// </summary>
        static Random randomGenerator = new Random();

        static AudioCategory musicCategory;
        static AudioCategory dialogueCategory;

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

        /// <summary>
        /// End dialogue cue, used for several methods here.
        /// </summary>
        static Cue endDialogue;

        /// <summary>
        /// Introduction diialogue cue, used for several methods here.
        /// </summary>
        static Cue introductionDialogue;
        

        /// <summary>
        /// Hero dying music cue, used for several methods here.
        /// </summary>
        static Cue heroDying1;

        /// <summary>
        /// Hero dying music cue, used for several methods here.
        /// </summary>
        static Cue heroDying2;

        /// <summary>
        /// Attacking flower music cue, used for several methods here.
        /// </summary>
        static Cue attackingFlower1;

        /// <summary>
        /// Attacking flower music cue, used for several methods here.
        /// </summary>
        static Cue attackingFlower2;

        /// <summary>
        /// Attacking flower music cue, used for several methods here.
        /// </summary>
        static Cue attackingFlower3;

        /// <summary>
        /// Attacking flower music cue, used for several methods here.
        /// </summary>
        static Cue attackingFlower4;

        /// <summary>
        /// Dying flower music cue, used for several methods here.
        /// </summary>
        static Cue dyingFlower1;

        /// <summary>
        /// Dying flower music cue, used for several methods here.
        /// </summary>
        static Cue dyingFlower2;

        /// <summary>
        /// Dying flower music cue, used for several methods here.
        /// </summary>
        static Cue dyingFlower3;

        /// <summary>
        /// Dying flower music cue, used for several methods here.
        /// </summary>
        static Cue dyingFlower4;
        #endregion

        #region Constructor
        /// <summary>
        /// Create sounds
        /// </summary>
        static Audio()
        {
            // Loading audio
            audioEngine = new AudioEngine("Content\\Audio\\DreamerAudio.xgs");
            waveBank    = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank   = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");



            musicCategory = audioEngine.GetCategory("Music");
            musicCategory.SetVolume(0.5f);

            dialogueCategory = audioEngine.GetCategory("Dialogue");


            // Get music cues
            backgroundMusic = soundBank.GetCue("GameBackgroundMusic");

            menuClick = soundBank.GetCue("MenuClick");

            endDialogue = soundBank.GetCue("EndDialogue");
            introductionDialogue = soundBank.GetCue("IntroductionDialogue");

            heroDying1 = soundBank.GetCue("FranciscoHeroDying1");
            heroDying2 = soundBank.GetCue("FranciscoHeroDying2");

            attackingFlower1 = soundBank.GetCue("LukeAttackingFlower");
            attackingFlower2 = soundBank.GetCue("SarahAttackingFlower");
            attackingFlower3 = soundBank.GetCue("ShringiFlowerAttacking");
            attackingFlower4 = soundBank.GetCue("VolkanFlowerAttacking");

            dyingFlower1 = soundBank.GetCue("LukeDyingFlower");
            dyingFlower2 = soundBank.GetCue("SarahDyingFlower");
            dyingFlower3 = soundBank.GetCue("ShringiFlowerDying");
            dyingFlower4 = soundBank.GetCue("VolkanFlowerDying");



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

        /// <summary>
        /// Play end dialogue sound
        /// </summary>
        public static void PlayEndDialogue()
        {
            if (!endDialogue.IsPrepared)
            {
                endDialogue = soundBank.GetCue("EndDialogue");
            }
            endDialogue.Play();
        }

        /// <summary>
        /// Play introduction dialogue sound
        /// </summary>
        public static void PlayIntroductionDialogue()
        {
            if (!introductionDialogue.IsPrepared)
            {
                introductionDialogue = soundBank.GetCue("IntroductionDialogue");
            }
            introductionDialogue.Play();
        }

        /// <summary>
        /// Just a place holder for Play bunny dying sound
        /// </summary>
        /// 
        public static void PlayDyingBunny()
        {
            PlayDyingFlower();
        }

        /// <summary>
        /// Just a place holder for Play bunny attacking sound
        /// </summary>
        /// 
        public static void PlayAttackingBunny()
        {
            PlayAttackingFlower();
        }

        /// <summary>
        /// Play hero dying sound
        /// </summary>
        public static void PlayHeroDying()
        {
            PauseBackgroundMusic();
            int choice = randomGenerator.Next(2);
            switch (choice) {
                case 0: PlayHeroDying1(); break;
                case 1: PlayHeroDying2(); break;
            }

        }

        public static void PlayHeroDying1()
        {
            if (!heroDying1.IsPrepared)
            {
                heroDying1 = soundBank.GetCue("FranciscoHeroDying1");
            }
            heroDying1.Play();
        }

        public static void PlayHeroDying2()
        {
            if (!heroDying2.IsPrepared)
            {
                heroDying2 = soundBank.GetCue("FranciscoHeroDying2");
            }
            heroDying2.Play();
        }

        /// <summary>
        /// Play flower attacking sound
        /// </summary>
        public static void PlayAttackingFlower()
        {
            int choice = randomGenerator.Next(4);
            switch (choice)
            {
                case 0: PlayAttackingFlower1(); break;
                case 1: PlayAttackingFlower2(); break;
                case 2: PlayAttackingFlower3(); break;
                case 3: PlayAttackingFlower4(); break;  
            }
        }

        public static void PlayAttackingFlower1()
        {
            if (!attackingFlower1.IsPrepared)
            {
                attackingFlower1 = soundBank.GetCue("LukeAttackingFlower");
            }
            attackingFlower1.Play();
        }

        public static void PlayAttackingFlower2()
        {
            if (!attackingFlower2.IsPrepared)
            {
                attackingFlower2 = soundBank.GetCue("SarahAttackingFlower");
            }
            attackingFlower2.Play();
        }

        public static void PlayAttackingFlower3()
        {
            if (!attackingFlower3.IsPrepared)
            {
                attackingFlower3 = soundBank.GetCue("ShringiFlowerAttacking");
            }
            attackingFlower3.Play();
        }

        public static void PlayAttackingFlower4()
        {
            if (!attackingFlower4.IsPrepared)
            {
                attackingFlower4 = soundBank.GetCue("VolkanFlowerAttacking");
            }
            attackingFlower4.Play();
        }


        /// <summary>
        /// Play flower dying sound
        /// </summary>
        /// 
        public static void PlayDyingFlower()
        {
            int choice = randomGenerator.Next(4);
            switch (choice)
            {
                case 0: PlayDyingFlower1(); break;
                case 1: PlayDyingFlower2(); break;
                case 2: PlayDyingFlower3(); break;
                case 3: PlayDyingFlower4(); break;
            }
        }

        public static void PlayDyingFlower1()
        {
            if (!dyingFlower1.IsPrepared)
            {
                dyingFlower1 = soundBank.GetCue("LukeDyingFlower");
            }
            dyingFlower1.Play();
        }

        public static void PlayDyingFlower2()
        {
            if (!dyingFlower2.IsPrepared)
            {
                dyingFlower2 = soundBank.GetCue("SarahDyingFlower");
            }
            dyingFlower2.Play();
        }

        public static void PlayDyingFlower3()
        {
            if (!dyingFlower3.IsPrepared)
            {
                dyingFlower3 = soundBank.GetCue("ShringiFlowerDying");
            }
            dyingFlower3.Play();
        }

        public static void PlayDyingFlower4()
        {
            if (!dyingFlower4.IsPrepared)
            {
                dyingFlower4 = soundBank.GetCue("VolkanFlowerDying");
            }
            dyingFlower4.Play();
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