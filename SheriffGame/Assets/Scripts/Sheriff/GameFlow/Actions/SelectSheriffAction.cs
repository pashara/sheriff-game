using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    
    [Serializable]
    public class SelectSheriffActionParam : ActionParam
    {
        public int round;
        public List<PlayerEntityId> playersQueue;
    }

    [Serializable]
    public class SelectSheriffEmulateParam : EmulateActionParams
    {
        public PlayerEntityId sheriffId;
        public List<PlayerEntityId> dealers;
    }
    
    public class SelectSheriffAction : BaseAction<SelectSheriffActionParam, SelectSheriffEmulateParam>
    {
        private readonly ISherifSelectService _sherifSelectService;

        public SelectSheriffAction(ISherifSelectService sherifSelectService)
        {
            _sherifSelectService = sherifSelectService;
        }
        
        public override void Emulate(SelectSheriffEmulateParam param)
        {
            _sherifSelectService.SelectSherif(param.sheriffId);
        }

        public override SelectSheriffEmulateParam Simulate(SelectSheriffActionParam param)
        {
            var actualRound = param.round;
            var neededElement = param.playersQueue[actualRound % param.playersQueue.Count];


            return new SelectSheriffEmulateParam()
            {
                sheriffId = neededElement,
                dealers = param.playersQueue.Where(x => x != neededElement).ToList(),
            };
        }
    }
}