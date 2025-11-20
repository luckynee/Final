using System;
using System.Collections.Generic;
using EventBus;
using PokerSeed.General;
using PokerSeed.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerSeed.Garden
{
    public class NewPlantView : MonoBehaviour
    {
        [SerializeField] private Enums.PlantColor plantColor;
        [SerializeField] private GameObject plantPrefab;
        
        private List<PlantData> _plantDataList = new List<PlantData>();
        private NewPlantController _newPlantController;

        public void Init(List<PlantData> plantDataList, PlantController plantController)
        {
            _plantDataList = plantDataList;

            DisplayPlant();
        }
        
        public void Init(List<PlantData> plantDataList, NewPlantController plantController)
        {
            _plantDataList = plantDataList;
            _newPlantController = plantController;

            DisplayPlant();
        }

        private void DisplayPlant()
        {
            foreach (var plant in _plantDataList)
            {
               var plantObj = Instantiate(plantPrefab, transform);
               plantObj.transform.GetChild(1).TryGetComponent(out Image image);
               image.sprite = plant.Asset;
               if (PlayerDataManager.Instance.HasOwnedSpecificPlant(plantColor, plant.Id))
               {
                   var plantButton = plantObj.gameObject.AddComponent<Button>();
                   plantButton.onClick.AddListener(delegate
                   {
                       //AudioManager.Instance.PlaySFX(Enums.SFXType.BUTTON_CLICK);
                       Bus<OnSfxTrigger>.Raise(new OnSfxTrigger(Enums.SFXType.BUTTON_CLICK));
                       
                       _newPlantController.OpenPlantInfoPopup(plant, plantColor);
                   });
                   
                   var animatorController = plantObj.transform.GetChild(1).gameObject.AddComponent<Animator>();
                   animatorController.runtimeAnimatorController = plant.AnimatorController;
                   image.color = Color.white;
               }
               else
               {
                   image.color = Color.black;
               }
            }
        }
    }
    
}
