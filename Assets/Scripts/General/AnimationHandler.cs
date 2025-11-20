using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

namespace PokerSeed.General
{
    public class AnimationHandler : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private Enums.AnimationType animationType;
        [SerializeField] private bool isLooping;
        [SerializeField] private LoopType loopType;
        [SerializeField] private float animDuration;
        [SerializeField] private Ease ease;
        [SerializeField] private float delay;
        [Header("POSITION-BASED")]
        [Space(5)]
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [Header("SCALE-BASED")]
        [Space(5)]
        [SerializeField] private float scalePercentage;
        [SerializeField] private bool playOnAwake;

        private Vector3 initPosition;

        private void Awake()
        {
            ResetAnimation();
            if (playOnAwake)
            {
                PlayAnimation(false);
            }
        }
        public void PlayAnimation(bool _isReverse = false, Action _callback = null, float _delay = 0)
        {
            gameObject.SetActive(true);
            if (_isReverse)
            {
                AdjustReversePosition();
            }
            if (_delay != 0)
            {
                delay = _delay;
            }
            StartCoroutine(CheckAnimationType(_isReverse, _callback));
        }

        private IEnumerator CheckAnimationType(bool isReverse, Action _callback = null)
        {
            yield return new WaitForSeconds(delay);
            switch (animationType)
            {
                case Enums.AnimationType.MOVE_HORIZONTAL:
                    GetComponent<RectTransform>().DOAnchorPosX(isReverse ? startPosition.x : endPosition.x, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.MOVE_VERTICAL:
                    GetComponent<RectTransform>().DOAnchorPosY(isReverse ? startPosition.y : endPosition.y, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.SCALE:
                    gameObject.transform.DOScale(isReverse ? startPosition : endPosition, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.SCROLL_VERTICAL:
                    GetComponent<ScrollRect>().DOVerticalNormalizedPos(isReverse ? startPosition.y : endPosition.y, animDuration, false).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        SetDelay(delay).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.SCROLL_HORIZONTAL:
                    GetComponent<ScrollRect>().DOHorizontalNormalizedPos(isReverse ? startPosition.x : endPosition.x, animDuration, false).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        SetDelay(delay).
                        OnComplete(delegate
                        {

                        });
                    break;
                case Enums.AnimationType.FADE_IMAGE:
                    GetComponent<Image>().DOFade(isReverse ? startPosition.x : endPosition.x, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.FADE_TEXT:
                    GetComponent<TextMeshProUGUI>().DOFade(isReverse ? startPosition.x : endPosition.x, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.FADE_CANVASGROUP:
                    GetComponent<CanvasGroup>().DOFade(isReverse ? startPosition.x : endPosition.x, animDuration).
                        SetLoops(isLooping ? -1 : 0, loopType).
                        SetEase(ease).
                        OnComplete(delegate
                        {
                            _callback?.Invoke();
                        });
                    break;
                case Enums.AnimationType.ROTATE_CLOCKWISE:
                    transform.DORotate(new Vector3(0, 0, -360), animDuration, RotateMode.FastBeyond360)
                        .SetRelative(true)
                        .SetLoops(isLooping ? -1 : 0, loopType)
                        .SetEase(ease);
                    break;
                case Enums.AnimationType.ROTATE_COUNTERCLOCKWISE:
                    transform.DORotate(new Vector3(0, 0, 360), animDuration, RotateMode.FastBeyond360)
                        .SetRelative(true)
                        .SetLoops(isLooping ? -1 : 0, loopType)
                        .SetEase(ease);
                    break;
            }
        }
        public void StopAnimation(bool _displayObject = false)
        {
            DOTween.Kill(gameObject);
            if (!_displayObject)
            {
                ResetAnimation();
            }
            gameObject.SetActive(_displayObject);
        }

        private void ResetAnimation()
        {
            switch (animationType)
            {
                case Enums.AnimationType.MOVE_HORIZONTAL:
                case Enums.AnimationType.MOVE_VERTICAL:
                    GetComponent<RectTransform>().anchoredPosition = startPosition;
                    break;
                case Enums.AnimationType.SCALE:
                    transform.localScale = startPosition;
                    break;
                case Enums.AnimationType.SCROLL_VERTICAL:
                    GetComponent<ScrollRect>().verticalNormalizedPosition = startPosition.y;
                    break;
                case Enums.AnimationType.FADE_IMAGE:
                    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, startPosition.y);
                    break;
                case Enums.AnimationType.FADE_TEXT:
                    GetComponent<TextMeshProUGUI>().color = new Color(GetComponent<TextMeshProUGUI>().color.r, GetComponent<TextMeshProUGUI>().color.g, GetComponent<TextMeshProUGUI>().color.b, startPosition.y);
                    break;
                case Enums.AnimationType.ROTATE_CLOCKWISE:
                case Enums.AnimationType.ROTATE_COUNTERCLOCKWISE:
                    transform.localEulerAngles = Vector3.zero;
                    break;
            }
        }
        private void AdjustReversePosition()
        {
            switch (animationType)
            {
                case Enums.AnimationType.MOVE_HORIZONTAL:
                case Enums.AnimationType.MOVE_VERTICAL:
                    GetComponent<RectTransform>().anchoredPosition = endPosition;
                    break;
                case Enums.AnimationType.SCALE:
                    transform.localScale = endPosition;
                    break;
            }
        }
    }
}
