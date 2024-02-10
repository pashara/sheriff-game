using System;
using System.Collections.Generic;
using NaughtyCharacter;
using Photon.Realtime;
using Sheriff.ClientServer.Game;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.States;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck;
using Sheriff.GameFlow.States.ClassicGame.States.Shopping;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameController
    {
        private readonly ClassicGameStateMachine _gameStateMachine;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly PlayerSpawnService _playerSpawnService;
        private readonly IPunSender _punGameManager;
        private readonly LinkWithVisualService _linkWithVisualService;

        public ClassicGameController(
            ClassicGameStateMachine gameStateMachine,
            EcsContextProvider ecsContextProvider,
            PlayerSpawnService playerSpawnService,
            IPunSender punGameManager,
            LinkWithVisualService linkWithVisualService
            )
        {
            _gameStateMachine = gameStateMachine;
            _ecsContextProvider = ecsContextProvider;
            _playerSpawnService = playerSpawnService;
            _punGameManager = punGameManager;
            _linkWithVisualService = linkWithVisualService;
        }

        public void LazySpawnPlayers()
        {
            List<PlayerController> spawnedPlayers = new();
            var playerEntities = _ecsContextProvider.Context.player.GetEntities();
            for (int j = 0; j < playerEntities.Length; j++)
            {
                var playerEntity = playerEntities[j];
                spawnedPlayers.Add(_playerSpawnService.Spawn(playerEntity, j == 0));
            }
            
            _linkWithVisualService.Link(spawnedPlayers);

            foreach (var playerController in spawnedPlayers)
                playerController.GetComponent<CharacterController>().enabled = true;
        }
        
        public void StartGame(int players)
        {
            _gameStateMachine.Enter<InitializeGameState, InitializeGamePayload>(new InitializeGamePayload()
            {
                PlayersCount = players
            });

            List<PlayerController> spawnedPlayers = new();
            var playerEntities = _ecsContextProvider.Context.player.GetEntities();
            for (int j = 0; j < players; j++)
            {
                var playerEntity = playerEntities[j];
                spawnedPlayers.Add(_playerSpawnService.Spawn(playerEntity, j == 0));
            }
            
            _linkWithVisualService.Link(spawnedPlayers);

            foreach (var playerController in spawnedPlayers)
                playerController.GetComponent<CharacterController>().enabled = true;
        }
        
        public void StartGame(Player[] players)
        {
            _gameStateMachine.Enter<InitializeGameState, InitializeGamePayload>(new InitializeGamePayload()
            {
                PlayersCount = players.Length
            });
            
            _linkWithVisualService.Link(players);
        }

        public void Link(Player[] players)
        {
            _linkWithVisualService.Link(players);
            
        }


        public void ApplyGameState(Type loadDataStateType)
        {
            var actualStateProvider = new ActualStateProviderProvider();
            _ecsContextProvider.Context.game.gameIdEntity.ReplaceActualStateProviderWritable(actualStateProvider);
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                playerEntity.ReplaceActualStateProvider(actualStateProvider);
            }

            if (loadDataStateType != null)
            {
                _gameStateMachine.Enter(loadDataStateType);
            }
        }

        public void OnReady<T>() where T : ClassicGameState
        {
            OnReady(typeof(T));
        }

        public void OnReady(Type type)
        {
            if (type == typeof(InitializeGameState))
            {
                _gameStateMachine.Enter<SetSheriffStatusState>();
                return;
            }
            
            if (type == typeof(SetSheriffStatusState))
            {
                _gameStateMachine.Enter<ShoppingState>();
                _punGameManager.SendGameState();
                return;
            }
            
            if (type == typeof(ShoppingState))
            {
                _gameStateMachine.Enter<SheriffCheckState>();
                _punGameManager.SendGameState();
                return;
            }
            
            if (type == typeof(SheriffCheckState))
            {
                _gameStateMachine.Enter<SetSheriffStatusState>();
                return;
            }
        }
    }
}