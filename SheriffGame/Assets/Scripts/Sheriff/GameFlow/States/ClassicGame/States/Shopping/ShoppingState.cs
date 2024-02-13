using System;
using System.Collections.Generic;
using Sheriff.ECS;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.Shopping
{
    public class ShoppingState : ClassicGameState
    {
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly DiContainer _container;

        public ShoppingState(
            EcsContextProvider ecsContextProvider
            )
        {
            _ecsContextProvider = ecsContextProvider;
        }
        
        public override string Title => "Стадия Торгов";
        
        public override void Enter()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.isDealer)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(DeclareCommand),
                        typeof(GetCardsFromDeckCommand),
                        typeof(PutCardsInBagCommand),
                        // typeof(PopCardFromBagCommand),
                        // typeof(PutCardInBagCommand),
                    });
                    playerEntity.ReplaceAllowedActions(allowedActions);
                }
            }
        }

        public override void Exit()
        {
        }
    }
}