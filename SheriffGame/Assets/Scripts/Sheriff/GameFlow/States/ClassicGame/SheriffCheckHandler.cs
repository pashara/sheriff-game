using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.DataBase;
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
        
        [JsonProperty("bad_cards")]
        public List<CardEntityId> BadCards;
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

        [JsonProperty("bad_cards")]
        public List<CardEntityId> BadCards;
    }
    
    
    public class SheriffCheckHandler
    {
        private readonly ICardConfigProvider _cardConfigProvider;
        private readonly Contexts _context;

        public SheriffCheckHandler(
            EcsContextProvider ecsContextProvider,
            ICardConfigProvider cardConfigProvider
        )
        {
            _cardConfigProvider = cardConfigProvider;
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

            var cards = selectedCards
                .Select(x => _context.card.GetEntityWithCardId(x)).ToList();
            var actualCards = cards
                .Select(x => (x.resourceType.Value, x))
                .GroupBy(x => x.Value)
                .Select(x => new ProductDeclaration(x.Count(), x.Key));


            return Check(sheriffId, dealerId, cards, declaredElements, actualCards);

        }

        private SherifCheckResult Check(PlayerEntityId sheriffId,
            PlayerEntityId dealerId,
            List<CardEntity> cards,
            IEnumerable<ProductDeclaration> dealerDeclare,
            IEnumerable<ProductDeclaration> expectedDeclare)
        {
            var declaredElements = dealerDeclare
                .GroupBy(x => x.ResourceType)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.ProductsCount));
            var actualCards = expectedDeclare
                .GroupBy(x => x.ResourceType)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.ProductsCount));
                // .ToDictionary(x => x.Key, x => x.Count());

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

            // // check by cards
            // foreach (var actualElement in actualCards)
            // {
            //     //Ignore duplicates
            //     if (badElementsCount.ContainsKey(actualElement.Key)) continue;
            //     
            //     declaredElements.TryGetValue(actualElement.Key, out var v);
            //     if (v == actualElement.Value) continue;
            //     var delta = Mathf.Abs(v - actualElement.Value);
            //     if (delta == 0) continue;
            //     
            //     badElementsCount[actualElement.Key] = delta;
            // }

            if (badElementsCount.Count == 0)
            {
                var elements = new List<GameResourceType>();
                foreach (var element in declaredElements)
                    for (var i = 0; i < element.Value; i++)
                        elements.Add(element.Key);
                
                return CalculateSherifLoose(sheriffId, dealerId, elements);
            }
            else
            {
                HashSet<CardEntity> cardsToRemove = new();
                foreach (var badElementInfo in badElementsCount)
                {
                    for (var i = 0; i < badElementInfo.Value; i++)
                    {
                        var element = cards.FirstOrDefault(x =>
                            !cardsToRemove.Contains(x)
                            && x.resourceType.Value == badElementInfo.Key);

                        if (element == null) continue;
                        
                        cardsToRemove.Add(element);
                    }
                }
                
                return CalculateDealerLoose(sheriffId, dealerId, cardsToRemove);
            }
        }

        private SherifLooseCheckResult CalculateSherifLoose(
            PlayerEntityId sheriffId, 
            PlayerEntityId dealerId,
            List<GameResourceType> gameResourceTypes)
        {
            int total = 0;
            foreach (var element in gameResourceTypes)
            {
                total += _cardConfigProvider.Get(element).Fine;
            }
            return new SherifLooseCheckResult()
            {
                Coins = total,
                FromPlayerId = sheriffId.EntityID,
                ToPlayerId = dealerId.EntityID
            };
        }

        private DealerLooseCheckResult CalculateDealerLoose(
            PlayerEntityId sheriffId, 
            PlayerEntityId dealerId,
            HashSet<CardEntity> badCards)
        {
            int total = 0;

            foreach (var badCard in badCards)
            {
                total += _cardConfigProvider.Get(badCard.resourceType.Value).Fine;
            }
            
            return new DealerLooseCheckResult()
            {
                Coins = total,
                FromPlayerId = dealerId.EntityID,
                BadCards = badCards.Select(x => x.cardId.Value).ToList(),
                ToPlayerId = sheriffId.EntityID
            };
        }
    }
}