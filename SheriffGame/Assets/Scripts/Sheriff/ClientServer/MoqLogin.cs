using Cysharp.Threading.Tasks;
using Sheriff.Bootstrap;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Sheriff.ClientServer
{
    public class MoqLogin : MonoBehaviour
    {
        [Inject]
        async void Construct(IGameSessionDataProvider gameSessionDataProvider)
        {
            await UniTask.DelayFrame(50);

            gameSessionDataProvider.Set(false);
            SceneManager.LoadScene("GameScene");
        }
    }
}
