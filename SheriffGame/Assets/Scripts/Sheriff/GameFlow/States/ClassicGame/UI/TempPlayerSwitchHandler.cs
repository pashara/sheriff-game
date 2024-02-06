using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Entitas;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.UI
{
    public class TempPlayerSwitchHandler : MonoBehaviour
    {
        private EcsContextProvider _ecsContextProvider;
        [SerializeField] private TMP_Dropdown dropdown;
        private List<PlayerEntityId> _actualElements;

        [Inject]
        private void Construct(EcsContextProvider ecsContextProvider)
        {
            _ecsContextProvider = ecsContextProvider;
            _ecsContextProvider.Context.player.OnEntityCreated -= PlayerOnOnEntityCreated;
            _ecsContextProvider = ecsContextProvider;
            UpdateElements();
            _ecsContextProvider.Context.player.OnEntityCreated += PlayerOnOnEntityCreated;
            
            dropdown.onValueChanged.AddListener(SelectCharacter);
        }
        
        private void OnDestroy()
        {
            _ecsContextProvider.Context.player.OnEntityCreated -= PlayerOnOnEntityCreated;
        }

        private void PlayerOnOnEntityCreated(IContext context, IEntity entity)
        {
            UpdateElements();
        }

        private async void UpdateElements()
        {
            await UniTask.DelayFrame(1);
            var elements = _ecsContextProvider
                .Context
                .player
                .GetEntities()
                .Select(x => (new TMP_Dropdown.OptionData(x.playerId.Value.ToString()), x.playerId.Value))
                .ToList();
            dropdown.options = elements.Select(x => x.Item1).ToList();
            _actualElements = elements.Select(x => x.Value).ToList();
        }
        
        private void SelectCharacter(int index)
        {
            var target = _actualElements[index];
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                playerEntity.playerController.Value.enabled = target == playerEntity.playerId.Value;
            }
        }
    }
}