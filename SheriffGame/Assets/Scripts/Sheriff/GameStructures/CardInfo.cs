using System;
using Sheriff.GameResources;
using Sheriff.GameView;
using UnityEngine;

namespace Sheriff.GameStructures
{
    [Serializable]
    public class CardInfo
    {
        [SerializeField] private GameResourceType resourceType;
        [SerializeField] private GameResourceCategory category;
        [SerializeField] private string title;
        [SerializeField] private CardInnerView innerPrefab;
        [SerializeField] private int cost;
        [SerializeField] private int fine;
    }
}