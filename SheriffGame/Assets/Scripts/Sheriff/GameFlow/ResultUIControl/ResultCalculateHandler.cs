using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Sheriff.GameStructures;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.ResultUIControl
{
    public class ResultCalculateHandler
    {
        [Serializable]
        public class Recalculations
        {
            public PlayerEntityId PlayerEntityId;
            public int Place;

            public KingBonus KingBonus = null;
            public QueenBonus QueenBonus = null;
            
            public TotalCardsInfo TotalCardsInfo;

            public int totalBonus;
        }
        
        [Serializable]
        public class KingBonus
        {
            public GameResourceType resourceType;
            public int bonus;
        }
        
        [Serializable]
        public class QueenBonus
        {
            public GameResourceType resourceType;
            public int bonus;
        }

        [Serializable]
        public class TotalCardsInfo
        {
            public int deniedCardsCount;
            public int allowedCardsCost;
            public int allowedCardsCount;
            public int deniedCardsCost;

            public Dictionary<GameResourceType, int> TransferredResources = new();
        }
        
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private ResultBonusesConfig _resultBonusesConfig;
        
        
        public List<Recalculations> Calculate()
        {
            var calculations = _ecsContextProvider.Context.player.GetEntities().Select(CalculatePlayer).ToList();
            ApplyAllBonuses(calculations);
            ApplyPlaces(calculations);
            return calculations;
        }

        private void ApplyAllBonuses(List<Recalculations> calculations)
        {
            calculations.ForEach(x =>
            {
                x.totalBonus = x.TotalCardsInfo.allowedCardsCost + x.TotalCardsInfo.deniedCardsCost;
            });
            
            var bonuses = CalculateBonuses(calculations);
            ApplyKingBonus(bonuses, calculations);
            ApplyQueenBonus(bonuses, calculations);
        }

        void ApplyPlaces(List<Recalculations> calculations)
        {
            calculations.Sort((a, b) => b.totalBonus - a.totalBonus);

            for (var i = 0; i < calculations.Count; i++)
            {
                calculations[i].Place = i + 1;
            }
        }

        private void ApplyKingBonus(List<(GameResourceType, int)> bonusInfo, List<Recalculations> recalculationsList)
        {
            var bonusConfig = _resultBonusesConfig.KingBonus.bonusInfo;
            var elements = bonusConfig.Select(x => x.resourceType).ToHashSet();
            var element = bonusInfo.FirstOrDefault(x => elements.Contains(x.Item1));
            if (element.Item1 == GameResourceType.None)
                return;

            bonusInfo.Remove(element);

            var potentialElements = recalculationsList
                .Where(x => x.TotalCardsInfo.TransferredResources.TryGetValue(element.Item1, out var v) &&
                            v == element.Item2).ToList();

            var count = potentialElements.Count;
            var config = bonusConfig.FirstOrDefault(x => x.resourceType == element.Item1);
            var potentialBonus = config?.bonus ?? 0;
            
            foreach (var recalculation in potentialElements)
            {
                var bonus = potentialBonus / count;
                recalculation.totalBonus += bonus;
                recalculation.KingBonus = new KingBonus()
                {
                    bonus = bonus,
                    resourceType = element.Item1,
                };
            }
        }


        private void ApplyQueenBonus(List<(GameResourceType, int)> bonusInfo, List<Recalculations> recalculationsList)
        {
            var bonusConfig = _resultBonusesConfig.QueenBonus.bonusInfo;
            var elements = bonusConfig.Select(x => x.resourceType).ToHashSet();
            var element = bonusInfo.FirstOrDefault(x => elements.Contains(x.Item1));
            if (element.Item1 == GameResourceType.None)
                return;

            bonusInfo.Remove(element);

            var potentialElements = recalculationsList
                .Where(x => x.TotalCardsInfo.TransferredResources.TryGetValue(element.Item1, out var v) &&
                            v == element.Item2).ToList();

            var count = potentialElements.Count;
            var config = bonusConfig.FirstOrDefault(x => x.resourceType == element.Item1);
            var potentialBonus = config?.bonus ?? 0;
            
            foreach (var recalculation in potentialElements)
            {
                var bonus = potentialBonus / count;
                recalculation.totalBonus += bonus;
                recalculation.QueenBonus = new QueenBonus()
                {
                    bonus = bonus,
                    resourceType = element.Item1,
                };
            }
        }



        List<(GameResourceType, int)> CalculateBonuses(List<Recalculations> recalculationsList)
        {
            // Создать список для хранения результатов
            List<(List<Recalculations>, GameResourceType)> results = new();

            Dictionary<GameResourceType, int> maxResources = new Dictionary<GameResourceType, int>();

            foreach (var recalculations in recalculationsList)
            {
                // Перебрать каждую запись в TransferredResources
                foreach (var resourcePair in recalculations.TotalCardsInfo.TransferredResources)
                {
                    if (maxResources.ContainsKey(resourcePair.Key))
                    {
                        maxResources[resourcePair.Key] = Mathf.Max(maxResources[resourcePair.Key], resourcePair.Value);
                    }
                    else
                    {
                        maxResources[resourcePair.Key] = resourcePair.Value;
                    }
                }
            }

            var sortedResources = maxResources.OrderByDescending(pair => pair.Value);
            return sortedResources.Select(x => (x.Key, x.Value)).ToList();
        }


        Recalculations CalculatePlayer(PlayerEntity player)
        {
            var calculations = new Recalculations();
            calculations.PlayerEntityId = player.playerId.Value;
            calculations.Place = 0;
            calculations.TotalCardsInfo = new TotalCardsInfo()
            {
                allowedCardsCount = player.transferredResources.Value.AllowedResources.Select(x => x.Value).Sum(),
                deniedCardsCount = player.transferredResources.Value.NotAllowedResources.Select(x => x.Value).Sum(),

                allowedCardsCost = player.transferredResources.Value.AllowedResources
                    .Select(x => _cardConfigProvider.Get(x.Key).Cost * x.Value).Sum(),
                deniedCardsCost = player.transferredResources.Value.NotAllowedResources
                    .Select(x => _cardConfigProvider.Get(x.Key).Cost * x.Value).Sum()
            };

            Merge(calculations.TotalCardsInfo.TransferredResources, player
                .transferredResources
                .Value
                .AllowedResources);
            Merge(calculations.TotalCardsInfo.TransferredResources, player
                .transferredResources
                .Value
                .NotAllowedResources);
            
            return calculations;
        }

        void Merge(Dictionary<GameResourceType, int> target, Dictionary<GameResourceType, int> source)
        {
            foreach (var s in source)
                target[s.Key] = target.TryGetValue(s.Key, out var c) ? c + s.Value : s.Value;
        }
    }
}