using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck
{
    public class SherifCheckSubState : ClassicGameSubState<SheriffCheckState>
    {
        public SherifCheckSubState(DiContainer container, IterationEnvironmentFactory environmentFactory) : base(
            container, environmentFactory)
        {
        }
    }
}