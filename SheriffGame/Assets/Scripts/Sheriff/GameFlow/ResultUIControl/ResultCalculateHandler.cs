using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow.ResultUIControl
{
    public class ResultCalculateHandler
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        public List<PlayerEntityId> Calculate()
        {
            return _ecsContextProvider.Context.player.GetEntities().Select(x => x.playerId.Value).ToList();
        }
    }
}