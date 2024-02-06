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
        [SerializeField] private GridProvider selectedCardsGrid;
        [SerializeField] private GridProvider declaredCardsGrid;
        
        private IReadOnlyReactiveProperty<ProductsDeclaration> _declaration;
        private PlayerEntity _playerEntity;
        private IReadOnlyReactiveProperty<SheriffChoice> _sheriffChoice;
        private readonly CompositeDisposable _disposable = new();
        private List<CardView> _spawnedCards = new();


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
            _sheriffChoice.Subscribe(x =>
            {
                skip.interactable = x is SheriffChoice.Check or SheriffChoice.Skip;
                check.interactable = x is SheriffChoice.Check or SheriffChoice.Skip;
            }).AddTo(_disposable);
            
            skip.OnClickAsObservable().Subscribe(x => { }).AddTo(_disposable);
            check.OnClickAsObservable().Subscribe(x => { }).AddTo(_disposable);

            SpawnDeclaredCards();
            SpawnSelectedCards();
        }

        public void Hide()
        {
            _disposable.Clear();
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
                for (int i = 0; i < declaration.Count; i++)
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