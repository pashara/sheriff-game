using Photon.Pun;
using Sheriff.Bootstrap;
using UniRx;
using UnityEngine;
using Zenject;

namespace Sheriff.ClientServer
{
    public class EnableIfMasterClient : MonoBehaviour
    {
        [InjectOptional] private IGameSessionDataProvider _gameSessionDataProvider;
        private void Awake()
        {
            Observable.EveryUpdate().Subscribe(x =>
            {
                SetActiveSafe(_gameSessionDataProvider is not { IsNetwork: true } || PhotonNetwork.IsMasterClient);
            }).AddTo(this);
        }


        private void SetActiveSafe(bool isActive)
        {
            switch (gameObject.activeSelf)
            {
                case true when !isActive:
                    gameObject.SetActive(false);
                    break;
                case false when isActive:
                    gameObject.SetActive(true);
                    break;
            }
        }
    }
}
