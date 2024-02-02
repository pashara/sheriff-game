using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.Shopping
{
    public class ShoppingSubState : ClassicGameSubState<ShoppingState>
    {
        private readonly EcsContextProvider _ecsContextProvider;

        public ShoppingSubState(
            DiContainer container, 
            IterationEnvironmentFactory environmentFactory
            ) : base(container, environmentFactory)
        {
        }
    }
}