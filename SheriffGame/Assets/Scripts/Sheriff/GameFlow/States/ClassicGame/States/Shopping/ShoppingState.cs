using System;
using System.Collections.Generic;
using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.Shopping
{
    public class ShoppingState : ClassicGameState
    {
        private readonly DiContainer _container;

        public ShoppingState(
            IterationEnvironment iterationEnvironment,
            EcsContextProvider ecsContextProvider
            )
        {
            foreach (var playerEntity in ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.isDealer)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(DeclareCommand),
                        typeof(GetCardsFromDeckCommand),
                        typeof(PopCardFromBagCommand),
                        typeof(PutCardInBagCommand),
                    });
                    playerEntity.ReplaceAllowedActions(allowedActions);
                }
            }
            
        }
        
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}