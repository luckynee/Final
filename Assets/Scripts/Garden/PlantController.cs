using PokerSeed.General;
using PokerSeed.UI;
using System.Collections;
using System.Collections.Generic;
using PokerSeed.Player;
using UnityEngine;
using UnityEngine.UI;

namespace PokerSeed.Garden
{
    public class PlantController : MonoBehaviour
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

        [Header("POPUP")]
        [Space(5)]
        [SerializeField] private PlantInfoPopup plantInfoPopup;
        [SerializeField] private SettingPopup settingPopup;

        private void Awake()
        {
            greenPlantView.Init(greenPlantDataList, this);
            yellowPlantView.Init(yellowPlantDataList, this);
            redPlantView.Init(redPlantDataList, this);
            goldenPlantView.Init(goldenPlantDataList, this);
            
            settingPopup.Init();

            settingBtn.onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                settingPopup.OnOpen(null);
            });
        }
        private void Start()
        {
            AudioManager.Instance.PlayAmbience(Enums.AmbienceType.GAMEPLAY);
            SceneTransitionManager.Instance.PlaySceneTopBottomVerticalAnimation(true);
        }
        public void OpenPlantInfoPopup(PlantData _plantData, Enums.PlantColor _plantColor)
        {
            plantInfoPopup.Init(_plantData, _plantColor);
            plantInfoPopup.OnOpen(null);
        }
    }
}
