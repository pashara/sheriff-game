﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using NaughtyCharacter;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Sheriff.ClientServer.Players;
using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.World;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Sheriff.ClientServer.Game
{
    public class PunGameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<WorldPlayerPlaceControllers> worldPlayerPlacementControllers;
        [SerializeField] private ClassicGameControllerWrapper classicGameControllerWrapper;
        [Inject] private DiContainer _container;
        [Inject] private EcsContextProvider _ecsContextProvider;


        [Button]
        public void MarkGameReady()
        {
            Hashtable props = new Hashtable
            {
                {SheriffGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            
            
            
            var spawnPoint = worldPlayerPlacementControllers[PhotonNetwork.LocalPlayer.GetPlayerNumber()].SpawnPoint;

            var instance = PhotonNetwork.Instantiate("DummyPlayer", spawnPoint.position, spawnPoint.rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)

        }
        
        
        


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
        }


        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                // StartCoroutine(SpawnAsteroid());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(SheriffGame.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(SheriffGame.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    if (!startTimeIsSet)
                    {
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    // not all players loaded yet. wait:
                    // Debug.Log("setting text waiting for players! ",this.InfoText);
                    // InfoText.text = "Waiting for other players...";
                }
            }
        
        }
        
        

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(SheriffGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool) playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(SheriffGame.PLAYER_LIVES, out lives))
                {
                    if ((int) lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));
            }
        }
        
        

        private IEnumerator EndOfGame(string winner, int score)
        {
            // float timer = 5.0f;
            //
            // while (timer > 0.0f)
            // {
            //     InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));
            //
            //     yield return new WaitForEndOfFrame();
            //
            //     timer -= Time.deltaTime;
            // }

            yield return PhotonNetwork.LeaveRoom();
        }
        
        
        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
        
        
        
        public override async void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;

            await UniTask.DelayFrame(1);
            
            MarkGameReady();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }


        [SerializeField] private GameStateSerializer _gameStateSerializer;
        
        
        [PunRPC]
        private void SetProjectState(Byte[] jsonArray)
        {
            var jsonGameState = Encoding.UTF8.GetString(jsonArray);
            var json = new LoadedSessionDataProvider(jsonGameState);
            
            var data = json.GetLoadData();
            _ecsContextProvider.FillData(data);
            classicGameControllerWrapper.StartGame(json, PhotonNetwork.PlayerList);
        }
        
        
        private async void StartGame()
        {
            Debug.Log("StartGame!");

            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate

            if (PhotonNetwork.IsMasterClient)
            {
                classicGameControllerWrapper.StartGame(PhotonNetwork.PlayerList);
                await UniTask.DelayFrame(2);
                var json = _gameStateSerializer.Serialize();
                var array = Encoding.UTF8.GetBytes(json);
                photonView.RPC(nameof(SetProjectState), RpcTarget.Others, array);
            }
        }
    }
}