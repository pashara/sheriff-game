using Cysharp.Threading.Tasks;
using Sheriff.ECS;
using Sheriff.GameFlow.PlayerUIControls.PlayersList;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.PlayerUIControls
{
    public class MainPlayerUIControl : MonoBehaviour
    {
        [SerializeField] private PlayerTransferredResourcesPanel playerTransferredResourcesPanel;
        [SerializeField] private GameResourcesTransferPanel resourcesTransferPanel;
        [SerializeField] private CoinsTransferPanel coinsTransferPanel;
        [SerializeField] private PlayersPanel playersPanel;
        [SerializeField] private Canvas root;

        private bool _isOpened = false;

        [Inject] private EcsContextProvider _ecsContextProvider;
        
        private void Awake()
        {
            ForceHide();
        }

        public async void Open()
        {
            if (_isOpened)
                return;
            
            _isOpened = true;
            PlayerEntity actualPlayer = null;
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.hasPlayerController && playerEntity.playerController.Value != null &&
                    playerEntity.playerController.Value.enabled)
                {
                    actualPlayer = playerEntity;
                    break;
                }
            }

            if (actualPlayer != null)
            {
                playerTransferredResourcesPanel.Initialize(actualPlayer.playerId.Value);
                resourcesTransferPanel.Initialize(actualPlayer.playerId.Value);
                coinsTransferPanel.Initialize(actualPlayer.playerId.Value);
                playersPanel.Initialize(actualPlayer.playerId.Value);

                await UniTask.DelayFrame(1);
                root.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (!_isOpened)
            {
                return;
            }

            _isOpened = false;
            ForceHide();
        }


        void ForceHide()
        {
            root.gameObject.SetActive(false);
            playerTransferredResourcesPanel.Deinitialize();
            resourcesTransferPanel.Deinitialize();
            coinsTransferPanel.Deinitialize();
            playersPanel.Dispose();
        }
    }
}
