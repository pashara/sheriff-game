﻿using System.Linq;
using Sheriff.ECS;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class GameViewController : MonoBehaviour
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private DiContainer _container;
        [SerializeField] private Transform playersRoot;
        [SerializeField] private DeckViewController deckViewController;
        [SerializeField] private PlayerViewController playerPrefab;
        
        public void LinkDeck()
        {
            deckViewController.Link();
        }
        
        public void LinkAllPlayers()
        {
            var i = 0;
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                var instance = _container.InstantiatePrefabForComponent<PlayerViewController>(playerPrefab, playersRoot);
                instance.Link(playerEntity);
                instance.transform.localPosition = new Vector3(10f * i, 0f, 0f);
                i++;
            }
        }
    }
}