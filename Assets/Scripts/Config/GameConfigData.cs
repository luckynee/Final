using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.Config
{
    [CreateAssetMenu(fileName = "GameConfigData", menuName = "PokerSeed/Config/GameConfigData")]
    public class GameConfigData : ScriptableObject
    {
        public int MaxGameRound = 3;
        public int MaxCardSlot = 6;
        public int MaxPestCardPerRound = 3;
        public int MaxSeedPoint = 10;
    }
}
