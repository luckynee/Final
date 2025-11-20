using PokerSeed.General;
using PokerSeed.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerSeed.Gameplay
{
    public class ToolView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image backgroundActiveImg;
        [SerializeField] private Image backgroundInactiveImg;
        [SerializeField] private Image toolImg;
        [SerializeField] private InfoTooltip infoTooltip;

        private Canvas rootCanvas;
        private RectTransform rectTransform;
        private Transform initParent;
        private Vector2 initPosition;
        private int initSiblingIndex;
        private bool isDragable;
        private bool isDragging;

        private ToolData toolData;
        private Action onBeginDragCard;
        private Action onEndDragCard;

        #region UNITY
        public void OnBeginDrag(PointerEventData _eventData)
        {
            if (!isDragable)
            {
                return;
            }
            infoTooltip.ForceClose();
            isDragging = true;
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
            isDragging = false;
            infoTooltip.ForceClose();
            onEndDragCard?.Invoke();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDragging)
            {
                return;
            }
            infoTooltip.ForceOpen();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isDragging)
            {
                return;
            }
            infoTooltip.ForceClose();
        }
        #endregion

        #region GETTER
        public ToolData GetToolData()
        {
            return toolData;
        }
        public bool GetIsDragable()
        {
            return isDragable;
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
            ToolData _toolData, 
            Canvas _rootCanvas, 
            Action _onBeginDragCard, 
            Action _onEndDragCard)
        {
            toolData = _toolData;
            rootCanvas = _rootCanvas;

            rectTransform = GetComponent<RectTransform>();
            initPosition = rectTransform.anchoredPosition;
            initParent = transform.parent;
            initSiblingIndex = transform.GetSiblingIndex();

            onBeginDragCard = _onBeginDragCard;
            onEndDragCard = _onEndDragCard;

            backgroundActiveImg.gameObject.SetActive(true);
            backgroundInactiveImg.gameObject.SetActive(false);
            toolImg.sprite = toolData.ActiveAsset;

            infoTooltip.DisplayTooltipText(toolData.Description);

            isDragable = true;
        }
        #endregion

        #region PROCESS
        public void UseTool()
        {
            VFXManager.Instance.PlayVFX(Enums.VFXTrigger.SMOKE_EFFECT, gameObject.transform.position);
            backgroundActiveImg.gameObject.SetActive(false);
            backgroundInactiveImg.gameObject.SetActive(true);
            isDragable = false;
            toolImg.sprite = toolData.InactiveAsset;
            ResetPosition();
        }
        public void ResetPosition()
        {
            rectTransform.anchoredPosition = initPosition;
            transform.SetParent(initParent);
            transform.SetSiblingIndex(initSiblingIndex);
            infoTooltip.ForceClose();
        }
        #endregion
    }
}
