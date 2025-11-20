using PokerSeed.General;
using System;
using EventBus;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class PointRankingInfoPopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Button closeBtn;

        #region DEFAULT
        public void Init()
        {
            closeBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                
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
        public void OnClose(Action _closePopupCallback)
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
