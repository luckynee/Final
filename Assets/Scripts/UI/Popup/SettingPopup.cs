using PokerSeed.General;
using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class SettingPopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button quitBtn;
        
        public void Init()
        {
            bgmVolumeSlider.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.ChangeVolumeBGM(bgmVolumeSlider.value);
            });
            sfxVolumeSlider.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.ChangeVolumeSFX(sfxVolumeSlider.value);
            });
            closeBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                OnClose(null);
            });
            quitBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
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
        public void OnOpen(Action _openPopupCallback)
        {
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback?.Invoke();
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
