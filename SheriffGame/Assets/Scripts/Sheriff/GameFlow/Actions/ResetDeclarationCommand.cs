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

    public class ResetDeclarationCommand : GameCommand<ResetDeclarationCommand.Params, ResetDeclarationCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerEntityId;
        }
        
        private readonly EcsContextProvider _ecsContextProvider;

        public ResetDeclarationCommand(
            EcsContextProvider ecsContextProvider) 
        {
            _ecsContextProvider = ecsContextProvider;
        }

        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override ResetDeclarationCommand Calculate(Params param)
        {
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