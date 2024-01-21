using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.StopState
{
    public class ShopState : ClassicGameState
    {
        private readonly DiContainer _container;

        public ShopState(
            IterationEnvironment iterationEnvironment,
            ActionHandleService actionHandleService
            )
        {
            iterationEnvironment.Container.BindInterfacesAndSelfTo<CardsOnHandService>().AsSingle();
            iterationEnvironment.Container.BindInterfacesAndSelfTo<UsersBagService>().AsSingle();
            iterationEnvironment.Container.BindInterfacesAndSelfTo<TestScript>().AsSingle().WithArguments(new object[] { new Ee("ShopState") });

            PutAllowedActions(actionHandleService, iterationEnvironment.Container);
        }
        
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
        
        static void PutAllowedActions(ActionHandleService handleService, IInstantiator container)
        {
            handleService.Put(container.Instantiate<GetCardsFromDeckAction>());
            handleService.Put(container.Instantiate<PutCardInBagAction>());
            handleService.Put(container.Instantiate<PopCardFromBagAction>());
        }
    }
}