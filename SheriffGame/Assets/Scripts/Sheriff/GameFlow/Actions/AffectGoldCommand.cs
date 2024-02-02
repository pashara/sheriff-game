using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Sirenix.OdinInspector;

namespace Sheriff.GameFlow
{

    public class AffectGoldCommand : GameCommand<AffectGoldCommand.Params, AffectGoldCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public int additiveGoldValue;
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
            [JsonProperty("count")]
            public int additiveGoldValue;
        }
        
        private readonly EcsContextProvider _ecsContextProvider;

        public AffectGoldCommand(
            EcsContextProvider ecsContextProvider) 
        {
            _ecsContextProvider = ecsContextProvider;
        }

        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override AffectGoldCommand Calculate(Params param)
        {
            _result = new EmulateParams()
            {
                playerEntityId = param.playerEntityId,
                additiveGoldValue = param.additiveGoldValue,
            };

            return this;
        }


        public override void Apply()
        {
            var entity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
            if (entity == null)
                return;
            
            if (!entity.hasGoldCashCurrency)
                return;

            var newValue = entity.goldCashCurrency.Value + _result.additiveGoldValue;
            entity.ReplaceGoldCashCurrency(newValue);
        }
    }
}