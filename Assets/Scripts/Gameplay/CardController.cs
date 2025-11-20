using PokerSeed.General;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using PokerSeed.UI;
using PokerSeed.Config;
using UnityEngine.SceneManagement;
using System;
using EventBus;
using PokerSeed.Player;
using UnityEngine.UI;
using PokerSeed.VFX;
using Tutorial;

namespace PokerSeed.Gameplay
{
    public class CardController : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private GameConfigData gameConfigData;
        [SerializeField] private List<CardData> cardDataList;
        [SerializeField] private RectTransform topCardContainer;
        [SerializeField] private RectTransform middleCardContainer;
        [SerializeField] private RectTransform bottomCardContainer;
        [SerializeField] private RectTransform finalCardContainer;
        [SerializeField] private CardView cardViewPrefab;
        [SerializeField] private TextMeshProUGUI topSeedPointTxt;
        [SerializeField] private TextMeshProUGUI middleSeedPointTxt;
        [SerializeField] private TextMeshProUGUI bottomSeedPointTxt;
        [SerializeField] private TextMeshProUGUI gameRoundTxt;
        [SerializeField] private CurrencyRewardVFX greenSeedVFX;
        [SerializeField] private CurrencyRewardVFX yellowSeedVFX;
        [SerializeField] private CurrencyRewardVFX redSeedVFX;
        [SerializeField] private Button informationBtn;
        [SerializeField] private Button settingBtn;       

        public List<CardData> CardDataList => cardDataList;
        public RectTransform TopCardContainer => topCardContainer;
        public RectTransform MiddleCardContainer => middleCardContainer;
        public RectTransform BottomCardContainer => bottomCardContainer;
    
        [Header("POPUP")]
        [Space(5)]
        [SerializeField] private PointRankingInfoPopup pointRankingInfoPopup;
        [SerializeField] private SuccessPopup successGreenSeedPopup;
        [SerializeField] private SuccessPopup successYellowSeedPopup;
        [SerializeField] private SuccessPopup successRedSeedPopup;
        [SerializeField] private SuccessPopup successGoldenSeedPopup;
        [SerializeField] private FailurePopup failureSeedPopup;
        [SerializeField] private SettingPopup settingPopup;

        [Header("TOOLTIP")]
        [Space(5)]
        [SerializeField] private InfoTooltip gainGreenSeedPointTooltip;
        [SerializeField] private InfoTooltip gainYellowSeedPointTooltip;
        [SerializeField] private InfoTooltip gainRedSeedPointTooltip;

        public event Action onTutorialCountPoint; //Tutorial
        
        private Action onRoundStartAction;
        private Action onRoundEndAction;
        private CardView finalCardView;
        private Enums.GameState curGameState;
        public Enums.GameState CurGameState => curGameState;
        private int curGameRound = 1;
        private int curPestCard;
        private int curTotalMiddleCard;
        private int curTotalGreenSeedPoints;
        private int curTotalYellowSeedPoints;
        private int curTotalRedSeedPoints;
        private int curTotalPopupOpened;
        private bool hasShownGreenSeedPopup;
        private bool hasShownYellowSeedPopup;
        private bool hasShownRedSeedPopup;
        private bool hasShownGoldenSeedPopup;

        private void Awake()
        {
            // pointRankingInfoPopup.Init();
            // settingPopup.Init();
            //
            // informationBtn.onClick.AddListener(delegate
            // {
            //     //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
            //     pointRankingInfoPopup.OnOpen(null);
            // });
            // settingBtn.onClick.AddListener(delegate
            // {
            //     //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
            //     settingPopup.OnOpen(null);
            // });
        }
        private void Start()
        {
            AudioManager.Instance.PlayAmbience(Enums.AmbienceType.GAMEPLAY);

            successGreenSeedPopup.Init(this);
            successYellowSeedPopup.Init(this);
            successRedSeedPopup.Init(this);
            successGoldenSeedPopup.Init(this);
            failureSeedPopup.Init();
            
            var configData = TutorialManager.Instance.IsOnTutorial ? TutorialManager.Instance.GetTutorialConfigData : gameConfigData;
            
            topSeedPointTxt.text = curTotalGreenSeedPoints + "/" + configData.MaxSeedPoint;
            middleSeedPointTxt.text = curTotalYellowSeedPoints + "/" + configData.MaxSeedPoint;
            bottomSeedPointTxt.text = curTotalRedSeedPoints + "/" + configData.MaxSeedPoint;

            SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(true, delegate 
            {
                if (TutorialManager.Instance.IsOnTutorial)
                {
                    StartTutorial();
                    return;
                }
                Debug.Log("Start Normal Game");
                pointRankingInfoPopup.OnOpen(null);
                StartGameRound();
            });
        }

