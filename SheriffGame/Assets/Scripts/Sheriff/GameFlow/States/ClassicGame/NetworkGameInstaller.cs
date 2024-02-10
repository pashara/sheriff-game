using Sheriff.Bootstrap;
using Sheriff.ClientServer;
using Sheriff.ClientServer.Game;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.CommandsApplier;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.View;
using ThirdParty.Randoms;
using UnityEngine;
using Zenject;

namespace Sheriff
{
    public class NetworkGameInstaller : MonoInstaller
    {
        [SerializeField] private CardsProvider _cardsProvider;
        [SerializeField] private GameViewController gameViewController;
        [SerializeField] private PlayerSpawnService playerSpawnService;
        [SerializeField] private ClassicGameControllerWrapper classicGameControllerWrapper;
        
        [SerializeField] private PunGameManager punGameManager;
        [SerializeField] private GameObject punUi;
        public override void InstallBindings()
        {
            var sessionInfo = Container.Resolve<IGameSessionDataProvider>();
            Container.BindInterfacesTo<CardsProvider>().FromInstance(_cardsProvider);
            
            Container.BindInterfacesAndSelfTo<RandomService>().FromInstance(new RandomService(0));
            Container.BindInterfacesAndSelfTo<ClassicGameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameViewController>().FromInstance(gameViewController);
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().FromInstance(playerSpawnService);
            // Container.BindInterfacesAndSelfTo<ClassicGameControllerWrapper>().FromInstance(classicGameControllerWrapper);
            Container.BindInterfacesAndSelfTo<LinkWithVisualService>().AsSingle();

            if (sessionInfo.IsNetwork)
                Container.BindInterfacesTo<NetworkCommandsApplyService>().AsSingle();
            else
                Container.BindInterfacesTo<CommandsApplyService>().AsSingle();

            Container.BindInterfacesAndSelfTo<SheriffCheckHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SherifSelectService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EcsContextProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayersAssociations>().AsSingle();

            if (sessionInfo.IsNetwork)
            {
                punGameManager.gameObject.SetActive(true);
                punUi.gameObject.SetActive(true);
                Container.BindInterfacesAndSelfTo<PunGameManager>().FromInstance(punGameManager);
            }
            else
            {
                punGameManager.gameObject.SetActive(false);
                punUi.gameObject.SetActive(false);
                Container.BindInterfacesAndSelfTo<PunManagerMoq>().FromInstance(new PunManagerMoq(classicGameControllerWrapper)).AsSingle();
            }
        }
    }
}