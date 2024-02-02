using UniRx;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class CardView : MonoBehaviour
    {
        
        public void Link(CardEntity cardEntity)
        {
            gameObject.name = $"Card in dec #{cardEntity.cardId.Value}";
            cardEntity.OnInDec().Select(x => x != null).Subscribe(x =>
            {
                gameObject.SetActive(x);
            }).AddTo(this);
        }
    }
}