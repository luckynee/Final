    using System;
    using PokerSeed.General;
using PokerSeed.Player;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class TutorialPopup : MonoBehaviour
    {
        private EventBindings<MainMenuInit> _onMainMenuInit;
        
        [SerializeField] private Button closeBtn;

        private bool isTutorialBeforeGameplay;

        private void Awake()
        {
            _onMainMenuInit = new EventBindings<MainMenuInit>(Init);
            
            Bus<MainMenuInit>.Register(_onMainMenuInit);
        }

        private void OnDisable()
        {
            Bus<MainMenuInit>.Unregister(_onMainMenuInit);
        }

        public void Init()
        {
            closeBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                OnClose();
            });
        }
        public void OnOpen(bool _isTutorialBeforeGameplay = false)
        {
            isTutorialBeforeGameplay = _isTutorialBeforeGameplay;
            gameObject.SetActive(true);
        }
        public void OnClose()
        {
            if (isTutorialBeforeGameplay)
            {
                if (!PlayerDataManager.Instance.HasShownTutorial)
                {
                    PlayerDataManager.Instance.FinishTutorial();
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null, delegate
                    {
                        SceneManager.LoadScene(Names.GameplaySceneName);
                    });
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
