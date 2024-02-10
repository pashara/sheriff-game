using Sheriff.Bootstrap;
using UnityEngine;
using Zenject;

namespace Sheriff.ClientServer.Lobby
{
    public class NetworkLoginConfigureHandler : MonoBehaviour
    {
        [Inject]
        private void Construct(IGameSessionDataProvider gameSessionDataProvider)
        {
            gameSessionDataProvider.Set(true);
        }
    }
}