using System;
using EventBus;
using PokerSeed.Garden;
using PokerSeed.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.UI
{
    public class NewPlantInfoPopUp : MonoBehaviour
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
        
        private EventBindings<PlantInfoPopUpInit> _onPlantInfoPopUpInit;
        private EventBindings<PlantBtnAddListenerEvent> _onPlantInfoPopUpOpen;

        private void Awake()
        {
            _onPlantInfoPopUpInit = new EventBindings<PlantInfoPopUpInit>(Init);
            _onPlantInfoPopUpOpen = new EventBindings<PlantBtnAddListenerEvent>(OnOpen);

            Bus<PlantInfoPopUpInit>.Register(_onPlantInfoPopUpInit);
            Bus<PlantBtnAddListenerEvent>.Register(_onPlantInfoPopUpOpen);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Bus<PlantInfoPopUpInit>.Unregister(_onPlantInfoPopUpInit);
            Bus<PlantBtnAddListenerEvent>.Unregister(_onPlantInfoPopUpOpen);
        }

        public void Init(PlantInfoPopUpInit plantInfoPopUpInit)
        {
            plantImg.sprite = plantInfoPopUpInit.PlantData.PopupAsset;
            plantNameTxt.text = plantInfoPopUpInit.PlantData.Name;
            plantScientificNameTxt.text = plantInfoPopUpInit.PlantData.ScientificName;
            plantFamilyTxt.text = plantInfoPopUpInit.PlantData.Family;
            plantNativeRangeTxt.text = plantInfoPopUpInit.PlantData.NativeRange;
            plantDescriptionTxt.text = plantInfoPopUpInit.PlantData.Description;

            greenSeedImg.gameObject.SetActive(false);
            yellowSeedImg.gameObject.SetActive(false);
            redSeedImg.gameObject.SetActive(false);

            switch (plantInfoPopUpInit.PlantColor)
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
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                OnClose(null);
            });
        }
        public void OnOpen(PlantBtnAddListenerEvent _openPopupCallback)
        {
            gameObject.SetActive(true);
            animHandler.PlayAnimation(false, delegate
            {
                animHandler.StopAnimation(true);
                _openPopupCallback.delegates?.Invoke();
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