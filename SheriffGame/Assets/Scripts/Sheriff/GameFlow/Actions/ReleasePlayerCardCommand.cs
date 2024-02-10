using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{
    
    public class ReleasePlayerCardCommand : GameCommand<ReleasePlayerCardCommand.Params, ReleasePlayerCardCommand>
    {
        
        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public CardEntityId cardEntityId;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                playerEntityId = @params.playerEntityId;
                cardEntityId = @params.cardEntityId;
            }
        }

        [Serializable]
        public class PopCardFromBagEmulateParam : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
            
            [JsonProperty("card_id")]
            public CardEntityId cardEntityId;
        }
        
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;
        
        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private PopCardFromBagEmulateParam _result;

        public override ReleasePlayerCardCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new PopCardFromBagEmulateParam()
            {
                playerEntityId = param.playerEntityId,
                cardEntityId = param.cardEntityId
            };
            return this;
        }

        public override void Apply()
        {
            var playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
            var cardEntity = _ecsContextProvider.Context.card.GetEntityWithCardId(_result.cardEntityId);

            cardEntity.MarkReleased();
            
            if (playerEntity.hasSelectedCards)
            {
                var a = playerEntity.selectedCards.Value;
                if (a.Remove(cardEntity.cardId.Value))
                    playerEntity.ReplaceSelectedCards(a);
            }

            List<CardEntityId> droppedCards = new();
            if (playerEntity.hasDropCards)
            {
                droppedCards = playerEntity.dropCards.Value ?? new List<CardEntityId>();
            }

            if (!droppedCards.Contains(cardEntity.cardId.Value))
            {
                droppedCards.Add(cardEntity.cardId.Value);
                playerEntity.ReplaceDropCards(droppedCards);
            }
        }

    }
}