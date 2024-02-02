using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    
    public class PopCardFromBagCommand : GameCommand<PopCardFromBagCommand.Params, PopCardFromBagCommand>
    {
        
        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public CardEntityId cardEntityId;
        }
        
        private readonly EcsContextProvider _ecsContextProvider;

        [Serializable]
        public class PopCardFromBagEmulateParam : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
            
            [JsonProperty("card_id")]
            public CardEntityId cardEntityId;
        }
        
        [JsonProperty("result")]
        private PopCardFromBagEmulateParam _result;

        public PopCardFromBagCommand(EcsContextProvider ecsContextProvider)
        {
            _ecsContextProvider = ecsContextProvider;
        }

        public override PopCardFromBagCommand Calculate(Params param)
        {
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
            if (playerEntity is not { hasSelectedCards: true }) return;

            if (playerEntity.selectedCards.Value.Remove(_result.cardEntityId))
                playerEntity.ReplaceSelectedCards(playerEntity.selectedCards.Value);
        }

    }
}