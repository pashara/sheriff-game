using System.Collections.Generic;
using Sheriff.ECS;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.ResultUIControl
{
    public class ResultUIController : MonoBehaviour
    {
        [SerializeField] private Canvas root;
        [SerializeField] private Transform elementsRoot;
        [SerializeField] private PlayerStatisticsView prefab;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private DiContainer _container;
        private readonly List<PlayerStatisticsView> _spawnedElements = new();
        
        [Button]
        public void Open()
        {
            root.gameObject.SetActive(true);
            var priority = _container.Instantiate<ResultCalculateHandler>().Calculate();

            int place = 1;
            foreach (var playerId in priority)
            {
                var player = _ecsContextProvider.Context.player.GetEntityWithPlayerId(playerId);
                var instance = _container.InstantiatePrefabForComponent<PlayerStatisticsView>(prefab, elementsRoot);
                _spawnedElements.Add(instance);
                instance.Fill(place, player);
                place++;
            }
        }

        [Button]
        public void Close()
        {
            root.gameObject.SetActive(false);
            _spawnedElements.ForEach(x => x.Dispose());
            _spawnedElements.Clear();
            
        }
    }
}