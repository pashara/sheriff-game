using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sheriff.ECS;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
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
        private void UpdateAll()
        {
            foreach (var spawnedElement in _spawnedElements)
            {
                spawnedElement.UpdateLayout();
            }
            LayoutRebuilder.MarkLayoutForRebuild(elementsRoot as RectTransform);
        }

        [Button]
        public async void Open()
        {
            root.gameObject.SetActive(true);
            var playerCalculations = _container.Instantiate<ResultCalculateHandler>().Calculate();

            foreach (var calculations in playerCalculations)
            {
                var player = _ecsContextProvider.Context.player.GetEntityWithPlayerId(calculations.PlayerEntityId);
                var instance = _container.InstantiatePrefabForComponent<PlayerStatisticsView>(prefab, elementsRoot);
                _spawnedElements.Add(instance);
                instance.Fill(calculations, player);
            }

            await UniTask.DelayFrame(1);
            UpdateAll();
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