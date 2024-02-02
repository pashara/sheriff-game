using Sheriff.ECS;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class DeckViewController : MonoBehaviour
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private DiContainer _container;
        [SerializeField] private CardView cardPrefab;
        
        public void Link()
        {
            foreach (var cardEntity in _ecsContextProvider.Context.card.GetEntities())
            {
                var instance = _container.InstantiatePrefabForComponent<CardView>(cardPrefab, transform);
                instance.Link(cardEntity);
            }
        }
    }
}