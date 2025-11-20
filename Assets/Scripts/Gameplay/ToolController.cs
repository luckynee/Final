using PokerSeed.General;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace PokerSeed.Gameplay
{
    public class ToolController : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private List<ToolData> toolDataList;
        [SerializeField] private ToolView toolViewPrefab;
        [SerializeField] private RectTransform toolViewContainer;
        [SerializeField] private CardController cardController;

        private List<ToolView> toolViewList = new List<ToolView>();

        #region UNITY
        private void Awake()
        {
            cardController.RegisterAction(
                delegate
                {
                    UpdateDragableTools(true);
                },
                delegate
                {
                    UpdateDragableTools(false);
                });
        }
        private void Start()
        {
            DisplayTools();
        }
        #endregion

        private void DisplayTools()
        {
            for(int i = 0; i < toolDataList.Count; i++)
            {
                ToolView toolViewSpawn = Instantiate(toolViewPrefab, toolViewContainer);
                toolViewSpawn.Init(toolDataList[i], rootCanvas,
                    delegate
                    {
                        BeginDragToolAction(toolViewSpawn);
                    },
                    delegate
                    {
                        EndDragToolAction(toolViewSpawn);
                    });
                toolViewList.Add(toolViewSpawn);
            }
        }
        private void UpdateDragableTools(bool _value)
        {
            for (int i = 0; i < toolViewList.Count; i++)
            {
                toolViewList[i].SetIsDragable(_value);
            }
        }
        private void BeginDragToolAction(ToolView _draggedTool)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_PICK);
            _draggedTool.transform.SetParent(rootCanvas.transform);
            if (TutorialManager.Instance.IsOnTutorial)
            {
                TutorialManager.Instance.GetActiveTutorial().transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        private void EndDragToolAction(ToolView _draggedTool)
        {
            CardView overlappedTopCardView = CheckOverlapWithCard(_draggedTool, cardController.TopCardContainer);
            CardView overlappedMiddleCardView = CheckOverlapWithCard(_draggedTool, cardController.MiddleCardContainer);
            CardView overlappedBottomCardView = CheckOverlapWithCard(_draggedTool, cardController.BottomCardContainer);

            bool hasOverlap = false;

            CheckToolUsage(_draggedTool, overlappedTopCardView, hasOverlap, Enums.GameState.SECOND_ROUND);
            CheckToolUsage(_draggedTool, overlappedMiddleCardView, hasOverlap, Enums.GameState.THIRD_ROUND);
            CheckToolUsage(_draggedTool, overlappedBottomCardView, hasOverlap, Enums.GameState.FIRST_ROUND);
            if (!hasOverlap)
            {
                _draggedTool.ResetPosition();
                if (TutorialManager.Instance.IsOnTutorial)
                {
                    TutorialManager.Instance.GetActiveTutorial().transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        private void CheckToolUsage(ToolView _draggedTool, CardView _overlappedCardView, bool _hasOverlap, Enums.GameState _gameState)
        {
            if (_hasOverlap)
            {
                return;
            }
            if (_overlappedCardView != null && _overlappedCardView.IsCardActive())
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_DROP);
                if (_draggedTool.GetToolData().EffectCard.Type == Enums.CardType.PEST)
                {
                    if (_overlappedCardView.GetCardData().Type == Enums.CardType.PEST)
                    {
                        int randomCard = Random.Range(0, cardController.CardDataList.Count - 1);
                        _overlappedCardView.ChangeCard(cardController.CardDataList[randomCard], (cardController.CurGameState == _gameState) ? true : false);
                        _draggedTool.UseTool();
                        if (TutorialManager.Instance.IsOnTutorial)
                        {
                            TutorialManager.Instance.NextTutorial();
                        }
                    }
                    else
                    {
                        _draggedTool.ResetPosition();
                    }
                }
                else
                {
                    if (_overlappedCardView.GetCardData().Type != Enums.CardType.PEST &&
                        _overlappedCardView.GetCardData().Type != _draggedTool.GetToolData().EffectCard.Type)
                    {
                        _overlappedCardView.ChangeCard(_draggedTool.GetToolData().EffectCard, (cardController.CurGameState == _gameState) ? true : false);
                        _draggedTool.UseTool();
                    }
                    else
                    {
                        _draggedTool.ResetPosition();
                    }
                }
                _hasOverlap = true;
            }
        }
        private CardView CheckOverlapWithCard(ToolView _draggedTool, RectTransform _cardContainer)
        {
            CardView overlappedCard = null;
            for (int i = 0; i < _cardContainer.childCount; i++)
            {
                if (Utils.Overlaps(_draggedTool.GetComponent<RectTransform>(), _cardContainer.GetChild(i).GetComponent<RectTransform>()))
                {
                    overlappedCard = _cardContainer.GetChild(i).GetComponent<CardView>();
                }
            }
            return overlappedCard;
        }
    }
}
