using System;
using System.Collections.Generic;
using Sheriff.GameResources;
using UnityEngine;

namespace Sheriff.GameStructures
{
    [CreateAssetMenu]
    public class ResultBonusesConfig : ScriptableObject
    {
        [Serializable]
        public class ResourcesBonusInfo
        {
            public List<ResourceBonusInfo> bonusInfo;
        }

        [Serializable]
        public class ResourceBonusInfo
        {
            public GameResourceType resourceType;
            public int bonus;
        }


        public ResourcesBonusInfo KingBonus;
        public ResourcesBonusInfo QueenBonus;
    }
}