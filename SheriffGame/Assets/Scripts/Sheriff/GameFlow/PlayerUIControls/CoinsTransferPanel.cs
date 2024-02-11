using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.InputLock;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sheriff.GameFlow.PlayerUIControls
{
    public class CoinsTransferPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField coinsCount;
        [SerializeField] private TMP_Dropdown playerDestinationDropdown;
        [SerializeField] private Button transferButton;
        [SerializeField] private UiToEcsController uiToEcsController;

        [Inject] private EcsContextProvider _ecsContextProvider;

        private readonly List<(PlayerEntity, string)> _playersOptions = new();
        private readonly ReactiveProperty<PlayerEntityId> _targetPlayer = new();
        private readonly ReactiveProperty<int> _potentialMoneyCount = new();
        private readonly CompositeDisposable _disposable = new();
        private PlayerEntity _playerEntity;


        public void Initialize(PlayerEntityId playerIdValue)
        {
            _playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(playerIdValue);
            _targetPlayer.Value = new PlayerEntityId(-1);
            _potentialMoneyCount.Value = 0;
            FillOptions();
            SubscribeInput();
            SubscribeValidations();

        }

        
        
        private void SubscribeValidations()
        {
            
            _targetPlayer
                .CombineLatest(
                    _potentialMoneyCount,
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
            bool isButtonInteractable = false;

            var playerCoins = _playerEntity.hasGoldCashCurrency ? _playerEntity.goldCashCurrency.Value : 0;
            var isEnoughMoney = _potentialMoneyCount.Value > 0 && _potentialMoneyCount.Value <= playerCoins;

            var destinationPlayer = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_targetPlayer.Value);
            var isSelectedDestination = destinationPlayer != null;


            isButtonInteractable = isEnoughMoney && isSelectedDestination;
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

            playerDestinationDropdown.options = _playersOptions.Select(x => new TMP_Dropdown.OptionData(x.Item2)).ToList();
        }

        private void SubscribeInput()
        {
            playerDestinationDropdown.onValueChanged.AsObservable().Subscribe(index =>
            {
                _targetPlayer.Value = _playersOptions.ElementAt(index).Item1?.playerId?.Value ?? new PlayerEntityId(-1);
            }).AddTo(_disposable);

            coinsCount.onValueChanged.AsObservable().Select(x => int.Parse(x)).Subscribe(x =>
            {
                _potentialMoneyCount.Value = x;
            }).AddTo(_disposable);

            transferButton.onClick.AsObservable().Subscribe(x =>
            {
                Transfer();
            }).AddTo(_disposable);
        }


        private async void Transfer()
        {
            using (LoadingOverlay.Lock())
            {
                var commandResult = await uiToEcsController.TransferMoney(
                    _playerEntity.playerId.Value, 
                    _targetPlayer.Value, 
                    _potentialMoneyCount.Value);
                
                if (!commandResult)
                    return;
            }
        }

    }
}