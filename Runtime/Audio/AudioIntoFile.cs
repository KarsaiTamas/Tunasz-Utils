using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TunaszUtils
{
    [System.Serializable]
    public class SaveAbleSoundLevel
    {
        public string name;
        public float volume;

        public SaveAbleSoundLevel(string name, float audioLevel)
        {
            this.name = name;
            this.volume = audioLevel;
        }

        public SaveAbleSoundLevel(string name)
        {
            this.name = name;
            volume = 0;
        }
    }

    [System.Serializable]
    public class AudioGroups
    {
        public List<SaveAbleSoundLevel> saveableAudioGroups;
        public SaveAbleSoundLevel this[System.Enum i]
        {
            get => saveableAudioGroups[System.Convert.ToInt32(i)];
            set => saveableAudioGroups[System.Convert.ToInt32(i)]=value;
        }
        public SaveAbleSoundLevel this[int i]
        {
            get => saveableAudioGroups[i];
            set => saveableAudioGroups[i] = value;
        }
        public AudioGroups()
        {
            this.saveableAudioGroups = new List<SaveAbleSoundLevel>();
        }
    }
    [System.Serializable]
    public class AudioIntoFile:MonoBehaviour
    {

        [MonoScript(type = typeof(System.Enum))]
        public string componentTypeName;
        public static AudioIntoFile instance;
        public AudioGroups audioVolumeToSave;
        public AudioMixer audioMixer;
        public List<Slider> audioVolume;
        public AudioMixerGroup[] AllMixerGroups
        {
            get
            {
                return audioMixer.FindMatchingGroups(string.Empty);
                
            }
        }
        public bool changeHappened = false;
        public void Awake()
        {
            instance = this;
            LoadAudio();
        }
        public void OnValidate()
        {
            if (componentTypeName == null) return;
            var audioType = EnumExtenstions.GetWithOrder(System.Type.GetType(componentTypeName)).ToList();
            audioVolumeToSave = new AudioGroups();

            foreach (var item in audioType)
            {
                audioVolumeToSave.saveableAudioGroups.Add(new SaveAbleSoundLevel( item));
            }
            audioVolume = new List<Slider>();
            foreach (Transform item in transform)
            {
                audioVolume.Add(item.GetComponent<Slider>());
            }
        }
        public void LoadAudio()
        {
            audioVolumeToSave = new AudioGroups();
            var audioType = EnumExtenstions.GetWithOrder(System.Type.GetType(componentTypeName)).ToList();
            var defaultAudio=new AudioGroups();
            foreach (var item in audioType)
            {
                defaultAudio.saveableAudioGroups.Add(new SaveAbleSoundLevel(item));
            }
            audioVolumeToSave = SLHandler.Load(defaultAudio, "AudioVolume");
            for (int i = 0; i < audioType.Count; i++)
            {
                string item = audioType[i];
                audioMixer.SetFloat(item, audioVolumeToSave[i].volume);
            }
            for (int i = 0; i < audioVolume.Count; i++)
            {
                audioVolume[i].value =  audioVolumeToSave[i].volume+80;
            }
            changeHappened = false;

        }
        public void ChangeHappened()
        {
            changeHappened = true;
        }
        public void SaveAudio()
        {
            for (int i = 0; i < audioVolume.Count; i++)
            {
                audioVolumeToSave[i].volume=audioVolume[i].value-80;
                print(audioVolumeToSave[i].name+ audioVolumeToSave[i].volume);
                audioMixer.SetFloat(audioVolumeToSave[i].name, audioVolumeToSave[i].volume);

            }
            SLHandler.Save(audioVolumeToSave,"AudioVolume");
            changeHappened = false;
        }
    }

}