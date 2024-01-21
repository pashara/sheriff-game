using System;
using System.Collections.Generic;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    [Serializable]
    public class PopCardFromBagActionParam : ActionParam
    {
        public PlayerEntityId playerEntityId;
        public CardEntityId cardEntityId;
    }

    [Serializable]
    public class PopCardFromBagEmulateParam : EmulateActionParams
    {
        public PlayerEntityId playerEntityId;
        public CardEntityId cardEntityId;
    }
    
    public class PopCardFromBagAction : BaseAction<PopCardFromBagActionParam, PopCardFromBagEmulateParam>
    {
        private readonly IUsersBagService _bagService;

        public PopCardFromBagAction(IUsersBagService bagService)
        {
            _bagService = bagService;
        }
        
        public override void Emulate(PopCardFromBagEmulateParam param)
        {
            _bagService[param.playerEntityId].CardIds.Remove(param.cardEntityId);
        }

        public override PopCardFromBagEmulateParam Simulate(PopCardFromBagActionParam param)
        {
            return new PopCardFromBagEmulateParam()
            {
                playerEntityId = param.playerEntityId,
                cardEntityId = param.cardEntityId
            };
        }
    }
}