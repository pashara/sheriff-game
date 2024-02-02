using Sheriff.GameFlow.States.ClassicGame.View.Player;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class PlayerViewController : MonoBehaviour
    {
        public void Link(PlayerEntity playerEntity)
        {
            GetComponent<PlayerEditorComponents>().Link(playerEntity);
            gameObject.name = $"Player {playerEntity.playerId.Value}";
        }
    }
}