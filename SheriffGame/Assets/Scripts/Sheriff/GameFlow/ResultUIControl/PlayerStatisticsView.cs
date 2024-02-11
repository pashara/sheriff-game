using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Transform allowedCardsRoot;
        [SerializeField] private Transform deniedCardsRoot;
        [SerializeField] private PlayerCardStatisticElement cardStatisticElement;
        [SerializeField] private TMP_Text onHandCardsLabel;
        [SerializeField] private TMP_Text allowedCardsCountLabel;
        [SerializeField] private TMP_Text deniedCardsCountLabel;
        [Inject] private DiContainer _container;

        private List<IDisposable> spawned = new();
        
        public void Fill(int place, PlayerEntity player)
        {
            placeLabel.SetText($"{place}");

            var allowedCardsCount = player.transferredResources.Value.AllowedResources.Select(x => x.Value).Sum();
            var deniedCardsCount = player.transferredResources.Value.NotAllowedResources.Select(x => x.Value).Sum();
            
            onHandCardsLabel.SetText($"{allowedCardsCount + deniedCardsCount}");
            allowedCardsCountLabel.SetText($"{allowedCardsCount}");
            deniedCardsCountLabel.SetText($"{deniedCardsCount}");
            

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