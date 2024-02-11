using System.Collections.Generic;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.View.UI;
using Sheriff.GameResources;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.PlayerUIControls
{
    public class PlayerTransferredResourcesPanel : MonoBehaviour
    {
        [SerializeField] private Transform allowedElementsRoot;
        [SerializeField] private Transform smugglingElementsRoot;

        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;

        private List<CardViewUI> _spawnedCards = new();

        private CompositeDisposable _disposable = new();
        
        public void Initialize(PlayerEntityId player)
        {
            _ecsContextProvider
                .Context
                .player
                .GetEntityWithPlayerId(player)
                .OnTransferredResources()
                .Subscribe(x =>
                {
                    Spawn(x?.Value);
                }).AddTo(_disposable);
        }

        public void Deinitialize()
        {
            ClearCards();
            _disposable.Clear();
        }

        private void ClearCards()
        {
            foreach (var spawnedCard in _spawnedCards)
                spawnedCard.Dispose();
            
            _spawnedCards.Clear();
        }

        private void Spawn(TransferredObjects transferredCards)
        {
            ClearCards();
            if (transferredCards != null)
            {
                foreach (var resource in transferredCards.AllowedResources)
                    SpawnCard(resource.Key, resource.Value, allowedElementsRoot);
                
                foreach (var resource in transferredCards.NotAllowedResources)
                    SpawnCard(resource.Key, resource.Value, smugglingElementsRoot);
            }
        }
        
        private void SpawnCard(GameResourceType resourceType, int count, Transform root)
        {
            if (count != 0)
            {
                var viewPrefab = _cardConfigProvider.Get(resourceType).UICardCardView;
                var instance = _container.InstantiatePrefabForComponent<CardViewUI>(viewPrefab, root);
                instance.Initialize();
                instance.ApplyCount(count);
                _spawnedCards.Add(instance);
            }
        }
        
    }
}