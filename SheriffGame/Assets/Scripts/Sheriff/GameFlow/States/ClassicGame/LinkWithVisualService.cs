using System.Collections.Generic;
using System.Linq;
using NaughtyCharacter;
using Photon.Pun;
using Photon.Realtime;
using Sheriff.ClientServer;
using Sheriff.ClientServer.Players;
using Sheriff.ECS;
using Sheriff.GameFlow.Players;
using Sheriff.GameFlow.States.ClassicGame.View;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class LinkWithVisualService
    {
        private readonly GameViewController _gameViewController;
        private readonly PlayerSpawnService _playerSpawnService;
        private readonly PlayersAssociations _playersAssociations;
        private readonly EcsContextProvider _ecsContextProvider;

        public LinkWithVisualService(
            GameViewController gameViewController, 
            PlayerSpawnService playerSpawnService,
            PlayersAssociations playersAssociations,
            EcsContextProvider ecsContextProvider)
        {
            _gameViewController = gameViewController;
            _playerSpawnService = playerSpawnService;
            _playersAssociations = playersAssociations;
            _ecsContextProvider = ecsContextProvider;
        }
        
        public void Link(List<PlayerController> spawnedPlayers)
        {
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
                controller.Link(playerEntity);
                _playerSpawnService.Link(playerEntity, controller);
            }
        }
        
        public void LinkMaster(Player[] punPlayers)
        {
            var players = _ecsContextProvider.Context.player.GetEntities().ToList();
            players.Sort((a, b) => 
                    (int)(a.playerId.Value.EntityID - b.playerId.Value.EntityID));
            
            for (int i = 0; i < Mathf.Min(_gameViewController.WorldPlayerPlaceControllers.Count, players.Count); i++)
            {
                var control = _gameViewController.WorldPlayerPlaceControllers[i];
                players[i].ReplaceWorldPlayerPlaceController(control);
                players[i].ReplacePlayerNetworkId(punPlayers[i].NickName);
                players[i].ReplaceNickname(punPlayers[i].NickName);
                
                control.Link(players[i]);
            }


            for (int j = 0; j < punPlayers.Length; j++)
            {
                var playerPun = punPlayers[j];
                var playerEntity = players[j];
                
                var data = _playersAssociations[playerPun];
                data.playerEntity = playerEntity;
                data.playerController.Link(data.playerEntity);
                _playerSpawnService.Link(playerEntity, data.playerController);
            }
        }
        
        public void LinkSlave(Player[] punPlayers)
        {
            var players = _ecsContextProvider.Context.player.GetEntities().ToList();
            players.Sort((a, b) => 
                (int)(a.playerId.Value.EntityID - b.playerId.Value.EntityID));
            
        
            for (int i = 0; i < Mathf.Min(_gameViewController.WorldPlayerPlaceControllers.Count, players.Count); i++)
            {
                var control = _gameViewController.WorldPlayerPlaceControllers[i];
                players[i].ReplaceWorldPlayerPlaceController(control);
                control.Link(players[i]);
            }


            for (int j = 0; j < punPlayers.Length; j++)
            {
                var playerPun = punPlayers[j];
                var playerEntity = _ecsContextProvider.Context.player.GetEntityWithPlayerNetworkId(playerPun.NickName);
                
                var data = _playersAssociations[playerPun];
                data.playerEntity = playerEntity;
                data.playerController.Link(data.playerEntity);
                _playerSpawnService.Link(playerEntity, data.playerController);
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
        
        public static PlayerController GetPlayerController(Player player)
        {
            return GetMainPhotonView(player).GetComponent<DummyPlayer>()?.View.GetComponent<PlayerController>();
        }
        public static PhotonView GetMainPhotonView(Player player)
        {
            var list = GetAllPhotonViewsPerPlayer(player);
            return list.FirstOrDefault(x => x.GetComponent<DummyPlayer>() != null);
        }
    }
}