        #region ACTION
        public void RegisterAction(Action _onRoundStartCallback, Action _onRoundEndCallback)
        {
            onRoundStartAction += _onRoundStartCallback;
            onRoundEndAction += _onRoundEndCallback;
        }
        #endregion

        #region TUTORIAL

        private void InvokeEvent()
        {
            onTutorialCountPoint?.Invoke();
            Debug.Log("Invoke");
        }
        private void StartTutorial()
        {
            gameRoundTxt.text = "Round: " + curGameRound + "/" + TutorialManager.Instance.GetTutorialConfigData.MaxGameRound;
            onRoundStartAction?.Invoke();

            curPestCard = TutorialManager.Instance.GetTutorialConfigData.MaxPestCardPerRound;
            curTotalMiddleCard = 0;

            ResetBoard(topCardContainer);
            ResetBoard(middleCardContainer);
            ResetBoard(bottomCardContainer);

            SetTopDeck(topCardContainer);
            SetBottomDeck(bottomCardContainer);
            SetFinalCard(cardDataList[3]);

            ChangeGameState(Enums.GameState.FIRST_ROUND);
        }

        private void SetBottomDeck(RectTransform _cardSlotContainer)
        {
            List<CardData> bottomDeck = new List<CardData>
            {
                cardDataList[0],
                cardDataList[0],
                cardDataList[0],
                cardDataList[0],
                cardDataList[5],
                cardDataList[5],
            };
            
            PutCardInDeck(bottomDeck, _cardSlotContainer);
        }

        private void SetTopDeck(RectTransform _cardSlotContainer)
        {
            List<CardData> topDeck = new List<CardData>
            {
                cardDataList[3],
                cardDataList[3],
                cardDataList[3],
                cardDataList[0],
                cardDataList[4],
                cardDataList[2],
            };
            
            PutCardInDeck(topDeck, _cardSlotContainer);
        }

        private void SetFinalCard(CardData _card)
        {
            // Pastikan finalCardView ada, atau buat jika diperlukan
            if (!finalCardView)
            {
                finalCardView = Instantiate(cardViewPrefab, finalCardContainer);
                finalCardView.transform.localPosition = Vector2.zero;
            }

            // Set data kartu akhir
            finalCardView.Init(
                _card, 
                rootCanvas,
                delegate { BeginDragCardAction(finalCardView); },
                delegate { EndDragCardAction(finalCardView); }
            );
        }

        private void PutCardInDeck(List<CardData> _deckist, RectTransform _cardSlotContainer)
        {
            for (int i = 0; i < _deckist.Count; i++)
            {
                CardView cardViewSpawn = Instantiate(cardViewPrefab, _cardSlotContainer);
                cardViewSpawn.name = _deckist[i].Type.ToString() + "_" + i;
                cardViewSpawn.transform.localPosition = Vector2.zero;
                cardViewSpawn.Init(
                    _deckist[i],
                    rootCanvas,
                    delegate { BeginDragCardAction(cardViewSpawn); },
                    delegate { EndDragCardAction(cardViewSpawn); }
                );
            }
        }

        #endregion

