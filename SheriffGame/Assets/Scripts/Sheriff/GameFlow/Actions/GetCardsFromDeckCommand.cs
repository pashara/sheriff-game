using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
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
        }
        
        [Serializable]
        public class GetCardsFromDeckEmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerDestination;
            [JsonProperty("cards")]
            public List<CardEntityId> cardIds;
        }
        
        [Inject] private readonly IRandomService _randomService;
        [Inject] private readonly EcsContextProvider _ecsContextProvider;
        

        [JsonProperty("result")]
        private GetCardsFromDeckEmulateParams _result = null;

        public override GetCardsFromDeckCommand Calculate(Params param)
        {
            var cardsCount = param.cardsCount;
            var cardsInDec = _ecsContextProvider.Context.card.GetGroup(CardMatcher.InDec);
            var entities = cardsInDec.GetEntities();

            var actualCardsCount = Mathf.Min(cardsCount, entities.Length);
            var indexes = Enumerable.Range(0, entities.Length).ToList();
            indexes.Shuffle(_randomService);

            _result = new GetCardsFromDeckEmulateParams()
            {
                playerDestination = param.playerEntityId,
                cardIds = indexes.Take(actualCardsCount).Select(index => entities[indexes[index]].cardId.Value).ToList()
            };

            return this;
        }


        public override void Apply()
        {
            foreach (var cardId in _result.cardIds)
            {
                var entity = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                entity.ReplaceCardOwner(_result.playerDestination);
                entity.isInDec = false;
            }
        }
    }
}