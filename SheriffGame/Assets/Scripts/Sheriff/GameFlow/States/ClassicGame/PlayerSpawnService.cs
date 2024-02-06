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
            Quaternion rotation = Quaternion.identity;
            ;
            if (playerEntity.hasPlayerPositionId)
            {
                var spawnController =
                    _gameViewController.WorldPlayerPlaceControllers.ElementAtOrDefault(playerEntity.playerPositionId
                        .Id);
                spawnPoint = spawnController.SpawnPoint.position + Vector3.up * 5f;
                rotation = spawnController.SpawnPoint.rotation;
            }
            else
            {
                spawnIndex++;
                var spawnController = _gameViewController.WorldPlayerPlaceControllers.ElementAtOrDefault(spawnIndex);
                spawnPoint = spawnController.SpawnPoint.position + Vector3.up * 5f;
                rotation = spawnController.SpawnPoint.rotation;
            }

            var instance = Instantiate(prefab, spawnPoint, rotation);
            instance.GetComponent<PlayerController>().enabled = isMain;
            playerEntity.ReplacePlayerController(instance.GetComponent<PlayerController>());
            instance.GetComponent<Character>().SetPlayerId(playerEntity.playerId.Value.EntityID);
            _container.InjectGameObject(instance);
        }
    }
}