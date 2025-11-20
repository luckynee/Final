using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.Audio;

namespace _PAProjcet
{
    public class NewAudioManager : MonoBehaviour
    {
        [SerializeField] private List<BGMData> bgmDataList;
        [SerializeField] private List<AmbienceData> ambienceDataList;
        [SerializeField] private List<SFXData> sfxDataList;
        [SerializeField] private float volumeFadeDuration;
        [SerializeField] private float delayBetweenCrossFade;
        [SerializeField] private AudioMixerGroup bgmAudioMixer;
        [SerializeField] private AudioMixerGroup sfxAudioMixer;

        private bool isMute;
        private float curBgmVolume;
        private float curSfxVolume;
        private Enums.BGMType curBGMPlaying;
        private Enums.AmbienceType curAmbiencePlaying;

        private List<AudioSource> bgmSourceList = new List<AudioSource>();
        private List<AudioSource> ambienceSourceList = new List<AudioSource>();
        private List<AudioSource> sfxSourceList = new List<AudioSource>();

        private GameObject bgmContainer;
        private GameObject ambienceContainer;
        private GameObject sfxContainer;
        
        //Event
        private EventBindings<OnSfxTrigger> _onSfxTrigger;
        private EventBindings<OnAmbienceTrigger> _onAmbienceTrigger;
        private EventBindings<OnSfxVolumeChange> _onSfxVolumeChange;
        private EventBindings<OnBgmVolumeChange> _onBgmVolumeChange;
        private EventBindings<OnAmbienceStop> _onAmbienceStop;
        private EventBindings<OnBgmStop> _onBgmStop;

        public static NewAudioManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            Init();
            LoadData();
            
            _onSfxTrigger = new EventBindings<OnSfxTrigger>(OnSfxTrigger);
            _onAmbienceTrigger = new EventBindings<OnAmbienceTrigger>(OnAmbienceTrigger);
            _onSfxVolumeChange = new EventBindings<OnSfxVolumeChange>(OnSfxVolumeChange);
            _onBgmVolumeChange = new EventBindings<OnBgmVolumeChange>(OnBgmVolumeChange);
            _onAmbienceStop = new EventBindings<OnAmbienceStop>(OnAmbienceStop);
            
            Bus<OnSfxTrigger>.Register(_onSfxTrigger);
            Bus<OnAmbienceTrigger>.Register(_onAmbienceTrigger);
            Bus<OnSfxVolumeChange>.Register(_onSfxVolumeChange);
            Bus<OnBgmVolumeChange>.Register(_onBgmVolumeChange);
            Bus<OnAmbienceStop>.Register(_onAmbienceStop);
            

            PlayBGM(Enums.BGMType.DEFAULT);
        }

        private void OnDisable()
        {
            Bus<OnSfxTrigger>.Unregister(_onSfxTrigger);
            Bus<OnAmbienceTrigger>.Unregister(_onAmbienceTrigger);
            Bus<OnSfxVolumeChange>.Unregister(_onSfxVolumeChange);
            Bus<OnBgmVolumeChange>.Unregister(_onBgmVolumeChange);
            Bus<OnAmbienceStop>.Unregister(_onAmbienceStop);
        }

