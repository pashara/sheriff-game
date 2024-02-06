using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class InteractableCardProvider : MonoBehaviour, ICardInteractable
    {
        [SerializeField] private CardView cardView;
        public CardView CardView => cardView;
    }
}