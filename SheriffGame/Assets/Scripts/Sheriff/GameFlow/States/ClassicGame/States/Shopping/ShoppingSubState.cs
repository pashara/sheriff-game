using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.Shopping
{
    public class ShoppingSubState : ClassicGameSubState<ShoppingState>
    {
        public ShoppingSubState(DiContainer container, IterationEnvironmentFactory environmentFactory) : base(container, environmentFactory)
        {
        }
    }
}