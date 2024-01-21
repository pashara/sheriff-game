using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class PlayerViewController : MonoBehaviour
    {
        public void Link(PlayerEntity playerEntity)
        {
            gameObject.name = $"Player {playerEntity.playerId.Value}";
        }

        [Button]
        private void TryGet()
        {
            
        }
    }
}