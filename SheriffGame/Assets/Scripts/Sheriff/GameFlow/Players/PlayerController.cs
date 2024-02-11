using Sheriff.Pers;
using TMPro;
using UniRx;
using UnityEngine;

namespace Sheriff.GameFlow.Players
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour thirdPartyController;
        [SerializeField] private CharacterView characterView;
        [SerializeField] private TMP_Text nickLabel;
        [SerializeField] private GameObject root;
        [SerializeField] private GameObject sheriffRoot;
        private readonly CompositeDisposable _disposable = new();

        public CharacterView CharacterView => characterView;

        
        public void Link(PlayerEntity playerEntity)
        {
            _disposable.Clear();
            playerEntity.OnSheriff().Subscribe(x =>
            {
                characterView.MakeSheriff(x != null);
                sheriffRoot.SetActive(x != null);
            }).AddTo(_disposable);
            
            playerEntity.OnNickname().Subscribe(x =>
            {
                nickLabel.SetText(x == null ? $"{playerEntity.playerId.Value}" : $"{x.Value}");
            }).AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable.Clear();
        }

        private void OnEnable()
        {
            root.gameObject.SetActive(false);
            thirdPartyController.enabled = true;
        }

        private void OnDisable()
        {
            root.gameObject.SetActive(true);
            thirdPartyController.enabled = false;
        }
    }
}