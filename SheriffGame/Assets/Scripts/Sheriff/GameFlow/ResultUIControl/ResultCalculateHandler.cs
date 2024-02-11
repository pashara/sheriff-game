using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
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
        public List<Recalculations> Calculate()
        {
            var calculations = _ecsContextProvider.Context.player.GetEntities().Select(CalculatePlayer).ToList();
            ApplyAllBonuses(calculations);
            return calculations;
        }

        void ApplyAllBonuses(List<Recalculations> calculations)
        {
            calculations.ForEach(x =>
            {
                x.totalBonus = x.TotalCardsInfo.allowedCardsCost + x.TotalCardsInfo.deniedCardsCost;
            });
            
            
            // var bonuses = CalculateBonuses(calculations);
            // var kingBonus = bonuses.FirstOrDefault();
            // var queenBonus = bonuses.LastOrDefault();
            // ApplyKingBonus(kingBonus.Recalculation, kingBonus.ResourceType, kingBonus.Amount);
            // ApplyQueenBonus(queenBonus.Recalculation, queenBonus.ResourceType, queenBonus.Amount);
            
            calculations.Sort((a, b) => b.totalBonus - a.totalBonus);

            for (var i = 0; i < calculations.Count; i++)
            {
                calculations[i].Place = i + 1;
            }
        }

        private void ApplyKingBonus(Recalculations source, GameResourceType resourceType, int amount)
        {
            source.KingBonus = new KingBonus()
            {
                bonus = 0,
                resourceType = resourceType
            };
        }


        private void ApplyQueenBonus(Recalculations source, GameResourceType resourceType, int amount)
        {
            source.QueenBonus = new QueenBonus()
            {
                bonus = 0,
                resourceType = resourceType
            };
        }



//         List<(GameResourceType ResourceType, int Amount, Recalculations Recalculation)> CalculateBonuses(List<Recalculations> recalculationsList)
//         {
//             HashSet<GameResourceType> gameResourceTypes = new()
//             {
//                 GameResourceType.Apple,
//                 GameResourceType.Bread,
//                 GameResourceType.Chicken,
//                 GameResourceType.Cheese,
//             };
//             // Создать список для хранения результатов
//             List<(GameResourceType ResourceType, int Amount, Recalculations Recalculation)> results =
//                 new List<(GameResourceType, int, Recalculations)>();
//
// // Создать словарь для хранения общего количества ресурсов для каждого типа
//             Dictionary<GameResourceType, int> totalResources = new Dictionary<GameResourceType, int>();
//
// // Пройти по каждому элементу в списке Recalculations
//             foreach (var recalculations in recalculationsList)
//             {
//                 // Перебрать каждую запись в TransferredResources
//                 foreach (var resourcePair in recalculations.TotalCardsInfo.TransferredResources)
//                 {
//                     if (!gameResourceTypes.Contains(resourcePair.Key))
//                         continue;
//                     // Если ресурс уже есть в словаре, добавить к его значению
//                     if (totalResources.ContainsKey(resourcePair.Key))
//                     {
//                         totalResources[resourcePair.Key] += resourcePair.Value;
//                     }
//                     // В противном случае, добавить ресурс в словарь
//                     else
//                     {
//                         totalResources.Add(resourcePair.Key, resourcePair.Value);
//                     }
//                 }
//             }
//
// // Отсортировать словарь по убыванию значения (количества ресурсов)
//             var sortedResources = totalResources.OrderByDescending(pair => pair.Value);
//
// // Взять первые две записи из отсортированного словаря
//             var firstPlace = sortedResources.ElementAt(0);
//             var secondPlace = sortedResources.ElementAt(1);
//
// // Найти элемент Recalculations для первого места
//             var firstPlaceRecalculation = recalculationsList.FirstOrDefault(rec =>
//                 rec.TotalCardsInfo.TransferredResources.ContainsKey(firstPlace.Key));
//
// // Найти элемент Recalculations для второго места
//             var secondPlaceRecalculation = recalculationsList.FirstOrDefault(rec =>
//                 rec.TotalCardsInfo.TransferredResources.ContainsKey(secondPlace.Key));
//
// // Добавить результаты в список
//             results.Add((firstPlace.Key, firstPlace.Value, firstPlaceRecalculation));
//             results.Add((secondPlace.Key, secondPlace.Value, secondPlaceRecalculation));
//
// // Теперь в results содержится информация о первом и втором месте по продукту GameResourceType,
// // его количеству и соответствующих элементах Recalculations.
//         }


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