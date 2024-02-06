using System;
using Sheriff.DataBase;
using Sheriff.GameStructures;
using TMPro;
using UnityEngine;
using Zenject;

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

        [Inject] private ICardConfigProvider _cardConfigProvider;
        private CardInfo _cardConfig;

        public Transform LocalRoot => localRoot;
        public CardEntity CardEntity { get; private set; }
        
        public void Link(CardEntity cardEntity)
        {
            CardEntity = cardEntity;
            gameObject.name = $"Card in dec #{cardEntity.cardId.Value}";
            _cardConfig = _cardConfigProvider.Get(cardEntity.resourceType.Value);
            FillTitle();
            FillCost();
            FillFee();
            // cardEntity.OnInDec().Select(x => x != null).Subscribe(x =>
            // {
            //     // gameObject.SetActive(x);
            // }).AddTo(this);
        }


        void FillTitle()
        {
            title.SetText(_cardConfig.Title);
        }

        void FillCost()
        {
            cost.SetText($"+{_cardConfig.Cost} coins");
        }

        void FillFee()
        {
            cost.SetText($"-{_cardConfig.Fine} coins");
        }
        


        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}