        //Initialize the BGM and SFX to be used on the game
        private void Init()
        {
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                if (bgmContainer == null)
                {
                    bgmContainer = new GameObject();
                    bgmContainer.name = "BGM";
                    bgmContainer.transform.SetParent(transform);
                }
                GameObject audioSource = new GameObject();
                audioSource.transform.SetParent(bgmContainer.transform);
                audioSource.AddComponent<AudioSource>();
                audioSource.name = bgmDataList[i].BGMScene.ToString();
                audioSource.GetComponent<AudioSource>().clip = bgmDataList[i].AudioClip;
                audioSource.GetComponent<AudioSource>().volume = bgmDataList[i].Volume;
                audioSource.GetComponent<AudioSource>().playOnAwake = false;
                audioSource.GetComponent<AudioSource>().loop = true;
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = bgmAudioMixer;
                bgmSourceList.Add(audioSource.GetComponent<AudioSource>());
            }
            for (int i = 0; i < ambienceDataList.Count; i++)
            {
                if (ambienceContainer == null)
                {
                    ambienceContainer = new GameObject();
                    ambienceContainer.name = "Ambience";
                    ambienceContainer.transform.SetParent(transform);
                }
                GameObject audioSource = new GameObject();
                audioSource.transform.SetParent(ambienceContainer.transform);
                audioSource.AddComponent<AudioSource>();
                audioSource.name = ambienceDataList[i].AmbienceScene.ToString();
                audioSource.GetComponent<AudioSource>().clip = ambienceDataList[i].AudioClip;
                audioSource.GetComponent<AudioSource>().volume = ambienceDataList[i].Volume;
                audioSource.GetComponent<AudioSource>().playOnAwake = false;
                audioSource.GetComponent<AudioSource>().loop = true;
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = sfxAudioMixer;
                ambienceSourceList.Add(audioSource.GetComponent<AudioSource>());
            }
            for (int i = 0; i < sfxDataList.Count; i++)
            {
                if (sfxContainer == null)
                {
                    sfxContainer = new GameObject();
                    sfxContainer.name = "SFX";
                    sfxContainer.transform.SetParent(transform);
                }
                GameObject audioSource = new GameObject();
                audioSource.transform.SetParent(sfxContainer.transform);
                audioSource.AddComponent<AudioSource>();
                audioSource.name = sfxDataList[i].SFXType.ToString();
                audioSource.GetComponent<AudioSource>().clip = sfxDataList[i].AudioClip;
                audioSource.GetComponent<AudioSource>().volume = sfxDataList[i].Volume;
                audioSource.GetComponent<AudioSource>().playOnAwake = false;
                audioSource.GetComponent<AudioSource>().loop = false;
                audioSource.GetComponent<AudioSource>().outputAudioMixerGroup = sfxAudioMixer;
                sfxSourceList.Add(audioSource.GetComponent<AudioSource>());
            }
        }
        #region BGM
        //Play standard BGM based on game Scene, use the crossfade one if want smooth transition otherwise
        //can use this simple version
        private void PlayBGM(Enums.BGMType bgmScene, bool stop = true)
        {
            if (curBGMPlaying == bgmScene)
            {
                return;
            }
            if (stop)
            {
                StopBGM();
            }
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                if (bgmDataList[i].BGMScene == bgmScene)
                {
                    curBGMPlaying = bgmScene;
                    bgmSourceList[i].volume = bgmDataList[i].Volume;
                    bgmSourceList[i].Play();
                    break;
                }
            }
        }
        public void StopBGM()
        {
            for (int i = 0; i < bgmSourceList.Count; i++)
            {
                bgmSourceList[i].Stop();
            }
        }
        #endregion

        #region CROSSFADE_BGM
        //Play crossfade BGM, the previous BGM volume slowly minus and the new BGM volume slowly increase.
        //For smooth BGM transition between 2 scenes
        public void PlayCrossFadeBGM(Enums.BGMType bgmScene)
        {
            StartCoroutine(CrossFadeAudioRoutine(bgmScene));
        }
        private IEnumerator CrossFadeAudioRoutine(Enums.BGMType bgmScene)
        {
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                if (bgmDataList[i].BGMScene == curBGMPlaying)
                {
                    StartCoroutine(FadeAudioRoutine(bgmSourceList[i], 0, true));
                    break;
                }
            }
            yield return new WaitForSeconds(delayBetweenCrossFade);
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                if (bgmDataList[i].BGMScene == bgmScene)
                {
                    curBGMPlaying = bgmScene;
                    bgmSourceList[i].volume = 0;
                    bgmSourceList[i].Play();
                    StartCoroutine(FadeAudioRoutine(bgmSourceList[i], bgmDataList[i].Volume, false));
                    break;
                }
            }
        }
        private IEnumerator FadeAudioRoutine(AudioSource _audioSource, float _targetVolume, bool _isStop)
        {
            float currentTime = 0;
            float start = _audioSource.volume;
            while (currentTime < volumeFadeDuration)
            {
                currentTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(start, _targetVolume, currentTime / volumeFadeDuration);
                yield return null;
            }
            if (_isStop)
            {
                _audioSource.Stop();
            }
            yield break;
        }
        #endregion

        #region AMBIENCE

        private void PlayAmbience(Enums.AmbienceType ambienceScene, bool stop = true)
        {
            if (curAmbiencePlaying == ambienceScene)
            {
                return;
            }
            if (stop)
            {
                StopAmbience();
            }
            for (int i = 0; i < ambienceDataList.Count; i++)
            {
                if (ambienceDataList[i].AmbienceScene == ambienceScene)
                {
                    ambienceSourceList[i].volume = ambienceDataList[i].Volume;
                    ambienceSourceList[i].Play();
                    break;
                }
            }
        }

        private void StopAmbience()
        {
            for (int i = 0; i < ambienceSourceList.Count; i++)
            {
                ambienceSourceList[i].Stop();
            }
        }
        #endregion

        #region SFX
        //Play SFX by type, just called it anywhere on the script
        private void PlaySFX(Enums.SFXType sfxType)
        {
            for (int i = 0; i < sfxDataList.Count; i++)
            {
                if (sfxDataList[i].SFXType == sfxType)
                {
                    sfxSourceList[i].Play();
                    break;
                }
            }
        }
        #endregion
        
        private void OnSfxTrigger(OnSfxTrigger _event)
        {
            PlaySFX(_event.SfxType);
        }
        
        private void OnAmbienceTrigger(OnAmbienceTrigger _event)
        {
            PlayAmbience(_event.AmbienceType);
        }
        
        private void OnSfxVolumeChange(OnSfxVolumeChange _event)
        {
            ChangeVolumeSFX(_event.Volume);
        }
        
        private void OnBgmVolumeChange(OnBgmVolumeChange _event)
        {
            ChangeVolumeBGM(_event.Volume);
        }
        
        private void OnAmbienceStop(OnAmbienceStop _event)
        {
            StopAmbience();
        }
        

        #region AUDIO_SETTING
        //Mute all audio for BGM and SFX
        public void MuteAudio()
        {
            isMute = true;
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                bgmSourceList[i].mute = true;
            }
            for (int i = 0; i < sfxDataList.Count; i++)
            {
                sfxSourceList[i].mute = true;
            }
            SaveData();
        }

        //Unmute all audio for BGM and SFX
        public void UnmuteAudios()
        {
            isMute = false;
            for (int i = 0; i < bgmDataList.Count; i++)
            {
                bgmSourceList[i].mute = false;
            }
            for (int i = 0; i < sfxDataList.Count; i++)
            {
                sfxSourceList[i].mute = false;
            }
            SaveData();
        }

        private void ChangeVolumeBGM(float value)
        {
            bgmAudioMixer.audioMixer.SetFloat("Volume", value);
            curBgmVolume = value;
        }

        private void ChangeVolumeSFX(float value)
        {
            sfxAudioMixer.audioMixer.SetFloat("Volume", value);
            curSfxVolume = value;
        }
       
        #endregion

        #region SAVELOAD
        //Use this for setting for mute audio (SFX/BGM)
        public void SaveData()
        {
            if (isMute)
            {
                PlayerPrefs.SetInt("MuteAudio", 1);
            }
            else
            {
                PlayerPrefs.SetInt("MuteAudio", 0);
            }
        }
        public void LoadData()
        {
            if (PlayerPrefs.GetInt("MuteAudio", 0) == 1)
            {
                isMute = true;
            }
            else
            {
                isMute = false;
            }
        }
        #endregion

        [System.Serializable]
        public struct BGMData
        {
            public Enums.BGMType BGMScene;
            public AudioClip AudioClip;
            public float Volume;
        }
        [System.Serializable]
        public struct AmbienceData
        {
            public Enums.AmbienceType AmbienceScene;
            public AudioClip AudioClip;
            public float Volume;
        }

        [System.Serializable]
        public struct SFXData
        {
            public Enums.SFXType SFXType;
            public AudioClip AudioClip;
            public float Volume;
        }
    }
}