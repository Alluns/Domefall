using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider, soundSlider;
        
        private SaveData data;
        
        private void OnEnable()
        {
            data = JsonSave.LoadData();

            musicSlider.value = data.bgmVolume;
            soundSlider.value = data.sfxVolume;
        }

        public void ResetGameProgress()
        {
            JsonSave.DeleteSave();
            
            SaveData newData = JsonSave.LoadData();

            newData.bgmVolume = data.bgmVolume;
            newData.sfxVolume = data.sfxVolume;
            
            JsonSave.Save(newData);
            
            SceneManager.LoadScene(0);
        }

        public void SaveSettings()
        {
            data.bgmVolume = musicSlider.value;
            data.sfxVolume = soundSlider.value;
            
            JsonSave.Save(data);
        }
    }
}
