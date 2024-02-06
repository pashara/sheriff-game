using System.Linq;
using Sheriff.ECS;
using Sheriff.GameFlow.States.ClassicGame.View;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class LinkWithVisualService
    {
        private readonly GameViewController _gameViewController;
        private readonly PlayerSpawnService _playerSpawnService;
        private readonly EcsContextProvider _ecsContextProvider;

        public LinkWithVisualService(
            GameViewController gameViewController, 
            PlayerSpawnService playerSpawnService,
            EcsContextProvider ecsContextProvider)
        {
            _gameViewController = gameViewController;
            _playerSpawnService = playerSpawnService;
            _ecsContextProvider = ecsContextProvider;
        }
        
        public void Link()
        {
            _gameViewController.LinkAllPlayers();

            var players = _ecsContextProvider.Context.player.GetEntities().ToList();
            players.Sort((a, b) => 
                    (int)(a.playerId.Value.EntityID - b.playerId.Value.EntityID));
            
            for (int i = 0; i < Mathf.Min(_gameViewController.WorldPlayerCardsControllers.Count, players.Count); i++)
            {
                var control = _gameViewController.WorldPlayerCardsControllers[i];
                control.Link(players[i]);
            }


            int j = 0;
            foreach (var playerEntity in players)
            {
                _playerSpawnService.Spawn(playerEntity, j == 0);
                j++;
            }
        }
    }
}