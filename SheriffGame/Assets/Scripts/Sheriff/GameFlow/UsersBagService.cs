using System.Collections.Generic;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{

    public class PlayerBagInfo
    {
        public List<CardEntityId> CardIds = new();
    }
    
    public interface IUsersBagService
    {
        IReadOnlyDictionary<PlayerEntityId, PlayerBagInfo> BagInfo { get; }


        PlayerBagInfo this[PlayerEntityId playerEntityId]
        {
            get;
        }

        void ResetBags();
    }
    
    public class UsersBagService : IUsersBagService, IStateApplyable
    {
        private readonly Dictionary<PlayerEntityId, PlayerBagInfo> _bagInfo = new();
        
        public IReadOnlyDictionary<PlayerEntityId, PlayerBagInfo> BagInfo => _bagInfo;

        public PlayerBagInfo this[PlayerEntityId playerEntityId]
        {
            get
            {
                _bagInfo.TryAdd(playerEntityId, new PlayerBagInfo());
                return _bagInfo[playerEntityId];
            }
        }
        
        public UsersBagService()
        {
            
        }


        public void ResetBags()
        {
            _bagInfo.Clear();
        }


        public void Apply()
        {
            
        }
    }
}