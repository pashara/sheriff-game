using System.Collections.Generic;
using NaughtyCharacter;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class WorldPlayerCardsController : MonoBehaviour, IInteractableGame
    {
        [SerializeField] private GameObject interactionCamera;
        [SerializeField] private GridProvider playerCards;
        [SerializeField] private PlayerInput input;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        [Inject] private DiContainer _container;
        const int Max = 6;
        public void Link(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
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
        }

        [Button]
        public void Close()
        {
            input.gameObject.SetActive(false);
            interactionCamera.SetActive(false);
            DestroyCards();
        }


        void DestroyCards()
        {
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

            int i = 0;
            foreach (var cardEntity in cards)
            {
                var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
                var targetSpawn = playerCards.GetAt(i);

                var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
                cardInstance.Link(cardEntity);
                _spawnedCards.Add(cardInstance);
                i++;
                
                if (i == 6)
                    break;
            }

        }

        public void Interact(Character character)
        {
            Open();
        }
    }
}
