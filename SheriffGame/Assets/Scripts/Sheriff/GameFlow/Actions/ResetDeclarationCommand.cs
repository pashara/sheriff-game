using System;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{

    public class ResetDeclarationCommand : GameCommand<ResetDeclarationCommand.Params, ResetDeclarationCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                playerEntityId = @params.playerEntityId;
            }
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override ResetDeclarationCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new EmulateParams()
            {
                playerEntityId = param.playerEntityId,
            };

            return this;
        }


        public override void Apply()
        {
            var entity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerEntityId);
            if (entity == null)
                return;
            
            if (entity.hasDeclareResourcesByPlayer)
                entity.RemoveDeclareResourcesByPlayer();
        }
    }
}