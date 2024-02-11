using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{
    public class TransferMoneyCommand : GameCommand<TransferMoneyCommand.Params, TransferMoneyCommand>
    {
        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId source;
            public PlayerEntityId destination;
            public int count;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                source = @params.source;
                destination = @params.destination;
                count = @params.count;
            }
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("from_player_id")]
            public PlayerEntityId source;
            [JsonProperty("to_player_id")]
            public PlayerEntityId destination;
            [JsonProperty("amount")]
            public int amount;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override TransferMoneyCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new EmulateParams()
            {
                source = param.source,
                destination = param.destination,
                amount = param.count,
            };

            return this;
        }


        public override void Apply()
        {
            var sourceEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.source);
            var destinationEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.destination);
            if (sourceEntity == null || destinationEntity == null)
                return;
            
            if (!sourceEntity.hasGoldCashCurrency)
                return;
            
            if (sourceEntity.goldCashCurrency.Value < _result.amount)
                return;
            
            sourceEntity.ReplaceGoldCashCurrency(sourceEntity.goldCashCurrency.Value - _result.amount);

            var destinationValue = 0;
            if (destinationEntity.hasGoldCashCurrency)
            {
                destinationValue = destinationEntity.goldCashCurrency.Value;
            }

            destinationEntity.ReplaceGoldCashCurrency(destinationValue + _result.amount);
        }
    }
}