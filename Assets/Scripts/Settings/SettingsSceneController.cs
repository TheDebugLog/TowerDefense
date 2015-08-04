using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TDL
{
    public class SettingsSceneController : SceneController
    {
        public Slider musicVolumeSlider;

        void Start()
        {
            musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        }

        public void ChangeVolume()
        {
            AudioManager.Instance.ChangeMusicVolume(musicVolumeSlider.value);
        }
    }

}
