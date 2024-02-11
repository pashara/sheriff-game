using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using ThirdParty.Randoms;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow
{
    public class GetCardsFromDeckCommand : GameCommand<GetCardsFromDeckCommand.Params, GetCardsFromDeckCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public int cardsCount;
            public bool ignoreLimits;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                playerEntityId = @params.playerEntityId;
                cardsCount = @params.cardsCount;
                ignoreLimits = @params.ignoreLimits;
            }
        }
        
        [Serializable]
        public class GetCardsFromDeckEmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerDestination;
            [JsonProperty("ignore_limits")]
            public bool ignoreLimits;
            [JsonProperty("cards")]
            public List<CardEntityId> cardIds;
        }
        
        [Inject] private readonly IRandomService _randomService;
        [Inject] private readonly EcsContextProvider _ecsContextProvider;
        

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private GetCardsFromDeckEmulateParams _result = null;

        public IReadOnlyList<CardEntityId> CalculatedCards => _result.cardIds;

        public override GetCardsFromDeckCommand Calculate(Params param)
        {
            _params = new Params(param);
            var cardsCount = param.cardsCount;
            var cardsInDec = _ecsContextProvider.Context.card.GetGroup(CardMatcher.InDec);
            var entities = cardsInDec.GetEntities();

            var actualCardsCount = Mathf.Min(cardsCount, entities.Length);
            var indexes = Enumerable.Range(0, entities.Length).ToList();
            indexes.Shuffle(_randomService);

            _result = new GetCardsFromDeckEmulateParams()
            {
                playerDestination = param.playerEntityId,
                cardIds = indexes.Take(actualCardsCount).Select(index => entities[indexes[index]].cardId.Value).ToList(),
                ignoreLimits = param.ignoreLimits
            };

            return this;
        }


        public override void Apply()
        {
            foreach (var cardId in _result.cardIds)
            {
                var entity = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                entity.MarkOnHand(_result.playerDestination);
            }

            if (!_result.ignoreLimits)
            {
                var player = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerDestination);
                var initialValue = 0;
                if (player.hasCardsPopPerStep)
                {
                    initialValue = player.cardsPopPerStep.Count;
                }

                initialValue += _result.cardIds.Count;
                player.ReplaceCardsPopPerStep(initialValue);
            }
        }
    }
}