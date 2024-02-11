using System;
using Sheriff.DataBase;
using Sheriff.GameFlow.States.ClassicGame.View.UI;
using Sheriff.GameResources;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.ResultUIControl.Cards
{
    public class PlayerCardStatisticElement : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform cardRoot;
        [SerializeField] private TMP_Text cardsIncome;

        private IDisposable cardView;

        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;
        
        public void Initialize(GameResourceType gameResourceType, int count)
        {
            var config = _cardConfigProvider.Get(gameResourceType);
            var cardInstance = _container.InstantiatePrefabForComponent<CardViewUI>(config.UICardCardView, cardRoot);
            cardInstance.Initialize();
            cardInstance.ApplyCount(count);
            cardsIncome.SetText($"${config.Cost * count}");
            cardView = cardInstance;
        }
        
        public void Dispose()
        {
            cardView?.Dispose();
            Destroy(gameObject);
        }
    }
}