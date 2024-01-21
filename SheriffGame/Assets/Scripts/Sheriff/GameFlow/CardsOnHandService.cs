using System.Collections.Generic;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    public class PlayerHandCardsInfo
    {
        public List<CardEntityId> cardsOnStartStep = new();
        public List<CardEntityId> actualStepCards = new();
    }
    
    public interface ICardsOnHandService
    {
        IReadOnlyDictionary<PlayerEntityId, PlayerHandCardsInfo> HandInfo { get; }


        PlayerHandCardsInfo this[PlayerEntityId playerEntityId]
        {
            get;
        }

        void ResetBags();
        
    }
    
    public class CardsOnHandService : ICardsOnHandService, IStateApplyable
    {
        private readonly Dictionary<PlayerEntityId, PlayerHandCardsInfo> _handInfo = new();
        
        public IReadOnlyDictionary<PlayerEntityId, PlayerHandCardsInfo> HandInfo => _handInfo;
        
        
        public PlayerHandCardsInfo this[PlayerEntityId playerEntityId]
        {
            get
            {
                _handInfo.TryAdd(playerEntityId, new PlayerHandCardsInfo());
                return _handInfo[playerEntityId];
            }
        }

        public void ResetBags()
        {
            _handInfo.Clear();
        }

        public void Apply()
        {
            
        }
    }
}