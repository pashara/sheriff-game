using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameFlow.States.ClassicGame.World.Cards;
using Sheriff.GameResources;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class PlayerDeclaredViewUI : MonoBehaviour
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;

        [SerializeField] private GridProvider selectedCardsGrid;
        [SerializeField] private GridProvider declaredCardsGrid;
        [SerializeField] private SheriffDeclaredResultViewUI sheriffDeclaredResultViewUI;
        
        private PlayerEntity _playerEntity;
        private List<CardView> _spawnedCards = new();
        private IReadOnlyReactiveProperty<SheriffChoice> _sheriffChoice;
        private CompositeDisposable _disposable = new();

        public void Link(PlayerEntity playerEntity, IReadOnlyReactiveProperty<SheriffChoice> sheriffChoice)
        {
            _playerEntity = playerEntity;
            _sheriffChoice = sheriffChoice;
        }
        
        public void Show()
        {
            _disposable.Clear();
            
            gameObject.SetActive(true);
            SpawnSelectedCards();
            SpawnDeclaredCards();
            AffectVisibilitySelected(true);
            sheriffDeclaredResultViewUI.Show(_playerEntity);
            
            _sheriffChoice.SkipLatestValueOnSubscribe().Subscribe(x =>
            {
                AffectVisibilitySelected(false);
            }).AddTo(_disposable);
        }

        public void Hide()
        {
            _disposable.Clear();
            sheriffDeclaredResultViewUI.Hide();
            
            gameObject.SetActive(false);
            foreach (var spawnedCard in _spawnedCards)
                spawnedCard.Dispose();
            _spawnedCards.Clear();
        }


        private void SpawnDeclaredCards()
        {
            if (!_playerEntity.hasDeclareResourcesByPlayer)
                return;

            var selectedCards = _playerEntity.declareResourcesByPlayer.Value;

            List<GameResourceType> gameResourceTypes = new();
            foreach (var declaration in selectedCards.Declarations)
                for (int i = 0; i < declaration.ProductsCount; i++)
                    gameResourceTypes.Add(declaration.ResourceType);

            Spawn(gameResourceTypes, declaredCardsGrid);
        }

        private void SpawnSelectedCards()
        {
            if (!_playerEntity.hasSelectedCards)
                return;

            var selectedCards = _playerEntity.selectedCards.Value
                .Select(x => _ecsContextProvider.Context.card.GetEntityWithCardId(x).resourceType.Value).ToList();
            
            Spawn(selectedCards, selectedCardsGrid);
        }


        private void Spawn(List<GameResourceType> elements, GridProvider gridProvider)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var gameResourceType = elements[i];
                var config = _cardConfigProvider.Get(gameResourceType);
                var targetSpawn = gridProvider.GetAt(i);

                var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
                
                cardInstance.Link(gameResourceType);
                _spawnedCards.Add(cardInstance);
            }
        }

        void AffectVisibilitySelected(bool isImmediately)
        {
            bool isVisible = _sheriffChoice.Value == SheriffChoice.Check;
            selectedCardsGrid.gameObject.SetActive(isVisible);
        }

    }
}