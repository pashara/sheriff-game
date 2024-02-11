using System;
using Sheriff.DataBase;
using Sheriff.GameResources;
using Sheriff.GameStructures;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.View.UI
{
    public class CardViewUI : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text cardsCountLabel;
        [SerializeField] private TMP_Text cost;
        [SerializeField] private TMP_Text fee;
        [SerializeField] private GameResourceType gameResourceType;
        [Inject] private ICardConfigProvider _cardConfigProvider;
        private CardInfo _cardConfig;

        public void Initialize()
        {
            _cardConfig = _cardConfigProvider.Get(gameResourceType);
            FillCost();
            FillFee();
        }

        public void ApplyCount(int count)
        {
            cardsCountLabel.SetText(count <= 1 ? "" : $"x{count}");
        }


        private void FillCost()
        {
            cost.SetText($"+{_cardConfig.Cost} coins");
        }

        private void FillFee()
        {
            fee.SetText($"-{_cardConfig.Fine} coins");
        }


        public void Dispose()
        {
            GameObject.Destroy(gameObject);
        }
    }
}