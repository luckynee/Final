using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.Config
{
    [CreateAssetMenu(fileName = "AnimationConfigData", menuName = "PokerSeed/Config/AnimationConfigData")]
    public class AnimationConfigData : ScriptableObject
    {
        public float PlantGachaDelay = 0.1f; //Duration before change to another picture
        public int TotalGachaToReveal = 3; //How many times the gacha will randomize before showing the final
    }
}
