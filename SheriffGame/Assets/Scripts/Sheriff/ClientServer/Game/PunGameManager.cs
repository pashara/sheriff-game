using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Sheriff.ECS;
using Sheriff.GameFlow;
using Sheriff.GameFlow.States.ClassicGame;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.GameFlow.States.ClassicGame.World;
using Sheriff.Pers;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Sheriff.ClientServer.Game
{

    public interface IPunSender
    {
        void SendCommandToMaster(string json);
        IReadOnlyReactiveCollection<string> ReceivedFromMasterCommands { get; }
        IReadOnlyReactiveCollection<string> ReceivedFromSlaveCommands { get; }
        void NotifyCommand(string serialize);
        void SendInitialGameState();
        void SendGameState();
        void IncView();
        void DecView();
    }
    
    public class PunGameManager : MonoBehaviourPunCallbacks, IPunSender
    {
        [SerializeField] private ClassicGameControllerWrapper classicGameControllerWrapper;
        [Inject] private GameViewController _gameViewController;
        [Inject] private DiContainer _container;
        [Inject] private PlayersAssociations _playersAssociations;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [SerializeField] private Button readyButton;
        [SerializeField] private GameObject readyUI;

        [Button]
        public void MarkGameReady()
        {
            Hashtable props = new Hashtable
            {
                {SheriffGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }


        void MarkLoaded()
        {
            Hashtable props = new Hashtable
            {
                {SheriffGame.PLAYER_LOADED, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        
        
        


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            AddPlayer(newPlayer);

        }

        void AddPlayer(Player newPlayer)
        {
            
            var photonView = LinkWithVisualService.GetMainPhotonView(newPlayer);
            var playerController = LinkWithVisualService.GetPlayerController(newPlayer);

            _playersAssociations[newPlayer] = new()
            {
                punPlayer = newPlayer,
                photonView = photonView,
                playerController = playerController,
            };
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
            
            
            if (changedProps.ContainsKey(SheriffGame.PLAYER_LOADED))
            {
                AddPlayer(targetPlayer);
                return;
            }

            if (changedProps.TryGetValue(SheriffGame.PLAYER_VIEW_ID, out var viewIdObj) && viewIdObj is int viewIdInt)
            {
                var playerController = _playersAssociations[targetPlayer].playerController;
                var view = playerController.GetComponent<CharacterView>();
                view?.Apply(viewIdInt);
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
            readyUI.SetActive(false);
            await UniTask.DelayFrame(1);

            
            
            var spawnPoint = _gameViewController.WorldPlayerPlaceControllers[PhotonNetwork.LocalPlayer.GetPlayerNumber()].SpawnPoint;

            PhotonNetwork.Instantiate("DummyPlayer", spawnPoint.position, spawnPoint.rotation, 0);
            // avoid this call on rejoin (ship was network instantiated before)
            readyUI.SetActive(true);


            readyButton.onClick.AsObservable().Subscribe(x =>
            {
                MarkGameReady();
                readyUI.SetActive(false);
            }).AddTo(this);

            MarkLoaded();
            // MarkGameReady();
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
            classicGameControllerWrapper.StartGame(data, PhotonNetwork.PlayerList);
        }
        
        [PunRPC]
        private void SetActualProjectState(Byte[] jsonArray)
        {
            var jsonGameState = Encoding.UTF8.GetString(jsonArray);
            var json = new LoadedSessionDataProvider(jsonGameState);
            
            var data = json.GetLoadData();
            _ecsContextProvider.FillData(data);
            classicGameControllerWrapper.ApplyGame(data);
        }
        
        
        private async void StartGame()
        {
            Debug.Log("StartGame!");

            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate

            if (PhotonNetwork.IsMasterClient)
            {
                classicGameControllerWrapper.StartGame(PhotonNetwork.PlayerList);
            }
        }

        public void SendInitialGameState()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var json = _gameStateSerializer.Serialize();
                var array = Encoding.UTF8.GetBytes(json);
                photonView.RPC(nameof(SetProjectState), RpcTarget.Others, array);
            }
        }

        public void SendGameState()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var json = _gameStateSerializer.Serialize();
                var array = Encoding.UTF8.GetBytes(json);
                photonView.RPC(nameof(SetActualProjectState), RpcTarget.Others, array);
            }
        }

        public void IncView()
        {
            Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
            int id = 0;
            if (props.ContainsKey(SheriffGame.PLAYER_VIEW_ID))
            {
                id = (int) props[SheriffGame.PLAYER_VIEW_ID];
            }
            id++;
            id = Mathf.Max(0, id);
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{SheriffGame.PLAYER_VIEW_ID, id}});
        }

        public void DecView()
        {
            Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
            int id = 0;
            if (props.ContainsKey(SheriffGame.PLAYER_VIEW_ID))
            {
                id = (int) props[SheriffGame.PLAYER_VIEW_ID];
            }
            id--;
            id = Mathf.Max(0, id);
            
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{SheriffGame.PLAYER_VIEW_ID, id}});
        }






        public void SendCommandToMaster(string json)
        {
            var array = Encoding.UTF8.GetBytes(json);
            photonView.RPC(nameof(OnReceiveCommandFromSlave), RpcTarget.MasterClient, array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">serialized CommandData</param>
        public void NotifyCommand(string json)
        {
            var array = Encoding.UTF8.GetBytes(json);
            photonView.RPC(nameof(OnReceiveCommand), RpcTarget.All, array);
        }


        private readonly ReactiveCollection<string> _receivedFromMasterCommands = new();
        private readonly ReactiveCollection<string> _receivedFromSlaveCommands = new();
        public IReadOnlyReactiveCollection<string> ReceivedFromMasterCommands => _receivedFromMasterCommands;
        public IReadOnlyReactiveCollection<string> ReceivedFromSlaveCommands => _receivedFromSlaveCommands;
        
        [PunRPC]
        private void OnReceiveCommand(Byte[] jsonArray)
        {
            var commandJson = Encoding.UTF8.GetString(jsonArray);
            _receivedFromMasterCommands.Add(commandJson);
        }
        [PunRPC]
        private void OnReceiveCommandFromSlave(Byte[] jsonArray)
        {
            var commandJson = Encoding.UTF8.GetString(jsonArray);
            _receivedFromSlaveCommands.Add(commandJson);
        }

    }
}