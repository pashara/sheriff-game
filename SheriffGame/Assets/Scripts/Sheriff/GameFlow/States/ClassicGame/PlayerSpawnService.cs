using System.Linq;
using NaughtyCharacter;
using Sheriff.GameFlow.States.ClassicGame.View;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class PlayerSpawnService : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [Inject] private GameViewController _gameViewController;
        [Inject] private DiContainer _container;

        private int spawnIndex = -1;
        
        public void Spawn(PlayerEntity playerEntity, bool isMain)
        {
            Vector3 spawnPoint = Vector3.zero;
            if (playerEntity.hasPlayerPositionId)
            {
                var spawnController =
                    _gameViewController.WorldPlayerCardsControllers.ElementAtOrDefault(playerEntity.playerPositionId
                        .Id);
                spawnPoint = spawnController.transform.position + Vector3.up * 5f;
            }
            else
            {
                spawnIndex++;
                var spawnController = _gameViewController.WorldPlayerCardsControllers.ElementAtOrDefault(spawnIndex);
                spawnPoint = spawnController.transform.position + Vector3.up * 5f;
            }

            var instance = Instantiate(prefab, spawnPoint, Quaternion.identity);
            instance.GetComponent<PlayerController>().enabled = isMain;
            _container.InjectGameObject(instance);
        }
    }
}