using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States
{
    public class GiveNeededCardsBeforeShopState : ClassicGameState
    {
        public GiveNeededCardsBeforeShopState() 
        {
        }

        private void Prepare(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<CardsOnHandService>().AsSingle();
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
        }
    }
}