using System;
using System.Collections.Generic;
using Sheriff.ECS.Components;

namespace Sheriff.GameFlow
{
    [Serializable]
    public class PutCardInBagActionParam : ActionParam
    {
        public PlayerEntityId playerEntityId;
        public CardEntityId cardEntityId;
    }

    [Serializable]
    public class PutCardInBagEmulateParam : EmulateActionParams
    {
        public PlayerEntityId playerEntityId;
        public CardEntityId cardEntityId;
    }
    
    public class PutCardInBagAction : BaseAction<PutCardInBagActionParam, PutCardInBagEmulateParam>
    {
        private readonly IUsersBagService _bagService;

        public PutCardInBagAction(IUsersBagService bagService)
        {
            _bagService = bagService;
        }
        
        public override void Emulate(PutCardInBagEmulateParam param)
        {
            _bagService[param.playerEntityId].CardIds.Add(param.cardEntityId);
        }

        public override PutCardInBagEmulateParam Simulate(PutCardInBagActionParam param)
        {
            return new PutCardInBagEmulateParam()
            {
                playerEntityId = param.playerEntityId,
                cardEntityId = param.cardEntityId
            };
        }
    }
}