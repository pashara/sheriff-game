using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Sheriff.InputLock;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMP_Dropdown = TMPro.TMP_Dropdown;

namespace Sheriff.GameFlow.PlayerUIControls
{
    public class GameResourcesTransferPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resourcesDropdown;
        [SerializeField] private TMP_Dropdown playerDestinationDropdown;
        [SerializeField] private Button transferButton;
        [SerializeField] private UiToEcsController uiToEcsController;


        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;

        private readonly List<(PlayerEntity, string)> _playersOptions = new();
        private readonly ReactiveProperty<PlayerEntityId> _targetPlayer = new();
        private readonly ReactiveProperty<GameResourceType> _potentialResource = new();
        private readonly CompositeDisposable _disposable = new();
        private PlayerEntity _playerEntity;
        private readonly List<(GameResourceType, GameResourceCategory, string, Sprite)> _resourcesOptions = new();


        public void Initialize(PlayerEntityId playerIdValue)
        {
            _playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(playerIdValue);
            _targetPlayer.Value = new PlayerEntityId(-1);
            _potentialResource.Value = GameResourceType.None;
            FillOptions();
            SubscribeInput();
            SubscribeValidations();
            ApplyValidation();
        }

        
        
        private void SubscribeValidations()
        {
            _targetPlayer
                .CombineLatest(
                    _potentialResource,
                    _playerEntity.OnGoldCashCurrency(),
                    F._)
                .ThrottleFrame(1)
                .Subscribe(x =>
                {
                    ApplyValidation();
                })
                .AddTo(_disposable);
        }

        private void ApplyValidation()
        {
            var isButtonInteractable = false;

            var isPotentialResourceSelected = _potentialResource.Value != GameResourceType.None;

            var destinationPlayer = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_targetPlayer.Value);
            var isSelectedDestination = destinationPlayer != null;


            isButtonInteractable = isPotentialResourceSelected && isSelectedDestination;
            transferButton.interactable = isButtonInteractable;
        }

        public void Deinitialize()
        {
            _disposable.Clear();
        }

        private void FillOptions()
        {
            _playersOptions.Clear();
            _playersOptions.Add((null, "select"));
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                if (!playerEntity.hasPlayerId)
                    continue;

                if (playerEntity == _playerEntity)
                    continue;

                _playersOptions.Add((playerEntity, $"{playerEntity.nickname.Value}"));
            }
            
            if (_playerEntity.hasTransferredResources)
            {
                var allowed = _playerEntity.transferredResources.Value
                    .AllowedResources.Where(x => x.Value > 0)
                    .Select(x => x.Key);
                
                var smuggled = _playerEntity.transferredResources.Value
                    .NotAllowedResources.Where(x => x.Value > 0)
                    .Select(x => x.Key);

                _resourcesOptions.Clear();
                _resourcesOptions.Add((GameResourceType.None, GameResourceCategory.Allowed, "None", null));
                
                foreach (var resource in allowed)
                {
                    var config = _cardConfigProvider.Get(resource);
                    _resourcesOptions.Add((resource, config.Category, config.Title, config.Icon));
                }

                foreach (var resource in smuggled)
                {
                    var config = _cardConfigProvider.Get(resource);
                    _resourcesOptions.Add((resource, config.Category, config.Title, config.Icon));
                }
            }

            resourcesDropdown.options = _resourcesOptions
                .Select(x => new TMP_Dropdown.OptionData(x.Item3, x.Item4)).ToList();
            
            playerDestinationDropdown.options = _playersOptions.Select(x => new TMP_Dropdown.OptionData(x.Item2)).ToList();
        }

        private void SubscribeInput()
        {
            _targetPlayer.Value = _playersOptions.ElementAtOrDefault(playerDestinationDropdown.value).Item1?.playerId?.Value ?? new PlayerEntityId(-1);
            playerDestinationDropdown.onValueChanged.AsObservable().Subscribe(index =>
            {
                _targetPlayer.Value = _playersOptions.ElementAt(index).Item1?.playerId?.Value ?? new PlayerEntityId(-1);
            }).AddTo(_disposable);
            
            _potentialResource.Value = _resourcesOptions.ElementAtOrDefault(resourcesDropdown.value).Item1;
            resourcesDropdown.onValueChanged.AsObservable().Subscribe(index =>
            {
                _potentialResource.Value = _resourcesOptions.ElementAt(index).Item1;
            }).AddTo(_disposable);

            transferButton.onClick.AsObservable().Subscribe(x =>
            {
                Transfer();
            }).AddTo(_disposable);
        }


        private async void Transfer()
        {
            try
            {
                using (LoadingOverlay.Lock())
                {
                    var commandResult = await uiToEcsController.TransferResource(
                        _playerEntity.playerId.Value,
                        _targetPlayer.Value,
                        _potentialResource.Value);

                    if (!commandResult)
                        return;
                }
            }
            catch (Exception e)
            {
                
            }
        }

    }
}