        #region GAMESTATE
        private void StartGameRound()
        {
            gameRoundTxt.text = "Round: " + curGameRound + "/" + gameConfigData.MaxGameRound;
            onRoundStartAction?.Invoke();

            curPestCard = gameConfigData.MaxPestCardPerRound;
            curTotalMiddleCard = 0;

            ResetBoard(topCardContainer);
            ResetBoard(middleCardContainer);
            ResetBoard(bottomCardContainer);

            ShuffleCards(topCardContainer);
            ShuffleCards(bottomCardContainer);
            ShuffleFinalCard();

            ChangeGameState(Enums.GameState.FIRST_ROUND);
        }
        private void FinishGameRound()
        {
            var configData = TutorialManager.Instance.IsOnTutorial ? TutorialManager.Instance.GetTutorialConfigData : gameConfigData; 
            
            if (curTotalGreenSeedPoints >= configData.MaxSeedPoint ||
                curTotalYellowSeedPoints >= configData.MaxSeedPoint ||
                curTotalRedSeedPoints >= configData.MaxSeedPoint)
            {
                if (curTotalGreenSeedPoints >= configData.MaxSeedPoint)
                {
                    curTotalPopupOpened++;
                }
                else
                {
                    hasShownGreenSeedPopup = true;
                }
                if (curTotalYellowSeedPoints >= configData.MaxSeedPoint)
                {
                    curTotalPopupOpened++;
                }
                else
                {
                    hasShownYellowSeedPopup = true;
                }
                if (curTotalRedSeedPoints >= configData.MaxSeedPoint)
                {
                    curTotalPopupOpened++;
                }
                else
                {
                    hasShownRedSeedPopup = true;
                }

                if (curTotalGreenSeedPoints >= configData.MaxSeedPoint &&
                    curTotalYellowSeedPoints >= configData.MaxSeedPoint &&
                    curTotalRedSeedPoints >= configData.MaxSeedPoint)
                {
                    curTotalPopupOpened++;
                }
                else 
                {
                    hasShownGoldenSeedPopup = true;
                }
                
                    
                CheckSuccessPopup();
            }
            else
            {
                failureSeedPopup.OnOpen(null);
            }
        }
        public void CheckSuccessPopup()
        {
            CloseSuccessPopup(successGreenSeedPopup);
            CloseSuccessPopup(successYellowSeedPopup);
            CloseSuccessPopup(successRedSeedPopup);
            CloseSuccessPopup(successGoldenSeedPopup);
    
            if (!hasShownGreenSeedPopup)
            {
                hasShownGreenSeedPopup = true;
                curTotalPopupOpened--;
                successGreenSeedPopup.OnOpen(delegate
                {
                    successGreenSeedPopup.CheckLastPopup(curTotalPopupOpened);
                });
            }
            else if (!hasShownYellowSeedPopup)
            {
                hasShownYellowSeedPopup = true;
                curTotalPopupOpened--;
                successYellowSeedPopup.OnOpen(delegate
                {
                    successYellowSeedPopup.CheckLastPopup(curTotalPopupOpened);
                });
            }
            else if (!hasShownRedSeedPopup)
            {
                hasShownRedSeedPopup = true;
                curTotalPopupOpened--;
                successRedSeedPopup.OnOpen(delegate
                {
                    successRedSeedPopup.CheckLastPopup(curTotalPopupOpened);
                });
            }
            else if (!hasShownGoldenSeedPopup)
            {
                hasShownGoldenSeedPopup = true;
                curTotalPopupOpened--;
                successGoldenSeedPopup.OnOpen(delegate
                {
                    successGoldenSeedPopup.CheckLastPopup(curTotalPopupOpened);
                });
                
            }

            if (!TutorialManager.Instance.IsOnTutorial) return;

            if (curTotalPopupOpened == 0)
            {
                TutorialManager.Instance.ChangeStateTutorial();
            }
        }


        private void CloseSuccessPopup(SuccessPopup _popup)
        {
            if (_popup.gameObject.activeSelf)
            {
                _popup.OnClose(delegate
                {
                    VFXManager.Instance.StopVFX();
                });
            }
        }

        private void CheckGameRound()
        {
            curGameRound++;
            if (curGameRound > gameConfigData.MaxGameRound)
            {
                if (TutorialManager.Instance.IsOnTutorial)
                {
                    TutorialManager.Instance.NextTutorial();
                    TutorialManager.Instance.ChangeToNormalTutorial();
                    PlayerDataManager.Instance.FinishTutorial();
                }
                FinishGameRound();
            }
            else
            {
                SceneTransitionManager.Instance.PlaySceneFullFadeAnimation(
                null,
                delegate
                {
                    if (TutorialManager.Instance.IsOnTutorial && curGameRound == 2)
                    {
                        TutorialManager.Instance.NextTutorial();
                    }
                    StartGameRound();
                }, 
                null);
            }
        }
        
