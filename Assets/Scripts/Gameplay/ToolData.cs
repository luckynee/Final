using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.Gameplay
{
    [CreateAssetMenu(fileName = "ToolData", menuName = "PokerSeed/Gameplay/ToolData")]
    public class ToolData : ScriptableObject
    {
        public string Id;
        public string Description;
        public CardData EffectCard;
        public Sprite ActiveAsset;
        public Sprite InactiveAsset;
    }
}
