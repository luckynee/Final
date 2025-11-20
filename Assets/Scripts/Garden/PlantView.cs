using PokerSeed.General;
using PokerSeed.Player;
using PokerSeed.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.Garden
{
    public class PlantView : MonoBehaviour
    {
        [SerializeField] private Enums.PlantColor plantColor;
        [SerializeField] private Image plantImg;
        [SerializeField] private Button plantInfoBtn;
        [SerializeField] private Button nextPlantBtn;
        [SerializeField] private Button prevPlantBtn;

        private PlantController plantController;
        private List<PlantData> curPlantDataList = new List<PlantData>();
        private int curPlantIndex;
        private float initPlantYPos;

        public void Init(List<PlantData> _plantDataList, PlantController _plantController)
        {
            curPlantDataList = _plantDataList;
            plantController = _plantController;

            initPlantYPos = plantImg.GetComponent<RectTransform>().anchoredPosition.y;

            nextPlantBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                NextPlant();
            });
            prevPlantBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                PrevPlant();
            });
            plantInfoBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                plantController.OpenPlantInfoPopup(curPlantDataList[curPlantIndex], plantColor);
            });

            DisplayPlant();
        }
        private void DisplayPlant()
        {
            plantImg.sprite = curPlantDataList[curPlantIndex].Asset;
            if (PlayerDataManager.Instance.HasOwnedSpecificPlant(plantColor, curPlantDataList[curPlantIndex].Id))
            {
                plantImg.color = Color.white;
                plantInfoBtn.enabled = true;
            }
            else
            {
                plantImg.color = Color.black;
                plantInfoBtn.enabled = false;
            }
            plantImg.GetComponent<RectTransform>().anchoredPosition = new Vector2(plantImg.GetComponent<RectTransform>().anchoredPosition.x, initPlantYPos + curPlantDataList[curPlantIndex].OffsetYPos);
        }
        private void NextPlant()
        {
            curPlantIndex++;
            if (curPlantIndex >= curPlantDataList.Count)
            {
                curPlantIndex = 0;
            }
            DisplayPlant();
        }
        private void PrevPlant()
        {
            curPlantIndex--;
            if (curPlantIndex < 0)
            {
                curPlantIndex = curPlantDataList.Count - 1;
            }
            DisplayPlant();
        }
    }
}