        public void ChangeGameState(Enums.GameState _gameState)
        {
            curGameState = _gameState;
            switch(curGameState)
            {
                case Enums.GameState.FIRST_ROUND:
                    for(int i = 0; i < bottomCardContainer.childCount; i++)
                    {
                        bottomCardContainer.GetChild(i).GetComponent<CardView>().DisplayCard(true);
                    }
                    break;
                case Enums.GameState.SECOND_ROUND:
                    DisableCards(bottomCardContainer);
                    for (int i = 0; i < topCardContainer.childCount; i++)
                    {
                        topCardContainer.GetChild(i).GetComponent<CardView>().DisplayCard(true);
                    }
                    break;
                case Enums.GameState.THIRD_ROUND:
                    DisableCards(topCardContainer);
                    RevealFinalCard();
                    break;
            }
        }
        private void ResetBoard(RectTransform _cardSlotContainer)
        {
            for (int i = _cardSlotContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_cardSlotContainer.GetChild(i).gameObject);
            }
        }
        private void DisableCards(RectTransform _cardSlotContainer)
        {
            for (int i = 0; i < _cardSlotContainer.childCount; i++)
            {
                _cardSlotContainer.GetChild(i).GetComponent<CardView>().SetIsDragable(false);
            }
        }
        private void ShuffleCards(RectTransform _cardSlotContainer)
        {
            List<CardData> shuffleCardDataList = new List<CardData>(cardDataList);

            int pest = 0;
    
            for (int i = 0; i < gameConfigData.MaxCardSlot; i++)
            {
                int randomCard = TutorialManager.Instance.IsOnTutorial && i == gameConfigData.MaxCardSlot -1 && pest == 0 ? 7 : UnityEngine.Random.Range(0, shuffleCardDataList.Count);
                
                CardView cardViewSpawn = Instantiate(cardViewPrefab, _cardSlotContainer);
                cardViewSpawn.name = shuffleCardDataList[randomCard].Type.ToString() + "_" + i;
                cardViewSpawn.transform.localPosition = Vector2.zero;
                cardViewSpawn.Init(
                    shuffleCardDataList[randomCard], 
                    rootCanvas,
                    () => BeginDragCardAction(cardViewSpawn),
                    () => EndDragCardAction(cardViewSpawn)
                );
                if (shuffleCardDataList[randomCard].Type == Enums.CardType.PEST)
                {
                    curPestCard--;
                    pest++;
                    if (curPestCard <= 0)
                    {
                        // Remove additional Pest cards if limit is reached
                        shuffleCardDataList.RemoveAll(x => x.Type == Enums.CardType.PEST);
                    }
                }
            }
        }

        private void ShuffleFinalCard()
        {
            List<CardData> shuffleCardDataList = new List<CardData>();
            for(int i = 0; i < cardDataList.Count; i++)
            {
                shuffleCardDataList.Add(cardDataList[i]);
            }
            shuffleCardDataList.RemoveAt(shuffleCardDataList.FindIndex(x => x.Type == Enums.CardType.PEST));
            int randomCard = UnityEngine.Random.Range(0, shuffleCardDataList.Count);
            
            finalCardView = Instantiate(cardViewPrefab, finalCardContainer);
            finalCardView.transform.localPosition = Vector2.zero;
            finalCardView.Init(
                    shuffleCardDataList[randomCard],
                    rootCanvas,
                    delegate
                    {
                        BeginDragCardAction(finalCardView);
                    },
                    delegate
                    {
                        EndDragCardAction(finalCardView);
                    });
        }
        private void RevealFinalCard()
        {
            onRoundEndAction?.Invoke();
            StartCoroutine(RevealFinalCardRoutine());
        }
        private IEnumerator RevealFinalCardRoutine()
        {
            finalCardView.DisplayCard(false);
            yield return new WaitForSeconds(2);
            SpawnFinalCardOnContainer(finalCardView, topCardContainer);
            SpawnFinalCardOnContainer(finalCardView, middleCardContainer);
            SpawnFinalCardOnContainer(finalCardView, bottomCardContainer);
            yield return new WaitForSeconds(1);
            DisplayGainedSeedPoints();
        }

        private void SpawnFinalCardOnContainer(CardView _finalCardView, RectTransform _cardSlotContainer)
        {
            CardView cardSpawn = Instantiate(cardViewPrefab, _cardSlotContainer);
            cardSpawn.Init(_finalCardView.GetCardData(), rootCanvas, null, null);
            cardSpawn.DisplayCard(true);
            cardSpawn.SetIsDragable(false);
            cardSpawn.transform.SetAsFirstSibling();
            cardSpawn.PlaySmokeEffect(true);
            AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_DROP);
        }
        #endregion

        #region POINTS
        private void DisplayGainedSeedPoints()
        {
            var configData = TutorialManager.Instance.IsOnTutorial ? TutorialManager.Instance.GetTutorialConfigData : gameConfigData; 
            
            int totalGreenSeedPoints = CheckCardRankingPoints(topCardContainer);
            int totalYellowSeedPoints = CheckCardRankingPoints(middleCardContainer);
            int totalRedSeedPoints = CheckCardRankingPoints(bottomCardContainer);
            curTotalGreenSeedPoints += totalGreenSeedPoints;
            curTotalYellowSeedPoints += totalYellowSeedPoints;
            curTotalRedSeedPoints += totalRedSeedPoints;
            
            gainGreenSeedPointTooltip.DisplayTooltipText("Gain Green " + "\n" + "Seed Points: " + totalGreenSeedPoints);
            DisplayPairCardIndicator(topCardContainer);
            gainGreenSeedPointTooltip.OnOpen(delegate
            {
                DisplaySeedPointVFX(greenSeedVFX, gainGreenSeedPointTooltip.transform, topSeedPointTxt.gameObject, totalGreenSeedPoints);
                topSeedPointTxt.text = curTotalGreenSeedPoints + "/" + configData.MaxSeedPoint;

                gainGreenSeedPointTooltip.OnClose(delegate 
                {
                    gainYellowSeedPointTooltip.DisplayTooltipText("Gain Yellow " + "\n" + "Seed Points: " + totalYellowSeedPoints);
                    DisplayPairCardIndicator(middleCardContainer);
                    gainYellowSeedPointTooltip.OnOpen(delegate
                    {
                        DisplaySeedPointVFX(yellowSeedVFX, gainYellowSeedPointTooltip.transform, middleSeedPointTxt.gameObject, totalYellowSeedPoints);
                        middleSeedPointTxt.text = curTotalYellowSeedPoints + "/" + configData.MaxSeedPoint;

                        gainYellowSeedPointTooltip.OnClose(delegate 
                        {
                            gainRedSeedPointTooltip.DisplayTooltipText("Gain Red " + "\n" + "Seed Points: " + totalRedSeedPoints);
                            DisplayPairCardIndicator(bottomCardContainer);
                            gainRedSeedPointTooltip.OnOpen(delegate
                            {
                                DisplaySeedPointVFX(redSeedVFX, gainRedSeedPointTooltip.transform, bottomSeedPointTxt.gameObject, totalRedSeedPoints);
                                bottomSeedPointTxt.text = curTotalRedSeedPoints + "/" + configData.MaxSeedPoint;

                                gainRedSeedPointTooltip.OnClose(delegate
                                {
                                    CheckGameRound();
                                }, 0.5f);
                            });
                        }, 0.5f);
                    });
                }, 0.5f);
            });
        }
        private void DisplayPairCardIndicator(RectTransform _cardContainer)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_MATCH);
            for (int i = 0; i < _cardContainer.childCount; i++)
            {
                _cardContainer.GetChild(i).GetComponent<CardView>().DisplayPairIndicator();
            }
            if(!TutorialManager.Instance.IsOnTutorial) return;
            if(curGameRound > 1) return;
            Invoke(nameof(InvokeEvent),1f);
        }
        private void DisplaySeedPointVFX(CurrencyRewardVFX _vfx, Transform _spawnPosition, GameObject _target, int _totalSeed)
        {
            if (_totalSeed >= 6)
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.POINT_6);
            }
            else
            {
                if (_totalSeed > 0)
                {
                    AudioManager.Instance.PlaySFX(Enums.SFXType.POINT_1);
                }
            }
            CurrencyRewardVFX spawnVFX = Instantiate(_vfx, transform);
            spawnVFX.transform.position = _spawnPosition.position;
            spawnVFX.SetTarget(_target.GetComponent<RectTransform>(), _totalSeed);
        }
        private int CheckCardRankingPoints(RectTransform _cardContainer)
        {
            int totalSeedPoints = 0;
            List<CardView> cardViewList = new List<CardView>();
            for (int i = 0; i < _cardContainer.childCount; i++)
            {
                cardViewList.Add(_cardContainer.GetChild(i).GetComponent<CardView>());
            }
            List<CardView> sortedCardViewList = new List<CardView>();
            sortedCardViewList = cardViewList.OrderBy(x => (int)(x.GetCardData().Type)).ToList();
            //Check 5 Different Cards
            if (((sortedCardViewList[0].GetCardData().Type != sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type != sortedCardViewList[2].GetCardData().Type && sortedCardViewList[0].GetCardData().Type != sortedCardViewList[3].GetCardData().Type && sortedCardViewList[0].GetCardData().Type != sortedCardViewList[4].GetCardData().Type) &&
                (sortedCardViewList[1].GetCardData().Type != sortedCardViewList[2].GetCardData().Type && sortedCardViewList[1].GetCardData().Type != sortedCardViewList[3].GetCardData().Type && sortedCardViewList[1].GetCardData().Type != sortedCardViewList[4].GetCardData().Type) &&
                (sortedCardViewList[2].GetCardData().Type != sortedCardViewList[3].GetCardData().Type && sortedCardViewList[2].GetCardData().Type != sortedCardViewList[4].GetCardData().Type) &&
                (sortedCardViewList[3].GetCardData().Type != sortedCardViewList[4].GetCardData().Type)) && CheckTotalPestCard(sortedCardViewList) == 0)
            {
                totalSeedPoints = 6;
                for (int i = 0; i < sortedCardViewList.Count; i++)
                {
                    sortedCardViewList[i].DisplayPairCard();
                }
            }
            //Check 4 Same Cards
            else if ((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[3].GetCardData().Type) ||
                (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[4].GetCardData().Type))
            {
                totalSeedPoints = 5;
                if (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[3].GetCardData().Type)
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                }
                else if (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)
                {
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                    sortedCardViewList[4].DisplayPairCard();
                }
            }
            //Check FullHouse
            else if (((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[4].GetCardData().Type) ||
                (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)) &&
                CheckTotalPestCard(sortedCardViewList) == 0)
            {
                totalSeedPoints = 4;
                if ((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[4].GetCardData().Type))
                {
                    sortedCardViewList[0].DisplayPairCard(false);
                    sortedCardViewList[1].DisplayPairCard(false);
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                    sortedCardViewList[4].DisplayPairCard();
                }
                else if ((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type))
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard(false);
                    sortedCardViewList[4].DisplayPairCard(false);
                }
            }
            //Check 3 Same Cards
            else if (((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type) ||
                     (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[3].GetCardData().Type) ||
                     (sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)) &&
                     CheckTotalPestCard(sortedCardViewList) <= 2)
            {
                totalSeedPoints = 3;
                if (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[0].GetCardData().Type == sortedCardViewList[2].GetCardData().Type)
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                }
                else if (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[1].GetCardData().Type == sortedCardViewList[3].GetCardData().Type)
                {
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                }
                else if (sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)
                {
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                    sortedCardViewList[4].DisplayPairCard();
                }
            }
            //Check 2 Pair Cards
            else if (((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type) ||
                     (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type) ||
                     (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)) &&
                     CheckTotalPestCard(sortedCardViewList) <= 1)
            {
                totalSeedPoints = 2;
                if (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type)
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard(false);
                    sortedCardViewList[3].DisplayPairCard(false);
                }
                else if (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard(false);
                    sortedCardViewList[4].DisplayPairCard(false);
                }
                else if (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type && sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)
                {
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard(false);
                    sortedCardViewList[4].DisplayPairCard(false);
                }
            }
            //Check 1 Pair Cards
            else if (((sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type) && (sortedCardViewList[0].GetCardData().Type != Enums.CardType.PEST && sortedCardViewList[1].GetCardData().Type != Enums.CardType.PEST)) ||
                     ((sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type) && (sortedCardViewList[1].GetCardData().Type != Enums.CardType.PEST && sortedCardViewList[2].GetCardData().Type != Enums.CardType.PEST)) ||
                     ((sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type) && (sortedCardViewList[2].GetCardData().Type != Enums.CardType.PEST && sortedCardViewList[3].GetCardData().Type != Enums.CardType.PEST)) ||
                     ((sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type) && (sortedCardViewList[3].GetCardData().Type != Enums.CardType.PEST && sortedCardViewList[4].GetCardData().Type != Enums.CardType.PEST)))
            {
                totalSeedPoints = 1;
                if (sortedCardViewList[0].GetCardData().Type == sortedCardViewList[1].GetCardData().Type)
                {
                    sortedCardViewList[0].DisplayPairCard();
                    sortedCardViewList[1].DisplayPairCard();
                }
                else if (sortedCardViewList[1].GetCardData().Type == sortedCardViewList[2].GetCardData().Type)
                {
                    sortedCardViewList[1].DisplayPairCard();
                    sortedCardViewList[2].DisplayPairCard();
                }
                else if (sortedCardViewList[2].GetCardData().Type == sortedCardViewList[3].GetCardData().Type)
                {
                    sortedCardViewList[2].DisplayPairCard();
                    sortedCardViewList[3].DisplayPairCard();
                }
                else if (sortedCardViewList[3].GetCardData().Type == sortedCardViewList[4].GetCardData().Type)
                {
                    sortedCardViewList[3].DisplayPairCard();
                    sortedCardViewList[4].DisplayPairCard();
                }
            }
            return totalSeedPoints;
        }

        private int CheckTotalPestCard(List<CardView> sortedCardViewList)
        {
            int totalPestCard = 0;
            for (int i = 0; i < sortedCardViewList.Count; i++)
            {
                if (sortedCardViewList[i].GetCardData().Type == Enums.CardType.PEST)
                {
                    totalPestCard++;
                }
            }
            return totalPestCard;
        }
        #endregion

        #region ACTION
        private void BeginDragCardAction(CardView _draggedCard)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_PICK);
            _draggedCard.transform.SetParent(rootCanvas.transform);
        }
        private void EndDragCardAction(CardView _draggedCard)
        {
            if (Utils.Overlaps(_draggedCard.GetComponent<RectTransform>(), middleCardContainer))
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.CARD_DROP);
                _draggedCard.transform.SetParent(middleCardContainer);
                _draggedCard.SetIsDragable(false);
                _draggedCard.PlaySmokeEffect();
                curTotalMiddleCard++;
                if (curTotalMiddleCard >= 4)
                {
                    if (TutorialManager.Instance.IsOnTutorial && curTotalMiddleCard == 4 && curGameRound < 2)
                    {
                        TutorialManager.Instance.tutorialStep = Enums.TutorialStep.THIRD_ROUND;
                        return;
                    }
                    ChangeGameState(Enums.GameState.THIRD_ROUND);
                }
                else if (curTotalMiddleCard >= 2)
                {
                    if (TutorialManager.Instance.IsOnTutorial && curTotalMiddleCard == 2 && curGameRound < 2)
                    {
                        TutorialManager.Instance.tutorialStep = Enums.TutorialStep.SECOND_ROUND;
                        return;
                    }
                    ChangeGameState(Enums.GameState.SECOND_ROUND);
                }
                UpdateCardPosition(topCardContainer);
                UpdateCardPosition(bottomCardContainer);
            }
            else
            {
                _draggedCard.ResetPosition();
            }
        }
        private void UpdateCardPosition(RectTransform _cardContainer)
        {
            for (int i = 0; i < _cardContainer.childCount; i++)
            {
                _cardContainer.GetChild(i).GetComponent<CardView>().UpdatePosition();
            }
        }
        #endregion
    }
}
