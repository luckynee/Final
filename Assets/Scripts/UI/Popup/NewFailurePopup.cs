using System;
using EventBus;
using PokerSeed.General;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Popup
{
    public class NewFailurePopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Button restartBtn;
        
        private EventBindings<MainMenuInit> _onMainMenuInit;
        private EventBindings<FailurePopUpAddEventListenerEvent> _onFailurePopUp;

        #region DEFAULT

        private void Awake()
        {
            _onMainMenuInit = new EventBindings<MainMenuInit>(Init);
            _onFailurePopUp = new EventBindings<FailurePopUpAddEventListenerEvent>(OnOpen);
            
            Bus<MainMenuInit>.Register(_onMainMenuInit);
            Bus<FailurePopUpAddEventListenerEvent>.Register(_onFailurePopUp);
        }
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Bus<MainMenuInit>.Unregister(_onMainMenuInit);
            Bus<FailurePopUpAddEventListenerEvent>.Unregister(_onFailurePopUp);
        }


        public void Init()
        {
            restartBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                OnRestart(delegate
                {
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null,
                        delegate
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        });
                });
            });
        }
        
        public void OnOpen(FailurePopUpAddEventListenerEvent _openPopupCallback)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.FAIL);
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback.delegates?.Invoke();
            });
        }
        private void OnRestart(Action _changeSceneCallback)
        {
            animHandler.PlayAnimation(true, delegate
            {
                animHandler.StopAnimation();    
                _changeSceneCallback?.Invoke();
                gameObject.SetActive(false);
            });
        }
        #endregion
    }
}
