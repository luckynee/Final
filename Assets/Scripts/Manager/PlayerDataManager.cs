using PokerSeed.Garden;
using PokerSeed.General;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace PokerSeed.Player
{
    public class PlayerDataManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerOwnedPlant> playerOwnedGreenPlantList;
        [SerializeField] private List<PlayerOwnedPlant> playerOwnedYellowPlantList;
        [SerializeField] private List<PlayerOwnedPlant> playerOwnedRedPlantList;
        [SerializeField] private List<PlayerOwnedPlant> playerOwnedGoldenPlantList;

        private bool hasShownTutorial;
        public bool HasShownTutorial => hasShownTutorial;

        public static PlayerDataManager Instance;

        #region UNITY
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
            
            //DeleteData();
            LoadData();
            hasShownTutorial = false;
        }
        #endregion

        #region GETTER
        public List<PlayerOwnedPlant> GetPlayerOwnedPlant(Enums.PlantColor _plantColor)
        {
            switch (_plantColor)
            {
                case Enums.PlantColor.GREEN:
                    return playerOwnedGreenPlantList;
                case Enums.PlantColor.YELLOW:
                    return playerOwnedYellowPlantList;
                case Enums.PlantColor.RED:
                    return playerOwnedRedPlantList;
                case Enums.PlantColor.GOLDEN:
                    return playerOwnedGoldenPlantList;
            }
            return null;
        }
        public bool HasOwnedAllPlants(Enums.PlantColor _plantColor)
        {
            int totalOwnedPlant = 0;
            switch (_plantColor)
            {
                case Enums.PlantColor.GREEN:
                    for (int i = 0; i < playerOwnedGreenPlantList.Count; i++)
                    {
                        if (playerOwnedGreenPlantList[i].Owned)
                        {
                            totalOwnedPlant++;
                        }
                    }
                    if (totalOwnedPlant == playerOwnedGreenPlantList.Count)
                    {
                        return true;
                    }
                    break;
                case Enums.PlantColor.YELLOW:
                    for (int i = 0; i < playerOwnedYellowPlantList.Count; i++)
                    {
                        if (playerOwnedYellowPlantList[i].Owned)
                        {
                            totalOwnedPlant++;
                        }
                    }
                    if (totalOwnedPlant == playerOwnedYellowPlantList.Count)
                    {
                        return true;
                    }
                    break;
                case Enums.PlantColor.RED:
                    for (int i = 0; i < playerOwnedRedPlantList.Count; i++)
                    {
                        if (playerOwnedRedPlantList[i].Owned)
                        {
                            totalOwnedPlant++;
                        }
                    }
                    if (totalOwnedPlant == playerOwnedRedPlantList.Count)
                    {
                        return true;
                    }
                    break;
                case Enums.PlantColor.GOLDEN:
                    for (int i = 0; i < playerOwnedGoldenPlantList.Count; i++)
                    {
                        if (playerOwnedGoldenPlantList[i].Owned)
                        {
                            totalOwnedPlant++;
                        }
                    }

                    if (totalOwnedPlant == playerOwnedGoldenPlantList.Count)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        public bool HasOwnedSpecificPlant(Enums.PlantColor _plantColor, string _plantId)
        {
            switch (_plantColor)
            {
                case Enums.PlantColor.GREEN:
                    for (int i = 0; i < playerOwnedGreenPlantList.Count; i++)
                    {
                        if (playerOwnedGreenPlantList[i].PlantData.Id == _plantId &&
                            playerOwnedGreenPlantList[i].Owned)
                        {
                            return true;
                        }
                    }
                    break;
                case Enums.PlantColor.YELLOW:
                    for (int i = 0; i < playerOwnedYellowPlantList.Count; i++)
                    {
                        if (playerOwnedYellowPlantList[i].PlantData.Id == _plantId &&
                            playerOwnedYellowPlantList[i].Owned)
                        {
                            return true;
                        }
                    }
                    break;
                case Enums.PlantColor.RED:
                    for (int i = 0; i < playerOwnedRedPlantList.Count; i++)
                    {
                        if (playerOwnedRedPlantList[i].PlantData.Id == _plantId &&
                            playerOwnedRedPlantList[i].Owned)
                        {
                            return true;
                        }
                    }
                    break;
                case Enums.PlantColor.GOLDEN:
                    for (int i = 0; i < playerOwnedGoldenPlantList.Count; i++)
                    {
                        if (playerOwnedGoldenPlantList[i].PlantData.Id == _plantId &&
                            playerOwnedGoldenPlantList[i].Owned)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        #endregion

        #region PROCESS
        public void UnlockPlant(Enums.PlantColor _plantColor, string _plantId)
        {
            switch (_plantColor)
            {
                case Enums.PlantColor.GREEN:
                    for (int i = 0; i < playerOwnedGreenPlantList.Count; i++)
                    {
                        if (playerOwnedGreenPlantList[i].PlantData.Id == _plantId)
                        {
                            playerOwnedGreenPlantList[i].Owned = true;
                        }
                    }
                    break;
                case Enums.PlantColor.YELLOW:
                    for (int i = 0; i < playerOwnedYellowPlantList.Count; i++)
                    {
                        if (playerOwnedYellowPlantList[i].PlantData.Id == _plantId)
                        {
                            playerOwnedYellowPlantList[i].Owned = true;
                        }
                    }
                    break;
                case Enums.PlantColor.RED:
                    for (int i = 0; i < playerOwnedRedPlantList.Count; i++)
                    {
                        if (playerOwnedRedPlantList[i].PlantData.Id == _plantId)
                        {
                            playerOwnedRedPlantList[i].Owned = true;
                        }
                    }
                    break;
                case Enums.PlantColor.GOLDEN:
                    for (int i = 0; i < playerOwnedGoldenPlantList.Count; i++)
                    {
                        if (playerOwnedGoldenPlantList[i].PlantData.Id == _plantId)
                        {
                            playerOwnedGoldenPlantList[i].Owned = true;
                        }
                    }
                    break;
            }
            SaveData();
        }
        public void FinishTutorial()
        {
            hasShownTutorial = true;
            SaveData();
        }
        #endregion

        #region SAVELOAD
        public void SaveData()
        {
            for(int i = 0; i < playerOwnedGreenPlantList.Count; i++)
            {
                PlayerPrefs.SetInt(Names.OwnedPlantId + playerOwnedGreenPlantList[i].PlantData.Id, playerOwnedGreenPlantList[i].Owned ? 1 : 0);
            }
            for (int i = 0; i < playerOwnedYellowPlantList.Count; i++)
            {
                PlayerPrefs.SetInt(Names.OwnedPlantId + playerOwnedYellowPlantList[i].PlantData.Id, playerOwnedYellowPlantList[i].Owned ? 1 : 0);
            }
            for (int i = 0; i < playerOwnedRedPlantList.Count; i++)
            {
                PlayerPrefs.SetInt(Names.OwnedPlantId + playerOwnedRedPlantList[i].PlantData.Id, playerOwnedRedPlantList[i].Owned ? 1 : 0);
            }

            for (int i = 0; i < playerOwnedGoldenPlantList.Count; i++)
            {
                PlayerPrefs.SetInt(Names.OwnedPlantId + playerOwnedGoldenPlantList[i].PlantData.Id, playerOwnedGoldenPlantList[i].Owned ? 1 : 0);
            }
            PlayerPrefs.SetInt(Names.FinishedTutorial, hasShownTutorial ? 1 : 0);
            PlayerPrefs.Save();
        }
        private void LoadData()
        {
            for (int i = 0; i < playerOwnedGreenPlantList.Count; i++)
            {
                playerOwnedGreenPlantList[i].Owned = (PlayerPrefs.GetInt(Names.OwnedPlantId + playerOwnedGreenPlantList[i].PlantData.Id) == 1) ? true : false;
            }
            for (int i = 0; i < playerOwnedYellowPlantList.Count; i++)
            {
                playerOwnedYellowPlantList[i].Owned = (PlayerPrefs.GetInt(Names.OwnedPlantId + playerOwnedYellowPlantList[i].PlantData.Id) == 1) ? true : false;
            }
            for (int i = 0; i < playerOwnedRedPlantList.Count; i++)
            {
                playerOwnedRedPlantList[i].Owned = (PlayerPrefs.GetInt(Names.OwnedPlantId + playerOwnedRedPlantList[i].PlantData.Id) == 1) ? true : false;
            }

            for (int i = 0; i < playerOwnedGoldenPlantList.Count; i++)
            {
                playerOwnedGoldenPlantList[i].Owned = (PlayerPrefs.GetInt(Names.OwnedPlantId + playerOwnedGoldenPlantList[i].PlantData.Id) == 1) ? true : false;
            }
            hasShownTutorial = (PlayerPrefs.GetInt(Names.FinishedTutorial) == 1) ? true : false;
        }
        private void DeleteData()
        {
            PlayerPrefs.DeleteAll();
        }
        #endregion

        [System.Serializable]
        public class PlayerOwnedPlant
        {
            public PlantData PlantData;
            public bool Owned;
        }
    }
}
