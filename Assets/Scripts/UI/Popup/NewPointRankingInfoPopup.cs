using System;
using EventBus;
using PokerSeed.General;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class NewPointRankingInfoPopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Button closeBtn;

        [SerializeField] private TutorialManager tutorialManager;

        private EventBindings<MainMenuInit> _onMainMenuInit;
        private EventBindings<PointInfoBtnAddListenerEvent> _pointInfoBtnEvent;

        #region DEFAULT

        private void Awake()
        {
            _onMainMenuInit = new EventBindings<MainMenuInit>(Init);
            _pointInfoBtnEvent = new EventBindings<PointInfoBtnAddListenerEvent>(OnOpen);
            
            Bus<MainMenuInit>.Register(_onMainMenuInit);
            Bus<PointInfoBtnAddListenerEvent>.Register(_pointInfoBtnEvent);
        }

        private void Start()
        {
            if(tutorialManager.IsOnTutorial)
            {
                gameObject.SetActive(false);
            };
        }

        private void OnDestroy()
        {
            Bus<MainMenuInit>.Unregister(_onMainMenuInit);
            Bus<PointInfoBtnAddListenerEvent>.Unregister(_pointInfoBtnEvent);
        }

        private void Init()
        {
            closeBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
                OnClose(null);
            });
        }

        private void OnOpen(PointInfoBtnAddListenerEvent _openPopupCallback)
        {
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback.delegates?.Invoke();
            });
        }
        private void OnClose(Action _closePopupCallback)
        {
            animHandler.PlayAnimation(true, delegate
            {
                _closePopupCallback?.Invoke();
                gameObject.SetActive(false);
                animHandler.StopAnimation();
            });
        }
        #endregion
    }
}
