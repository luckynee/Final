using PokerSeed.Gameplay;
using PokerSeed.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PokerSeed.Garden;
using PokerSeed.Config;
using PokerSeed.Player;
using System;
using Tutorial;
using UnityEngine.SceneManagement;

namespace PokerSeed.UI
{
    public class SuccessPopup : MonoBehaviour
    {
        [SerializeField] private AnimationConfigData animationConfigData;
        [SerializeField] private Enums.PlantColor plantColor;
        [SerializeField] private List<PlantData> plantDataList;
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Image plantImg;
        [SerializeField] private TextMeshProUGUI plantNameTxt;
        [SerializeField] private GameObject unlockSeedContainer;
        [SerializeField] private Button unlockSeedBtn;
        [SerializeField] private GameObject completedSeedContainer;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button gardenBtn;
        [SerializeField] private Button nextBtn;

        private CardController cardController;

        #region DEFAULT
        public void Init(CardController _cardController)
        {
            cardController = _cardController;

            unlockSeedBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                UnlockSeed();
            });
            restartBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                VFXManager.Instance.StopVFX();
                OnClose(delegate
                {
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null,
                    delegate
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    });
                });
            });
            gardenBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                VFXManager.Instance.StopVFX();
                OnClose(delegate 
                {
                    SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(false, null,
                    delegate
                    {
                        SceneManager.LoadScene(Names.GardenSceneName);
                    });
                });
            });
            nextBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                VFXManager.Instance.StopVFX();
                cardController.CheckSuccessPopup();
            });

            unlockSeedBtn.interactable = true;
            restartBtn.interactable = false;
            gardenBtn.interactable = false;
            nextBtn.interactable = false;
            CheckLastPopup(1);
            if (PlayerDataManager.Instance.HasOwnedAllPlants(plantColor))
            {
                unlockSeedContainer.SetActive(false);
                completedSeedContainer.SetActive(true);
                restartBtn.interactable = true;
                gardenBtn.interactable = true;
            }
            else
            {
                unlockSeedContainer.SetActive(true);
                completedSeedContainer.SetActive(false);
            }
        }
        public void OnOpen(Action _openPopupCallback)
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.SUCCESS);
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
                animHandler.StopAnimation();
                _closePopupCallback?.Invoke();
                gameObject.SetActive(false);
            });
        }
        #endregion

        #region PROCESS
        public void CheckLastPopup(int _totalOpenedPopup)
        {
            if (_totalOpenedPopup <= 0)
            {
                nextBtn.gameObject.SetActive(false);
                restartBtn.gameObject.SetActive(true);
                gardenBtn.gameObject.SetActive(true);
            }
            else
            {
                nextBtn.gameObject.SetActive(true);
                restartBtn.gameObject.SetActive(false);
                gardenBtn.gameObject.SetActive(false);
            }
        }
        private void UnlockSeed()
        {
            AudioManager.Instance.PlaySFX(Enums.SFXType.ROULETTE);
            unlockSeedBtn.enabled = false;
            StartCoroutine(PlantGachaRoutine(
            delegate 
            {
                VFXManager.Instance.PlayVFX(Enums.VFXTrigger.PLANT_UNLOCKED, new Vector2(0, 10));
                restartBtn.interactable = true;
                gardenBtn.interactable = true;
                nextBtn.interactable = true;
                if(!TutorialManager.Instance.IsOnTutorial)return;
                Debug.Log("Dapat Tanaman");
            }));
        }
        private IEnumerator PlantGachaRoutine(Action _finishGachaCallback = null)
        {
            List<PlantData> gachaPlantList = new List<PlantData>();
            int randomPlant = UnityEngine.Random.Range(0, gachaPlantList.Count);
            for(int i = 0; i < PlayerDataManager.Instance.GetPlayerOwnedPlant(plantColor).Count; i++)
            {
                if (!PlayerDataManager.Instance.GetPlayerOwnedPlant(plantColor)[i].Owned)
                {
                    gachaPlantList.Add(PlayerDataManager.Instance.GetPlayerOwnedPlant(plantColor)[i].PlantData);
                }
            }
            int curGachaTime = 0;
            while (curGachaTime < animationConfigData.TotalGachaToReveal)
            {
                for(int i = 0; i < gachaPlantList.Count; i++)
                {
                    plantImg.sprite = gachaPlantList[i].PopupAsset;
                    yield return new WaitForSeconds(animationConfigData.PlantGachaDelay);
                }
                curGachaTime++;
                yield return new WaitForSeconds(animationConfigData.PlantGachaDelay);
            }
            plantImg.sprite = gachaPlantList[randomPlant].PopupAsset;
            plantNameTxt.text = gachaPlantList[randomPlant].Name;
            PlayerDataManager.Instance.UnlockPlant(plantColor, gachaPlantList[randomPlant].Id);
            _finishGachaCallback?.Invoke();
        }
        #endregion
    }
}
