using TMPro;
using UniRx;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Declares
{
    public class SheriffDeclaredResultViewUI : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new();
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text coinsLabel;

        public void Show(
            PlayerEntity playerEntity
        )
        {
            _disposable.Clear();
            playerEntity.OnSheriffCheckResult().Skip(1).Subscribe(x =>
            {
                ApplyView(playerEntity, false);
            }).AddTo(_disposable);

            ApplyView(playerEntity, true);
        }

        public void Hide()
        {
            _disposable.Clear();
            ClearView();
        }


        void ApplyView(PlayerEntity playerEntity, bool isImmediately)
        {
            if (!playerEntity.hasSheriffCheckResult)
            {
                ClearView();
            }
            else
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("isImmediately", isImmediately);
                if (playerEntity.sheriffCheckResult.Value is SkipCheckSherifResult)
                {
                    animator.SetTrigger("skip");
                }
                else if (playerEntity.sheriffCheckResult.Value is SherifLooseCheckResult sherifLoose)
                {
                    animator.SetTrigger("sheriff_lose");
                    coinsLabel.SetText($"{sherifLoose.Coins}");
                }
                else if (playerEntity.sheriffCheckResult.Value is DealerLooseCheckResult sheriffWin)
                {
                    animator.SetTrigger("sheriff_win");
                    coinsLabel.SetText($"{sheriffWin.Coins}");
                }
                else
                {
                    ClearView();
                }
            }
        }

        void ClearView()
        {
            animator.gameObject.SetActive(false);
        }
    }
}