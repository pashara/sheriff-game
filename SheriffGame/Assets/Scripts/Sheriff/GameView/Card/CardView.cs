using UnityEngine;

namespace Sheriff.GameView
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Transform innerViewRoot;
        [SerializeField] private Transform cardShirtRoot;

        public CardEntity CardEntity { get; private set; }

        public void Link(CardEntity cardEntity)
        {
            CardEntity = cardEntity;
        }

        
        public void SetShirt(ShirtView shirtView)
        {
            shirtView.transform.SetParent(cardShirtRoot);
            shirtView.transform.localPosition = Vector3.zero;
            shirtView.transform.localRotation = Quaternion.identity;
            shirtView.transform.localScale = Vector3.one;
        }

        public void SetInner(CardInnerView innerView)
        {
            innerView.transform.SetParent(innerViewRoot);
            innerView.transform.localPosition = Vector3.zero;
            innerView.transform.localRotation = Quaternion.identity;
            innerView.transform.localScale = Vector3.one;
        }
    }
}