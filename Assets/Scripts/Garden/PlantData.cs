using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerSeed.Garden
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "PokerSeed/Garden/PlantData")]
    public class PlantData : ScriptableObject
    {
        public string Id;
        public string Name;
        public string ScientificName;
        public string Family;
        public string NativeRange;
        [TextArea]
        public string Description;
        public Sprite Asset;
        public Sprite PopupAsset;
        public float OffsetYPos;
        public RuntimeAnimatorController AnimatorController;
    }
}
