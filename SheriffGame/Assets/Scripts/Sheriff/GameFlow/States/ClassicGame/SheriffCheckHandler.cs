using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame
{
    [Serializable]
    public abstract class SherifCheckResult
    {
    }


    [Serializable]
    public class SkipCheckSherifResult : SherifCheckResult
    {
        [JsonProperty("sheriff_id")]
        public long SheriffId;
        
        [JsonProperty("player_id")]
        public long DealerId;
    }

    [Serializable]
    public class SherifLooseCheckResult : SherifCheckResult
    {
        [JsonProperty("coins")]
        public int Coins;

        [JsonProperty("from_player_id")]
        public long FromPlayerId;
        [JsonProperty("to_player_id")]
        public long ToPlayerId;
    }

    [Serializable]
    public class DealerLooseCheckResult : SherifCheckResult
    {
        [JsonProperty("coins")]
        public int Coins;

        [JsonProperty("from_player_id")]
        public long FromPlayerId;
        [JsonProperty("to_player_id")]
        public long ToPlayerId;
    }
    
    
    public class SheriffCheckHandler
    {
        private readonly Contexts _context;

        public SheriffCheckHandler(EcsContextProvider ecsContextProvider)
        {
            _context = ecsContextProvider.Context;
        }
        
        public SherifCheckResult Skip(PlayerEntityId sheriffId, PlayerEntityId dealerId)
        {
            return new SkipCheckSherifResult()
            {
                SheriffId = sheriffId.EntityID,
                DealerId = dealerId.EntityID
            };
        }
        
        public SherifCheckResult Check(PlayerEntityId sheriffId, PlayerEntityId dealerId)
        {
            var sheriff = _context.player.GetEntityWithPlayerId(sheriffId);
            var dealer = _context.player.GetEntityWithPlayerId(dealerId);
            
            if (sheriff == null || dealer == null)
                return Skip(sheriffId, dealerId);

            if (!dealer.hasDeclareResourcesByPlayer || !dealer.hasSelectedCards)
                return Skip(sheriffId, dealerId);
            
            if (!dealer.isReadyForCheck)
                return Skip(sheriffId, dealerId);
            

            var declare = dealer.declareResourcesByPlayer.Value;
            var selectedCards = dealer.selectedCards.Value;

            if (declare?.Declarations == null || declare.Declarations.Count == 0 || selectedCards == null || selectedCards.Count == 0)
                return Skip(sheriffId, dealerId);


            var declaredElements = declare.Declarations;

            var actualCards = selectedCards
                .Select(x => _context.card.GetEntityWithCardId(x))
                .Select(x => (x.resourceType.Value, x))
                .GroupBy(x => x.Value)
                .Select(x => new ProductDeclaration(x.Count(), x.Key));


            return Check(sheriffId, dealerId, declaredElements, actualCards);

        }

        private SherifCheckResult Check(
            PlayerEntityId sheriffId, 
            PlayerEntityId dealerId,
            IEnumerable<ProductDeclaration> dealerDeclare, 
            IEnumerable<ProductDeclaration> expectedDeclare)
        {
            var declaredElements = dealerDeclare
                .GroupBy(x => x.ResourceType)
                .ToDictionary(x => x.Key, x => x.Count());
            var actualCards = expectedDeclare
                .GroupBy(x => x.ResourceType)
                .ToDictionary(x => x.Key, x => x.Count());

            Dictionary<GameResourceType, int> badElementsCount = new();

            
            //Check by declared list
            foreach (var declaredElement in declaredElements)
            {
                actualCards.TryGetValue(declaredElement.Key, out var v);
                if (v == declaredElement.Value) continue;
                var delta = Mathf.Abs(v - declaredElement.Value);
                if (delta == 0) continue;
                badElementsCount[declaredElement.Key] = delta;
            }

            // check by cards
            foreach (var actualElement in actualCards)
            {
                //Ignore duplicates
                if (badElementsCount.ContainsKey(actualElement.Key)) continue;
                
                declaredElements.TryGetValue(actualElement.Key, out var v);
                if (v == actualElement.Value) continue;
                var delta = Mathf.Abs(v - actualElement.Value);
                if (delta == 0) continue;
                
                badElementsCount[actualElement.Key] = delta;
            }

            if (badElementsCount.Count == 0)
            {
                return CalculateSherifLoose(sheriffId, dealerId, badElementsCount);
            }
            else
            {
                return CalculateDealerLoose(sheriffId, dealerId, badElementsCount);
            }
        }

        private SherifLooseCheckResult CalculateSherifLoose(
            PlayerEntityId sheriffId, 
            PlayerEntityId dealerId,
            Dictionary<GameResourceType, int> badElementsList)
        {
            return new SherifLooseCheckResult()
            {
                Coins = 50,
                FromPlayerId = sheriffId.EntityID,
                ToPlayerId = dealerId.EntityID
            };
        }

        private DealerLooseCheckResult CalculateDealerLoose(
            PlayerEntityId sheriffId, 
            PlayerEntityId dealerId,
            Dictionary<GameResourceType, int> badElementsList)
        {
            return new DealerLooseCheckResult()
            {
                Coins = 50,
                FromPlayerId = dealerId.EntityID,
                ToPlayerId = sheriffId.EntityID
            };
        }
    }
}