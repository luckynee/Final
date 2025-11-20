using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kopsis.General
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D clickableCursorTexture;
        private int UILayer;
        private bool isDefaultCursor;
        public static CursorManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            UILayer = LayerMask.NameToLayer("UI");
        }
        private void Update()
        {
            if (IsPointerOverUIElement())
            {
                if (isDefaultCursor)
                {
                    isDefaultCursor = false;
                    Cursor.SetCursor(clickableCursorTexture, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                if (!isDefaultCursor)
                {
                    isDefaultCursor = true;
                    Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
                }
            }
        }
        public bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }
        //Returns 'true' if we touched or hovering on Unity UI element.
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaycastResult = eventSystemRaysastResults[index];
                if (curRaycastResult.gameObject.layer == UILayer &&
                    curRaycastResult.gameObject.GetComponent<Button>() &&
                    curRaycastResult.gameObject.GetComponent<Button>().enabled &&
                    curRaycastResult.gameObject.GetComponent<Button>().interactable)
                {
                    return true;
                }
            }
            return false;
        }
        //Gets all event system raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
