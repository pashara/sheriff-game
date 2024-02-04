using System;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameResources;
using UnityEngine;

namespace Sheriff.GameStructures
{
    [Serializable]
    public class CardInfo
    {
        [SerializeField] private GameResourceType resourceType;
        [SerializeField] private GameResourceCategory category;
        [SerializeField] private string title;
        [SerializeField] private int cost;
        [SerializeField] private int fine;
        [SerializeField] private CardView cardPrefab;



        public GameResourceType ResourceType => resourceType;
        public GameResourceCategory Category => category;
        public string Title => title;
        public int Cost => cost;
        public int Fine => fine;
        public CardView CardView => cardPrefab;
    }
}