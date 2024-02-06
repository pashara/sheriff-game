using System.Collections.Generic;
using Sheriff.DataBase;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.View;
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
            var cardEntity = _ecsContextProvider.Context.card.GetEntityWithCardId(cardEntityId);
            var config = _cardConfigProvider.Get(cardEntity.resourceType.Value);
            var targetSpawn = decGrid.GetAt(_spawnedCards.Count);

            var cardInstance = _container.InstantiatePrefabForComponent<CardView>(config.CardView, targetSpawn.target);
            cardInstance.Link(cardEntity);
            _spawnedCards.Add(cardInstance);
        }
    }
}