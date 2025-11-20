using PokerSeed.General;
using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _PAProjcet
{
    public class NewSettingsPopUp : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button quitBtn;
        
        private EventBindings<MainMenuInit> _onMainMenuInit;
        private EventBindings<SettingBtnAddListenerEvent> _creditBtnEvent;

        private void Awake()
        {
            _onMainMenuInit ??= new EventBindings<MainMenuInit>(Init);
            _creditBtnEvent ??= new EventBindings<SettingBtnAddListenerEvent>(OnOpen);
            
            Bus<MainMenuInit>.Register(_onMainMenuInit);
            Bus<SettingBtnAddListenerEvent>.Register(_creditBtnEvent);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Bus<MainMenuInit>.Unregister(_onMainMenuInit);
            Bus<SettingBtnAddListenerEvent>.Unregister(_creditBtnEvent);
        }

        public void Init()
        {
            bgmVolumeSlider.onValueChanged.AddListener(delegate
            {
                //AudioManager.Instance.ChangeVolumeBGM(bgmVolumeSlider.value);
                Bus<OnBgmVolumeChange>.Raise(new OnBgmVolumeChange(bgmVolumeSlider.value));
            });
            sfxVolumeSlider.onValueChanged.AddListener(delegate
            {
                //AudioManager.Instance.ChangeVolumeSFX(sfxVolumeSlider.value);
                Bus<OnSfxVolumeChange>.Raise(new OnSfxVolumeChange(sfxVolumeSlider.value));
            });
            closeBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                OnClose(null);
            });
            quitBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                OnClose(delegate
                {
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                    {
                        if (SceneManager.GetActiveScene().name == Names.MainMenuSceneName)
                        {
                            Application.Quit();
                        }
                        else
                        {
                            SceneManager.LoadScene(Names.MainMenuSceneName);
                        }
                    });
                });
            });
            
        }
        public void OnOpen(SettingBtnAddListenerEvent _openPopupCallback)
        {
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback.delegates.Invoke();
            });
        }
        public void OnClose(Action _closePopupCallback)
        {
            animHandler.PlayAnimation(true, delegate
            {
                animHandler.StopAnimation();
                _closePopupCallback?.Invoke();
                gameObject.SetActive(false);
            });
        }
    }
}
