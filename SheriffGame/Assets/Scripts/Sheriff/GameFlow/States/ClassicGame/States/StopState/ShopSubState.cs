using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.StopState
{
    public class ShopSubState : ClassicGameSubState<ShopState>
    {
        public ShopSubState(DiContainer container, IterationEnvironmentFactory environmentFactory) : base(container, environmentFactory)
        {
        }
    }
}