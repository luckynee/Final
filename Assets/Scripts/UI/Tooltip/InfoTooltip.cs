using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerSeed.General;
using System;
using DG.Tweening;

namespace PokerSeed.UI
{
    public class InfoTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tooltipTxt;
        [SerializeField] private AnimationHandler animHandler;

        public void DisplayTooltipText(string _text)
        {
            tooltipTxt.text = _text;
        }
        public void OnOpen(Action _openTooltipCallback)
        {
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openTooltipCallback?.Invoke();
            });
        }
        public void OnClose(Action _closeTooltipCallback, float _delay = 0)
        {
            animHandler.PlayAnimation(true, delegate
            {
                animHandler.StopAnimation();
                _closeTooltipCallback?.Invoke();
                gameObject.SetActive(false);
            }, _delay);
        }
        public void ForceOpen()
        {
            animHandler.StopAnimation();
            animHandler.transform.localScale = Vector2.one;
            gameObject.SetActive(true);
        }
        public void ForceClose()
        {
            DOTween.Kill(gameObject);
            gameObject.SetActive(false);
        }
    }
}
