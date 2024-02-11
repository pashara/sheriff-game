using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.GameFlow.ResultUIControl.Cards;
using Sheriff.GameResources;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.ResultUIControl
{
    public class PlayerStatisticsView : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text placeLabel;
        [SerializeField] private TMP_Text nickNameLabel;
        [SerializeField] private Transform allowedCardsRoot;
        [SerializeField] private Transform deniedCardsRoot;
        [SerializeField] private PlayerCardStatisticElement cardStatisticElement;
        [SerializeField] private TMP_Text onHandCardsLabel;
        [SerializeField] private TMP_Text allowedCardsCountLabel;
        [SerializeField] private TMP_Text deniedCardsCountLabel;
        [Inject] private DiContainer _container;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        private List<IDisposable> spawned = new();
        
        public void Fill(ResultCalculateHandler.Recalculations place, PlayerEntity player)
        {
            placeLabel.SetText($"{place}");
            nickNameLabel.SetText(player.nickname.Value);

            var cardsCalculations = place.TotalCardsInfo;
            
            onHandCardsLabel.SetText($"${cardsCalculations.allowedCardsCost + cardsCalculations.deniedCardsCost} ({cardsCalculations.allowedCardsCount + cardsCalculations.deniedCardsCount})");
            allowedCardsCountLabel.SetText($"${cardsCalculations.allowedCardsCost} ({cardsCalculations.allowedCardsCount})");
            deniedCardsCountLabel.SetText($"${cardsCalculations.deniedCardsCost} ({cardsCalculations.deniedCardsCount})");
            

            foreach (var resource in player.transferredResources.Value.AllowedResources)
                SpawnCardStatistic(resource.Key, resource.Value, allowedCardsRoot);

            foreach (var resource in player.transferredResources.Value.NotAllowedResources)
                SpawnCardStatistic(resource.Key, resource.Value, deniedCardsRoot);
        }



        void SpawnCardStatistic(GameResourceType gameResourceType, int count, Transform root)
        {
            if (count <= 0)
                return;
            
            var statisticElement = _container
                .InstantiatePrefabForComponent<PlayerCardStatisticElement>(cardStatisticElement, root);
            statisticElement.Initialize(gameResourceType, count);
            spawned.Add(statisticElement);
        }
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}