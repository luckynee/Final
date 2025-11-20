using System;
using System.Collections.Generic;
using _PAProjcet;
using EventBus;
using PokerSeed.General;
using PokerSeed.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.Garden
{
    public class NewPlantController : MonoBehaviour
    {
        [Header("GENERAL")]
        [Space(5)]
        [SerializeField] private List<PlantData> greenPlantDataList;
        [SerializeField] private List<PlantData> yellowPlantDataList;
        [SerializeField] private List<PlantData> redPlantDataList;
        [SerializeField] private List<PlantData> goldenPlantDataList;
        //[SerializeField] private PlantView greenPlantView;
        //[SerializeField] private PlantView yellowPlantView;
        //[SerializeField] private PlantView redPlantView;
        [SerializeField] private Button settingBtn;
        
        [SerializeField] private NewPlantView greenPlantView;
        [SerializeField] private NewPlantView yellowPlantView;
        [SerializeField] private NewPlantView redPlantView;
        [SerializeField] private NewPlantView goldenPlantView;
        
       // [SerializeField] private NewPlantInfoPopUp plantInfoPopup;
        //[SerializeField] private NewSettingsPopUp settingPopup;

        private void Awake()
        {
            greenPlantView.Init(greenPlantDataList, this);
            yellowPlantView.Init(yellowPlantDataList, this);
            redPlantView.Init(redPlantDataList, this);
            goldenPlantView.Init(goldenPlantDataList, this);
            
            
        }
        private void Start()
        {
            //AudioManager.Instance.PlayAmbience(Enums.AmbienceType.GAMEPLAY);
            Bus<OnAmbienceTrigger>.Raise(new OnAmbienceTrigger(Enums.AmbienceType.GAMEPLAY));
            SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(true);
            
            //settingPopup.Init();

            settingBtn.onClick.AddListener(delegate
            {
                //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                Bus<SettingBtnAddListenerEvent>.Raise(new SettingBtnAddListenerEvent(null));
                //settingPopup.OnOpen(null);
            });
            
            Bus<MainMenuInit>.Raise(new MainMenuInit());
        }

        private void OnDisable()
        {
            Bus<OnAmbienceStop>.Raise(new OnAmbienceStop());
        }

        public void OpenPlantInfoPopup(PlantData _plantData, Enums.PlantColor _plantColor)
        {
            Bus<PlantInfoPopUpInit>.Raise(new PlantInfoPopUpInit(_plantData, _plantColor));
            Bus<PlantBtnAddListenerEvent>.Raise(new PlantBtnAddListenerEvent(null));
            //plantInfoPopup.Init(_plantData, _plantColor);
            //plantInfoPopup.OnOpen(null);
        }
    }
}