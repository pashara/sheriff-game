using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class DeclaredBagWorldUI : MonoBehaviour
    {
        [SerializeField] private GameObject inputHandler;
        [SerializeField] private GameObject mainUI;
        [SerializeField] private SheriffDeclaredViewUI sheriffUI;
        [SerializeField] private OwnerDeclaredViewUI ownerUI;
        [SerializeField] private PlayerDeclaredViewUI playerUI;

        private ReactiveProperty<ProductsDeclaration> _productsDeclaration = new();
        private ReactiveProperty<SheriffChoice> _sheriffChoice = new();
        [Inject] private EcsContextProvider _contextProvider;
        [Inject] private DiContainer _container;
        [Inject] private SheriffCheckHandler _sheriffCheckHandler;
        [Inject] private CommandsApplyService _commandsApplyService;
        
        private PlayerEntity _playerEntity;
        private CompositeDisposable _disposable = new();

        public PlayerEntityId Owner { get; private set; }

        public void Link(PlayerEntity playerEntity)
        {
            Owner = playerEntity.playerId.Value;
            _playerEntity = playerEntity;
            sheriffUI.Link(_productsDeclaration, playerEntity, _sheriffChoice);
            ownerUI.Link(playerEntity, _sheriffChoice);
            playerUI.Link(playerEntity, _sheriffChoice);
            
            Close();
        }


        public void OpenAsSheriff(PlayerEntity player)
        {
            ActivateController(player);
            sheriffUI.Show();
            ownerUI.Hide();
            playerUI.Hide();
        }

        public void OpenAsOwner(PlayerEntity player)
        {
            ActivateController(player);
            sheriffUI.Hide();
            ownerUI.Show();
            playerUI.Hide();
        }

        public void OpenAsPlayer(PlayerEntity player)
        {
            ActivateController(player);
            sheriffUI.Hide();
            ownerUI.Hide();
            playerUI.Show();
        }


        public void Close()
        {
            sheriffUI.Hide();
            ownerUI.Hide();
            playerUI.Hide();
            DeactivateController();
        }
        

        private void ActivateController(PlayerEntity player)
        {
            _disposable.Clear();
            
            inputHandler.SetActive(true);
            mainUI.SetActive(true);

            sheriffUI.OnSelect.Subscribe(x =>
            {
                if (!player.isSheriff)
                    return;

                SherifCheckResult checkResult = null;
                if (x == SheriffChoice.Check)
                {
                    checkResult = _sheriffCheckHandler.Check(player.playerId.Value, _playerEntity.playerId.Value);
                }
                else if (x == SheriffChoice.Skip)
                {
                    checkResult = _sheriffCheckHandler.Skip(player.playerId.Value, _playerEntity.playerId.Value);
                }

                var command = _container.Instantiate<CheckDealersCommand>().Calculate(new CheckDealersCommand.Params()
                {
                    CheckResult = checkResult
                });
                _commandsApplyService.Apply(command);
                
            }).AddTo(_disposable);

            _playerEntity.OnSheriffCheckResult().Subscribe(x =>
            {
                if (x == null)
                {
                    _sheriffChoice.Value = SheriffChoice.None;
                }
                else if (x.Value is SkipCheckSherifResult)
                {
                    _sheriffChoice.Value = SheriffChoice.Skip;
                }
                else
                {
                    _sheriffChoice.Value = SheriffChoice.Check;
                }
            });
            
            sheriffUI.Hide();
            
            _playerEntity.OnDeclareResourcesByPlayer().Subscribe(x =>
            {
                _productsDeclaration.Value = x?.Value;
            }).AddTo(_disposable);
        }

        private void DeactivateController()
        {
            inputHandler.SetActive(false);
            mainUI.SetActive(false);
            _disposable.Clear();
        }

    }
}