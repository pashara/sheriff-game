using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sheriff.GameFlow.PlayerUIControls.PlayersList
{
    public class PlayerListElementUI : MonoBehaviour, IDisposable
    {
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private GameObject sheriffRoot;
        [SerializeField] private TMP_Text allowedCardsTransferred;
        [SerializeField] private TMP_Text smuggledCardsTransferred;
        [SerializeField] private TMP_Text cash;
        
        private readonly CompositeDisposable _disposable = new();
        
        
        public void Initialize(PlayerEntity playerEntity)
        {
            nameLabel.SetText(playerEntity.playerId.Value.ToString());
            playerEntity.OnSheriff().Subscribe(x =>
            {
                sheriffRoot.SetActive(x != null);
            }).AddTo(_disposable);

            playerEntity.OnTransferredResources().Subscribe(x =>
            {
                var allowed = x?.Value.AllowedResources?.Select(y => y.Value).Sum() ?? 0;
                var smuggled = x?.Value.NotAllowedResources?.Select(y => y.Value).Sum() ?? 0;
                Fill(allowed, smuggled);
            }).AddTo(_disposable);

            playerEntity.OnGoldCashCurrency().Subscribe(x =>
            {
                cash.SetText(x == null ? "$0" : $"${x.Value}");
            }).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Clear();
            Destroy(gameObject);
        }
        
        private void Fill(int allowed, int smuggled)
        {
            allowedCardsTransferred.SetText($"x{allowed}");
            smuggledCardsTransferred.SetText($"x{smuggled}");
        }
    }
}