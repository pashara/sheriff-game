using System;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameFlow.States.ClassicGame.View.UI;
using Sheriff.GameResources;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private CardViewUI uiCardPrefab;
        [SerializeField] private Sprite icon;



        public GameResourceType ResourceType => resourceType;
        public GameResourceCategory Category => category;
        public string Title => title;
        public int Cost => cost;
        public int Fine => fine;
        public CardView CardView => cardPrefab;
        public CardViewUI UICardCardView => uiCardPrefab;

        public Sprite Icon => icon;
    }
}