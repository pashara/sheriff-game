using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{

    public class AffectGoldCommand : GameCommand<AffectGoldCommand.Params, AffectGoldCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public int additiveGoldValue;

            public Params()
            {
            }
            
            public Params(Params @params)
            {
                playerEntityId = @params.playerEntityId;
                additiveGoldValue = @params.additiveGoldValue;
            }
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
            [JsonProperty("count")]
            public int additiveGoldValue;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override AffectGoldCommand Calculate(Params param)
        {
            _params = new Params(param);
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