using Sheriff.GameFlow.States.ClassicGame.World.Cards;
using Sheriff.GameFlow.States.ClassicGame.World.Declares;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World
{
    public class WorldPlayerPlaceControllers : MonoBehaviour
    {
        [SerializeField] private DeclaredBagWorldUI declaredController;
        [SerializeField] private WorldPlayerCardsController cardsController;
        [SerializeField] private Transform spawnPoint;
        
        public DeclaredBagWorldUI DeclaredBagWorldUI => declaredController;
        public WorldPlayerCardsController CardsController => cardsController;

        public Transform SpawnPoint => spawnPoint;


        public void Link(PlayerEntity player)
        {
            declaredController.Link(player);
            cardsController.Link(player);
        }
    }
}