using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameFlow.States.ClassicGame.World.Cards;
using Sheriff.GameResources;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class SheriffDeclaredViewUI : MonoBehaviour
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;

        [SerializeField] private Button skip;
        [SerializeField] private Button check;
        [SerializeField] private SheriffDeclaredResultViewUI sheriffDeclaredResultViewUI;
        [SerializeField] private GridProvider selectedCardsGrid;
        [SerializeField] private GridProvider declaredCardsGrid;
        
        private IReadOnlyReactiveProperty<ProductsDeclaration> _declaration;
        private PlayerEntity _playerEntity;
        private IReadOnlyReactiveProperty<SheriffChoice> _sheriffChoice;
        private readonly CompositeDisposable _disposable = new();
        private List<CardView> _spawnedCards = new();

        private Subject<SheriffChoice> _onSelect = new();
        public IObservable<SheriffChoice> OnSelect => _onSelect;


        public void Link(
            IReadOnlyReactiveProperty<ProductsDeclaration> declaration,
            PlayerEntity playerEntity,
            IReadOnlyReactiveProperty<SheriffChoice> sheriffChoice
            )
        {
            _declaration = declaration;
            _playerEntity = playerEntity;
            _sheriffChoice = sheriffChoice;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _disposable.Clear();
            sheriffDeclaredResultViewUI.Show(_playerEntity);
            _sheriffChoice.Subscribe(x =>
            {
                skip.interactable = x is not (SheriffChoice.Check or SheriffChoice.Skip);
                check.interactable = x is not (SheriffChoice.Check or SheriffChoice.Skip);
            }).AddTo(_disposable);

            _sheriffChoice.SkipLatestValueOnSubscribe().Subscribe(x =>
            {
                AffectVisibilitySelected(false);
            }).AddTo(_disposable);
            
            skip.OnClickAsObservable().Subscribe(x => { _onSelect.OnNext(SheriffChoice.Skip); }).AddTo(_disposable);
            check.OnClickAsObservable().Subscribe(x => { _onSelect.OnNext(SheriffChoice.Check); }).AddTo(_disposable);

            AffectVisibilitySelected(true);
            SpawnDeclaredCards();
            SpawnSelectedCards();
        }

        public void Hide()
        {
            _disposable.Clear();
            gameObject.SetActive(false);
            sheriffDeclaredResultViewUI.Hide();
            foreach (var spawnedCard in _spawnedCards)
                spawnedCard.Dispose();
            _spawnedCards.Clear();
        }

        void AffectVisibilitySelected(bool isImmediately)
        {
            bool isVisible = _sheriffChoice.Value == SheriffChoice.Check;
            selectedCardsGrid.gameObject.SetActive(isVisible);
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
    }
}