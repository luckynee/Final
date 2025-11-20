using PokerSeed.Garden;
using PokerSeed.General;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class PlantInfoPopup : MonoBehaviour
    {
        [SerializeField] private AnimationHandler animHandler;
        [SerializeField] private Image greenSeedImg;
        [SerializeField] private Image yellowSeedImg;
        [SerializeField] private Image redSeedImg;
        [SerializeField] private Image plantImg;
        [SerializeField] private TextMeshProUGUI plantNameTxt;
        [SerializeField] private TextMeshProUGUI plantScientificNameTxt;
        [SerializeField] private TextMeshProUGUI plantFamilyTxt;
        [SerializeField] private TextMeshProUGUI plantNativeRangeTxt;
        [SerializeField] private TextMeshProUGUI plantDescriptionTxt;
        [SerializeField] private Button closeBtn;

        public void Init(PlantData _plantData, Enums.PlantColor _plantColor)
        {
            plantImg.sprite = _plantData.PopupAsset;
            plantNameTxt.text = _plantData.Name;
            plantScientificNameTxt.text = _plantData.ScientificName;
            plantFamilyTxt.text = _plantData.Family;
            plantNativeRangeTxt.text = _plantData.NativeRange;
            plantDescriptionTxt.text = _plantData.Description;

            greenSeedImg.gameObject.SetActive(false);
            yellowSeedImg.gameObject.SetActive(false);
            redSeedImg.gameObject.SetActive(false);

            switch (_plantColor)
            {
                case Enums.PlantColor.GREEN:
                    greenSeedImg.gameObject.SetActive(true);
                    break;
                case Enums.PlantColor.YELLOW:
                    yellowSeedImg.gameObject.SetActive(true);
                    break;
                case Enums.PlantColor.RED:
                    redSeedImg.gameObject.SetActive(true);
                    break;
            }

            closeBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                OnClose(null);
            });
        }
        public void OnOpen(Action _openPopupCallback)
        {
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
    }
}
