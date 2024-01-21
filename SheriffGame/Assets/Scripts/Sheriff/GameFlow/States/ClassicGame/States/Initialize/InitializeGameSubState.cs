using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.Initialize
{
    public class InitializeGameSubState : ClassicGameSubState<InitializeGameState>
    {
        public InitializeGameSubState(
            DiContainer container, 
            IterationEnvironmentFactory environmentFactory
            ) : base(
            container, environmentFactory)
        {
        }
    }
}