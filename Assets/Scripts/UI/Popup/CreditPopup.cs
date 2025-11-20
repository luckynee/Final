using PokerSeed.General;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class CreditPopup : MonoBehaviour
    {
        
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Button closeBtn;

        public void Init()
        {
            closeBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                OnClose(null);
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
}
