using System;
using EventBus;
using PokerSeed.General;
using UnityEngine;
using UnityEngine.UI;

public class NewCreditPopUp : MonoBehaviour
{
    [SerializeField] private AnimationHandler animHandler;
    [SerializeField] private Button closeBtn;
    
    private EventBindings<MainMenuInit> _onMainMenuInit;
    private EventBindings<CreditBtnAddListenerEvent> _creditBtnEvent;

    private void Awake()
    {
        _onMainMenuInit = new EventBindings<MainMenuInit>(Init);
        _creditBtnEvent = new EventBindings<CreditBtnAddListenerEvent>(OnOpen);
            
        Bus<MainMenuInit>.Register(_onMainMenuInit);
        Bus<CreditBtnAddListenerEvent>.Register(_creditBtnEvent);

    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Bus<MainMenuInit>.Unregister(_onMainMenuInit);
        Bus<CreditBtnAddListenerEvent>.Unregister(_creditBtnEvent);
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

    private void OnOpen(CreditBtnAddListenerEvent _openPopupCallback)
    {
        gameObject.SetActive(true);
        animHandler.PlayAnimation(false, delegate
        {
            animHandler.StopAnimation(true);
            _openPopupCallback.delegates.Invoke();
        });
    }

    private void OnClose(Action _closePopupCallback)
    {
        animHandler.PlayAnimation(true, delegate
        {
            animHandler.StopAnimation();
            _closePopupCallback?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
