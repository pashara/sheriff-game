using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.CommandsApplier;
using Sheriff.GameFlow.States.ClassicGame.World.Cards;
using Sheriff.InputLock;
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
        [SerializeField] private WorldToEcsController worldToEcsController;

        private ReactiveProperty<ProductsDeclaration> _productsDeclaration = new();
        private ReactiveProperty<SheriffChoice> _sheriffChoice = new();
        [Inject] private EcsContextProvider _contextProvider;
        [Inject] private DiContainer _container;
        [Inject] private SheriffCheckHandler _sheriffCheckHandler;
        [Inject] private ICommandsApplyService _commandsApplyService;
        
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
            if (_playerEntity == null)
                return;
            ActivateController(player);
            sheriffUI.Show();
            ownerUI.Hide();
            playerUI.Hide();
        }

        public void OpenAsOwner(PlayerEntity player)
        {
            if (_playerEntity == null)
                return;
            ActivateController(player);
            sheriffUI.Hide();
            ownerUI.Show();
            playerUI.Hide();
        }

        public void OpenAsPlayer(PlayerEntity player)
        {
            if (_playerEntity == null)
                return;
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
            if (_playerEntity == null)
                return;
            
            _disposable.Clear();
            
            inputHandler.SetActive(true);
            mainUI.SetActive(true);

            sheriffUI.OnSelect.Subscribe(async x =>
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

                using (LoadingOverlay.Lock())
                {
                    await worldToEcsController.ApplySheriffChoice(checkResult);
                }

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