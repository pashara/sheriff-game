using System;
using System.Collections.Generic;
using Sheriff.DataBase;
using Sheriff.GameFlow.ResultUIControl.Cards;
using Sheriff.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private TMP_Text totalPointsLabel;
        [Inject] private DiContainer _container;
        private readonly List<PlayerCardStatisticElement> _spawned = new();

        public void Fill(ResultCalculateHandler.Recalculations calculations, PlayerEntity player)
        {
            placeLabel.SetText($"#{calculations.Place}");
            nickNameLabel.SetText(player.nickname.Value);

            var cardsCalculations = calculations.TotalCardsInfo;

            onHandCardsLabel.SetText(
                $"${cardsCalculations.allowedCardsCost + cardsCalculations.deniedCardsCost} " +
                $"({cardsCalculations.allowedCardsCount + cardsCalculations.deniedCardsCount})");
            
            allowedCardsCountLabel.SetText(
                $"${cardsCalculations.allowedCardsCost} ({cardsCalculations.allowedCardsCount})");
            deniedCardsCountLabel.SetText(
                $"${cardsCalculations.deniedCardsCost} ({cardsCalculations.deniedCardsCount})");
            
            totalPointsLabel.SetText($"TOTAL: {calculations.totalBonus}");


            foreach (var resource in player.transferredResources.Value.AllowedResources)
                SpawnCardStatistic(resource.Key, resource.Value, allowedCardsRoot, calculations);

            foreach (var resource in player.transferredResources.Value.NotAllowedResources)
                SpawnCardStatistic(resource.Key, resource.Value, deniedCardsRoot, calculations);
        }
        

        void SpawnCardStatistic(GameResourceType gameResourceType, int count, Transform root, ResultCalculateHandler.Recalculations calculations)
        {
            if (count <= 0)
                return;
            
            var statisticElement = _container
                .InstantiatePrefabForComponent<PlayerCardStatisticElement>(cardStatisticElement, root);
            statisticElement.Initialize(gameResourceType, count, calculations);
            _spawned.Add(statisticElement);
        }
        
        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void UpdateLayout()
        {
            foreach (var element in _spawned)
            {
                element.UpdateLayout();
            }
            LayoutRebuilder.MarkLayoutForRebuild(allowedCardsRoot as RectTransform);
            LayoutRebuilder.MarkLayoutForRebuild(deniedCardsRoot as RectTransform);
        }
    }
}