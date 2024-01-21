using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.Rules;
using Sheriff.Rules.ClassicRules;
using ThirdParty.Randoms;
using UnityEngine;

namespace Sheriff.GameFlow
{

    public class ActionParam
    {
        
    }

    public class EmulateActionParams
    {
        
    }

    public abstract class BaseAction<T1, T2> 
        where T1 : ActionParam 
        where T2 : EmulateActionParams
    {
        public abstract void Emulate(T2 param);
        public abstract T2 Simulate(T1 param);
    }



    [Serializable]
    public class GetCardsFromDeckParams : ActionParam
    {
        public PlayerEntityId playerEntityId;
        public int cardsCount;
    }

    [Serializable]
    public class GetCardsFromDeckEmulateParams : EmulateActionParams
    {
        public PlayerEntityId playerDestination;
        public List<CardEntityId> cardIds;
    }
    
    
    public class GetCardsFromDeckAction : BaseAction<GetCardsFromDeckParams, GetCardsFromDeckEmulateParams>
    {
        private readonly IRandomService _randomService;
        private readonly IGroup<CardEntity> _cardsInDec;
        private readonly EcsContextProvider _ecsContextProvider;

        public GetCardsFromDeckAction(
            EcsContextProvider ecsContextProvider,
            IRandomService randomService)
        {
            _randomService = randomService;
            _cardsInDec = ecsContextProvider.Context.card.GetGroup(CardMatcher.InDec);
            _ecsContextProvider = ecsContextProvider;
        }
        
        public override void Emulate(GetCardsFromDeckEmulateParams emulateParams)
        {
            foreach (var cardId in emulateParams.cardIds)
            {
                var entity = _ecsContextProvider.Context.card.GetEntityWithCardId(cardId);
                entity.ReplaceCardOwner(emulateParams.playerDestination);
                entity.isInDec = false;
            }
        }

        public override GetCardsFromDeckEmulateParams Simulate(GetCardsFromDeckParams param)
        {
            var cardsCount = param.cardsCount;
            var entities = _cardsInDec.GetEntities();

            var actualCardsCount = Mathf.Min(cardsCount, entities.Length);
            var indexes = Enumerable.Range(0, entities.Length).ToList();
            indexes.Shuffle(_randomService);

            var result = new GetCardsFromDeckEmulateParams()
            {
                playerDestination = param.playerEntityId,
                cardIds = indexes.Take(actualCardsCount).Select(index => entities[indexes[index]].cardId.Value).ToList()
            };
            return result;
        }
    }
}