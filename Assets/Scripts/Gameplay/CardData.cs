using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.Gameplay
{
    [CreateAssetMenu(fileName = "CardData", menuName = "PokerSeed/Gameplay/CardData")]
    public class CardData : ScriptableObject
    {
        public Enums.CardType Type;
        public string Name;
        public Sprite Background;
        public Sprite Asset;
    }
}
