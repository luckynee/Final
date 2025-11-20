using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using PokerSeed.General;
using System;

namespace PokerSeed.Gameplay
{
    public class CardView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI nameTxt;
        [SerializeField] private GameObject pairOutlineContainer;
        [SerializeField] private Image firstPairOutlineImg;
        [SerializeField] private Image secondPairOutlineImg;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image cardImg;
        [SerializeField] private Image closedImg;

        private Canvas rootCanvas;
        private RectTransform rectTransform;
        private Transform initParent;
        private Vector2 initPosition;
        private int initSiblingIndex;
        private bool isDragable = true;

        private CardData cardData;
        private Action onBeginDragCard;
        private Action onEndDragCard;

        #region UNITY
        public void OnBeginDrag(PointerEventData _eventData)
        {
            if (!isDragable)
            {
                return;
            }
            onBeginDragCard?.Invoke();
        }
        public void OnDrag(PointerEventData _eventData)
        {
            if (!isDragable)
            {
                return;
            }
            rectTransform.anchoredPosition += _eventData.delta / rootCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData _eventData)
        {
            if (!isDragable)
            {
                return;
            }
            if (!Utils.Overlaps(rectTransform, rootCanvas.GetComponent<RectTransform>()))
            {
                ResetPosition();
            }
            onEndDragCard?.Invoke();
        }
        #endregion

        #region GETTER
        public CardData GetCardData()
        {
            return cardData;
        }
        public bool GetIsDragable()
        {
            return isDragable;
        }
        public bool IsCardActive()
        {
            return !closedImg.gameObject.activeSelf;
        }
        #endregion

        #region SETTER
        public void SetIsDragable(bool _value)
        {
            isDragable = _value;
        }
        #endregion

        #region INIT
        public void Init(
            CardData _cardData, 
            Canvas _rootCanvas, 
            Action _onBeginDragCard, 
            Action _onEndDragCard)
        {
            cardData = _cardData;
            rootCanvas = _rootCanvas;

            rectTransform = GetComponent<RectTransform>();
            initPosition = rectTransform.anchoredPosition;
            initParent = transform.parent;
            initSiblingIndex = transform.GetSiblingIndex();

            onBeginDragCard = _onBeginDragCard;
            onEndDragCard = _onEndDragCard;

            nameTxt.text = cardData.Name;
            backgroundImg.sprite = cardData.Background;
            cardImg.sprite = cardData.Asset;

            isDragable = false;
            pairOutlineContainer.SetActive(false);
            firstPairOutlineImg.gameObject.SetActive(false);
            secondPairOutlineImg.gameObject.SetActive(false);
            closedImg.gameObject.SetActive(true);
        }
        #endregion

        #region PROCESS
        public void DisplayCard(bool _isDragable)
        {
            if (cardData.Type != Enums.CardType.PEST)
            {
                isDragable = _isDragable;
            }
            closedImg.gameObject.SetActive(false);
        }
        public void DisplayPairCard(bool _isFirstPair = true)
        {
            if (_isFirstPair)
            {
                firstPairOutlineImg.gameObject.SetActive(true);
            }
            else
            {
                secondPairOutlineImg.gameObject.SetActive(true);
            }
        }
        public void DisplayPairIndicator()
        {
            pairOutlineContainer.SetActive(true);
        }
        public void ChangeCard(CardData _cardData, bool _isDragable = true)
        {
            cardData = _cardData;
            isDragable = _isDragable;

            nameTxt.text = cardData.Name;
            backgroundImg.sprite = cardData.Background;
            cardImg.sprite = cardData.Asset;
        }
        public void UpdatePosition()
        {
            initPosition = rectTransform.anchoredPosition;
            initParent = transform.parent;
            initSiblingIndex = transform.GetSiblingIndex();
        }
        public void ResetPosition()
        {
            rectTransform.anchoredPosition = initPosition;
            transform.SetParent(initParent);
            transform.SetSiblingIndex(initSiblingIndex);
        }
        public void PlaySmokeEffect(bool _isSpawn = false)
        {
            StartCoroutine(SmokeEffectRoutine(_isSpawn));
        }
        private IEnumerator SmokeEffectRoutine(bool _isSpawn = false)
        {
            yield return new WaitForSeconds(0.1f);
            if (_isSpawn)
            {
                VFXManager.Instance.SpawnVFX(Enums.VFXTrigger.SMOKE_EFFECT, transform.position);
            }
            else
            {
                VFXManager.Instance.PlayVFX(Enums.VFXTrigger.SMOKE_EFFECT, transform.position);
            }
        }
        #endregion
    }
}
