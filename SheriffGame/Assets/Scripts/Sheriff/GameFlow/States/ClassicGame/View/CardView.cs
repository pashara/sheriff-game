using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.View
{
    public class CardView : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text cost;
        [SerializeField] private TMP_Text fee;
        [SerializeField] private GameObject openedRoot;
        [SerializeField] private GameObject closedRoot;

        [SerializeField] private Transform localRoot;

        public Transform LocalRoot => localRoot;
        
        public void Link(CardEntity cardEntity)
        {
            gameObject.name = $"Card in dec #{cardEntity.cardId.Value}";
            // cardEntity.OnInDec().Select(x => x != null).Subscribe(x =>
            // {
            //     // gameObject.SetActive(x);
            // }).AddTo(this);
        }


        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}