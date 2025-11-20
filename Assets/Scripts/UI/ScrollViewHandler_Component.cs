using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Potluck.General
{
    public class ScrollViewHandler_Component : MonoBehaviour, IBeginDragHandler
    {
        public UnityEvent OnBeginDragAction;
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragAction?.Invoke();
        }
    }
}