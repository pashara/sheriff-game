using System;
using System.Collections.Generic;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.PlayerUIControls.PlayersList
{
    public class PlayersPanel : MonoBehaviour, IDisposable
    {
        [SerializeField] private PlayerListElementUI playerPrefab;
        [SerializeField] private Transform root;

        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private DiContainer _container;
        private readonly List<PlayerListElementUI> _spawned = new();
        
        public void Initialize(PlayerEntityId playerIdValue)
        {
            Dispose();
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                var instance = _container.InstantiatePrefabForComponent<PlayerListElementUI>(playerPrefab, root);
                instance.Initialize(playerEntity);
                _spawned.Add(instance);
            }
        }
        
        public void Dispose()
        {
            foreach (var element in _spawned)
            {
                element.Dispose();
            }
            _spawned.Clear();
        }
    }
}
