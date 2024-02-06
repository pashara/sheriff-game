using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyCharacter;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class WorldPlayerCardsController : MonoBehaviour
    {
        [SerializeField] private GameObject interactionCamera;
        [SerializeField] private GridProvider playerCards;
        [SerializeField] private CardFlyController cardFlyController;
        [SerializeField] private WorldToEcsController worldToEcsController;

        [SerializeField] private DecCardsWorldController decCardsWorldController;
        [SerializeField] private DeclarePoolController declarePoolController;
        
        [SerializeField] private PlayerInput input;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;

        private List<CardView> cardPlacements = new();
        const int Max = 6;
        public IReadOnlyCollection<CardView> CardsInBag => declarePoolController.CardViews;

        public PlayerEntityId Owner => _playerEntity.playerId.Value;

        public void PutInBag(CardView cardView)
        {
            declarePoolController.PutCardInBag(cardView);

            var cardIndex = cardPlacements.IndexOf(cardView);
            cardPlacements[cardIndex] = null;
            
            var i = 0;
            foreach (var card in declarePoolController.CardViews)
            {
                cardFlyController.PlayMoveToBag(card, i).Forget();
                i++;
            }
        }

        public async void GetNewCard()
        {
            var ids = await worldToEcsController.GetNewCard();
            foreach (var id in ids)
            {
                SpawnNewCard(id);
            }
        }

        public async void ReleaseCard(CardView cardView)
        {
            declarePoolController.RemoveFromBag(cardView);
            var cardIndex = cardPlacements.IndexOf(cardView);
            if (cardIndex >= 0)
            {
                cardPlacements[cardIndex] = null;
            }

            await worldToEcsController.ReleaseCard(cardView);
            await cardFlyController.PlaMoveToDec(cardView);

            await UniTask.Delay(1000);
            decCardsWorldController.PutCard(cardView.CardEntity.cardId.Value);
            _spawnedCards.Remove(cardView);
            cardView.Dispose();
        }

        public void PopFromBag(CardView cardView)
        {
            declarePoolController.RemoveFromBag(cardView);
            var cardIndex = cardPlacements.IndexOf(null);
            cardPlacements[cardIndex] = cardView;
            
            cardFlyController.PlayMoveFromBag(cardView, cardIndex).Forget();
            
            var i = 0;
            foreach (var card in declarePoolController.CardViews)
            {
                cardFlyController.PlayMoveToBag(card, i).Forget();
                i++;
            }
        }
        
        

        public void Link(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
            worldToEcsController.Link(playerEntity);
            declarePoolController.Link(playerEntity);
            declarePoolController.Dispose();
        }

        private readonly List<CardView> _spawnedCards = new();
        private PlayerEntity _playerEntity;


        [Button]
        public void Open()
        {
            DestroyCards();
            input.gameObject.SetActive(true);
            interactionCamera.SetActive(true);
            SpawnCards();
            declarePoolController.Initialize();
        }

        [Button]
        public void Close()
        {
            input.gameObject.SetActive(false);
            interactionCamera.SetActive(false);
            declarePoolController.Dispose();
            DestroyCards();
            declarePoolController.ClearState();
        }


        void DestroyCards()
        {
            cardPlacements.Clear();
            try
            {
                foreach (var spawnedCard in _spawnedCards)
                {
                    spawnedCard?.Dispose();
                }
            }
            catch
            {
                // ignored
            }

            _spawnedCards.Clear();
        }
        
        private void SpawnCards()
        {
            var cards = _ecsContextProvider.Context.card.GetEntitiesWithCardOwner(_playerEntity.playerId.Value);
            var declaredCards = _playerEntity.hasSelectedCards
                ? _playerEntity.selectedCards.Value.Select(x =>
                    _ecsContextProvider.Context.card.GetEntityWithCardId(x)).ToList()
                : new List<CardEntity>();


            var onHandCards = cards.Where(x => x.isCardOnHand).ToList();
            var selectedCards = cards.Where(x => x.isSelectToDeclare).ToList();
            
            
            for (int i = 0; i < Mathf.Min(onHandCards.Count, 6); i++)
            {
                var cardEntity = onHandCards[i];
                
                var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
                var targetSpawn = playerCards.GetAt(i);

                var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
                cardInstance.Link(cardEntity);
                _spawnedCards.Add(cardInstance);
                cardPlacements.Add(cardInstance);
            }
            
            for (int i = 0; i < selectedCards.Count; i++)
            {
                var cardEntity = selectedCards[i];
                
                var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
                var targetSpawn = cardFlyController.GetBagTarget(i);

                var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
                
                declarePoolController.PutCardInBag(cardInstance);
                cardInstance.Link(cardEntity);
                _spawnedCards.Add(cardInstance);
            }
        }

        private void SpawnNewCard(CardEntityId id)
        {
            var cardEntity = _ecsContextProvider.Context.card.GetEntityWithCardId(id);
            if (!cardEntity.isCardOnHand)
                return;
            
            var cardIndex = cardPlacements.IndexOf(null);
            if (cardIndex == -1)
                return;
            
                
            var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
            var targetSpawn = playerCards.GetAt(cardIndex);

            var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
            cardInstance.Link(cardEntity);
            _spawnedCards.Add(cardInstance);
            cardPlacements[cardIndex] = cardInstance;

            cardFlyController.PlayAppearAnimation(cardInstance, cardIndex);
        }

        public void Interact(Character character)
        {
            Open();
        }
    }
}
