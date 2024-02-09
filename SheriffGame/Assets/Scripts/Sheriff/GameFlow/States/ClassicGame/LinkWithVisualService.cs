using System.Collections.Generic;
using System.Linq;
using NaughtyCharacter;
using Photon.Pun;
using Photon.Realtime;
using Sheriff.ClientServer.Players;
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
        
        public void Link(List<PlayerController> spawnedPlayers)
        {
            _gameViewController.LinkAllPlayers();

            var players = _ecsContextProvider.Context.player.GetEntities().ToList();
            players.Sort((a, b) => 
                    (int)(a.playerId.Value.EntityID - b.playerId.Value.EntityID));
            
            for (int i = 0; i < Mathf.Min(_gameViewController.WorldPlayerPlaceControllers.Count, players.Count); i++)
            {
                var control = _gameViewController.WorldPlayerPlaceControllers[i];
                control.Link(players[i]);
            }
    
            
            for (int j = 0; j < spawnedPlayers.Count; j++)
            {
                var playerEntity = players[j];
                var controller = spawnedPlayers[j];
                _playerSpawnService.Link(playerEntity, controller);
            }
        }
        
        public void Link(Player[] punPlayers)
        {
            _gameViewController.LinkAllPlayers();
        
            var players = _ecsContextProvider.Context.player.GetEntities().ToList();
            players.Sort((a, b) => 
                    (int)(a.playerId.Value.EntityID - b.playerId.Value.EntityID));
            
            for (int i = 0; i < Mathf.Min(_gameViewController.WorldPlayerPlaceControllers.Count, players.Count); i++)
            {
                var control = _gameViewController.WorldPlayerPlaceControllers[i];
                control.Link(players[i]);
            }


            for (int j = 0; j < punPlayers.Length; j++)
            {
                var playerPun = punPlayers[j];
                var playerEntity = players[j];
                var controller = GetMainPhotonView(playerPun);
                _playerSpawnService.Link(playerEntity, controller);
            }
        }
        
        public static List<PhotonView> GetAllPhotonViewsPerPlayer(Player player)
        {
            if (player != null)
            {
                List<PhotonView> list = new List<PhotonView>();
                int actorNr = player.ActorNumber;
                for(int viewId = actorNr * PhotonNetwork.MAX_VIEW_IDS + 1; viewId < (actorNr + 1) * PhotonNetwork.MAX_VIEW_IDS; viewId++)
                {
                    PhotonView photonView = PhotonView.Find(viewId);
                    if (photonView)
                    {
                        list.Add(photonView);
                    }
                }
                return list;
            }
            return null;
        }
        
        public static PlayerController GetMainPhotonView(Player player)
        {
            var list = GetAllPhotonViewsPerPlayer(player);
            return list
                .Select(x => x.GetComponent<DummyPlayer>()?.View.GetComponent<PlayerController>())
                .FirstOrDefault(x => x != null);
        }
    }
}