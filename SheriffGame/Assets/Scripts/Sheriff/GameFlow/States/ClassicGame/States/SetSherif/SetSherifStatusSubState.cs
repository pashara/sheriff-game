using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SetSherif
{
    public class SetSherifStatusSubState : ClassicGameSubState<SetSheriffStatusState>
    {
        public SetSherifStatusSubState(DiContainer container, IterationEnvironmentFactory environmentFactory) : base(
            container, environmentFactory)
        {
        }
    }
}