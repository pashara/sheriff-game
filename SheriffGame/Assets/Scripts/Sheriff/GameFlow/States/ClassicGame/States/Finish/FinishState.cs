using Sheriff.GameFlow.ResultUIControl;

namespace Sheriff.GameFlow.States.ClassicGame.States.Finish
{
    public class FinishGameState : ClassicGameState
    {
        private readonly ResultUIController _resultUIController;

        public FinishGameState(
            ResultUIController resultUIController)
        {
            _resultUIController = resultUIController;
        }
        
        public override string Title => "Финал";
        
        public async override void Enter()
        {
            _resultUIController.Open();
        }




        public override void Exit()
        {
        }
    }
}