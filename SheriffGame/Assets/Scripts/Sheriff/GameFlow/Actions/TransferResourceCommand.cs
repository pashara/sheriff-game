using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Zenject;

namespace Sheriff.GameFlow
{
    public class TransferResourceCommand : GameCommand<TransferResourceCommand.Params, TransferResourceCommand>
    {
        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId source;
            public PlayerEntityId destination;
            public GameResourceType gameResource;
            public GameResourceCategory category;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                source = @params.source;
                destination = @params.destination;
                gameResource = @params.gameResource;
                category = @params.category;
            }
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("from_player_id")]
            public PlayerEntityId source;
            [JsonProperty("to_player_id")]
            public PlayerEntityId destination;
            [JsonProperty("category")]
            public GameResourceCategory category;
            [JsonProperty("resource")]
            public GameResourceType gameResource;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override TransferResourceCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new EmulateParams()
            {
                source = param.source,
                destination = param.destination,
                gameResource = param.gameResource,
            };

            return this;
        }


        public override void Apply()
        {
            var sourceEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.source);
            var destinationEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.destination);
            if (sourceEntity == null || destinationEntity == null)
                return;
            
            if (!sourceEntity.hasTransferredResources)
                return;
            
            if (sourceEntity.transferredResources.Value == null)
                return;
            

            if (!sourceEntity.transferredResources.Value.TryGetValue(_result.gameResource, out var count))
                return;

            if (count <= 0)
                return;

            TransferredObjects destinationTransfer = null;
            TransferredObjects sourceTransfer = sourceEntity.transferredResources.Value;
            destinationTransfer = destinationEntity.hasTransferredResources
                ? destinationEntity.transferredResources.Value
                : new TransferredObjects();
            
            sourceTransfer.Dec(_result.category, _result.gameResource);
            destinationTransfer.Inc(_result.category, _result.gameResource);

            sourceEntity.ReplaceTransferredResources(sourceTransfer);
            destinationEntity.ReplaceTransferredResources(destinationTransfer);
        }
    }
}