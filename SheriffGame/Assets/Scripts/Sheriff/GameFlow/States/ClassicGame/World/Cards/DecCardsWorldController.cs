using System;
using System.Collections.Generic;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.States.Shopping;
using Sheriff.GameFlow.States.ClassicGame.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class DecCardsWorldController : MonoBehaviour
    {
        [SerializeField] private GridProvider decGrid;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;

        private List<CardView> _spawnedCards = new();
        private CompositeDisposable _disposable = new();
        private bool _isListen;

        void Listen()
        {
            if (_isListen)
                return;
            _disposable.Clear();
            try
            {
                _ecsContextProvider.Context.game.gameIdEntity.OnActualStateProviderWritable().Subscribe(x =>
                {
                    _isListen = true;
                    if (x?.Value?.ActualState?.Value == null)
                    {
                        Clear();
                    }
                    else if (x.Value.ActualState.Value.GetType() != typeof(ShoppingState))
                    {
                        Clear();
                    }
                }).AddTo(_disposable);
            }
            catch
            {
                
            }
        }
        
        public void Clear()
        {
            _spawnedCards.ForEach(x => x.Dispose());
            _spawnedCards.Clear();
        }
        
        public void PutCard(CardEntityId cardEntityId)
        {
            SpawnCard(cardEntityId);
        }


        private void SpawnCard(CardEntityId cardEntityId)
        {
            Listen();
            var cardEntity = _ecsContextProvider.Context.card.GetEntityWithCardId(cardEntityId);
            var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
            var targetSpawn = decGrid.GetAt(_spawnedCards.Count);

            var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
            cardInstance.Link(cardEntity);
            _spawnedCards.Add(cardInstance);
        }

        private void OnDestroy()
        {
            _disposable.Clear();
        }
    }
}