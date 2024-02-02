using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{
    
    
    public class SelectSheriffCommand : GameCommand<SelectSheriffCommand.Params, SelectSheriffCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public int round;
            public List<PlayerEntityId> playersQueue;
        }
        [Serializable]
        public class SelectSheriffEmulateParam : EmulateActionParams
        {
            [JsonProperty("sherif_id")]
            public PlayerEntityId sheriffId;
            [JsonProperty("dealer_ids")]
            public List<PlayerEntityId> dealers;
        }
        
        [Inject] private readonly ISherifSelectService _sherifSelectService;
        
        [JsonProperty("result")]
        private SelectSheriffEmulateParam _result;
        
        public override void Apply()
        {
            _sherifSelectService.SelectSherif(_result.sheriffId);
        }

        public override SelectSheriffCommand Calculate(Params param)
        {
            var actualRound = param.round;
            var neededElement = param.playersQueue[actualRound % param.playersQueue.Count];
            
            _result =  new SelectSheriffEmulateParam()
            {
                sheriffId = neededElement,
                dealers = param.playersQueue.Where(x => x != neededElement).ToList(),
            };

            return this;
        }
    }
}