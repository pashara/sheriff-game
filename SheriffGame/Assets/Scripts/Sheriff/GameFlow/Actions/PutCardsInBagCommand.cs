using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{
    
    public class PutCardsInBagCommand : GameCommand<PutCardsInBagCommand.Params, PutCardsInBagCommand>
    {
        [Serializable]
        public class Params : ActionParam
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
            [JsonProperty("card_ids")]
            public List<CardEntityId> cardEntityIds;
        }
        
        [Serializable]
        public class PutCardInBagEmulateParam : EmulateActionParams
        {
            public PlayerEntityId playerEntityId;
            public List<CardEntityId> cardEntityIds;
        }
        
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;
        
        [JsonProperty("result")]
        private PutCardInBagEmulateParam _result;

        public PutCardsInBagCommand(EcsContextProvider ecsContextProvider)
        {
            _ecsContextProvider = ecsContextProvider;
        }
        
        public override PutCardsInBagCommand Calculate(Params param)
        {
            _result = new PutCardInBagEmulateParam()
            {
                playerEntityId = param.playerEntityId,
                cardEntityIds = param.cardEntityIds.ToList()
            };

            return this;
        }

        public override void Apply()
        {
            var playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
            if (playerEntity == null) return;
            
            if (!playerEntity.hasSelectedCards)
                playerEntity.AddSelectedCards(new List<CardEntityId>());

            foreach (var cardEntityId in _result.cardEntityIds)
            {
                if (playerEntity.selectedCards.Value.Contains(cardEntityId)) return;

                playerEntity.selectedCards.Value.Add(cardEntityId);

                _ecsContextProvider.Context.card.GetEntityWithCardId(cardEntityId).MarkSelected();
            }
            playerEntity.ReplaceSelectedCards(playerEntity.selectedCards.Value);
        }
    }
}