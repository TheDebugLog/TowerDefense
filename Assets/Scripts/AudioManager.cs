using UnityEngine;
using System.Collections;

namespace TDL
{
    /// <summary>
    /// Handles all related to the audio of the application. It is the only class that can modify the AudioSource that contains the music.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        AudioSource _applicationMusic;
        static AudioManager _instance = null;
        #endregion

        private AudioManager() { }

        #region Methods
        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _applicationMusic = GetComponent<AudioSource>();
            //Get the volume from the IOManager
        }

        /// <summary>
        /// Changes the volume of the game music
        /// </summary>
        /// <param name="newVolume">New volume of the music</param>
        public void ChangeMusicVolume(float newVolume)
        {
            if (0 <= newVolume && newVolume <= 1)
            {
                _applicationMusic.volume = newVolume;
            }
            else
            {
                Debug.Log("The volume of the music should be a number between 0 and 1. You entered: " + newVolume.ToString());
            }
        }

        public void SaveVolume()
        { 
            //TODO tell the IOManager to save the volume in the player settings
        }
        #endregion

        #region Properties
        public static AudioManager Instance
        {
            get 
            {
                return _instance;
            }
        }

        public float MusicVolume 
        {
            get
            {
                return _applicationMusic.volume;
            }
        }
        #endregion
    }

}
