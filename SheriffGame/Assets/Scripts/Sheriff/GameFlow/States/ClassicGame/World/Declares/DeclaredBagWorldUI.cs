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
        [SerializeField] private OwnerDeclaredViewUI playerUI;

        private ReactiveProperty<ProductsDeclaration> _productsDeclaration = new();
        private ReactiveProperty<SheriffChoice> _sheriffChoice = new();
        [Inject] private EcsContextProvider _contextProvider;
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


        public void OpenAsSheriff()
        {
            ActivateController();
            sheriffUI.Show();
            ownerUI.Hide();
            playerUI.Hide();
        }

        public void OpenAsOwner()
        {
            ActivateController();
            sheriffUI.Hide();
            ownerUI.Show();
            playerUI.Hide();
        }

        public void OpenAsPlayer()
        {
            ActivateController();
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
        

        private void ActivateController()
        {
            _disposable.Clear();
            
            inputHandler.SetActive(true);
            mainUI.SetActive(true);

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