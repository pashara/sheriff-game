using System.Collections.Generic;
using Entitas;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Zenject;

namespace Sheriff.GameFlow
{

    public interface IStateApplyable
    {
        void Apply();
    }
    
    public interface ISherifSelectService
    {
        void SelectSherif(PlayerEntityId paramSheriffId);

        void Clear();
    }
    
    public class SherifSelectService : ISherifSelectService, IStateApplyable, IInitializable
    {
        private readonly EcsContextProvider _ecsContextProvider;
        private PlayerEntityId potentialSheriff = new PlayerEntityId();
        private readonly IGroup<PlayerEntity> _players;

        public SherifSelectService(EcsContextProvider ecsContextProvider)
        {
            _ecsContextProvider = ecsContextProvider;
            _players = _ecsContextProvider.Context.player.GetGroup(PlayerMatcher.PlayerId);
        }
        
        public void SelectSherif(PlayerEntityId paramSheriffId)
        {
            potentialSheriff = paramSheriffId;
        }
        

        public void Apply()
        {
            foreach (var entity in _players.GetEntities())
            {
                var isSheriff = entity.playerId.Value == potentialSheriff;
                entity.isSheriff = isSheriff;
                entity.isDealer = !isSheriff;
            }
        }

        public void Clear()
        {
        }

        public void Initialize()
        {
            
        }
    }
}