using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    public class CardFlyController : MonoBehaviour
    {
        [SerializeField] private CardView editorSource;
        [SerializeField] private GridProvider playerCards;
        [SerializeField] private GridProvider bagGrid;
        [SerializeField] private GridProvider newCardsDec;
        [SerializeField] private GridProvider cardsDec;

        [SerializeField] private CardsFlyConfigSO flyNewCardConfig;
        [SerializeField] private CardsFlyConfigSO flyBagConfig;
        [SerializeField] private CardsFlyConfigSO flyDecConfig;
        
        
        [Button]
        public async UniTask PlayAppearAnimation(int targetIndex)
        {
            await PlayAppearAnimation(editorSource, targetIndex);
        }
        
        [Button]
        public async UniTask PlayMoveToBag(int targetIndex)
        {
            await PlayMoveToBag(editorSource, targetIndex);
        }
        
        [Button]
        public async UniTask PlaMoveToDec()
        {
            await PlaMoveToDec(editorSource);
        }
        

        public async UniTask PlayAppearAnimation(CardView card, int targetIndex)
        {
            var from = newCardsDec.GetAt(0);
            var to = playerCards.GetAt(targetIndex);

            card.transform.parent = from.target;
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;
            
            await Play(card, to, flyNewCardConfig);
        }

        public async UniTask PlayMoveToBag(CardView card, int targetIndex)
        {
            var to = bagGrid.GetAt(targetIndex);
            
            await Play(card, to, flyBagConfig);
        }

        public async UniTask PlaMoveToDec(CardView card)
        {
            var to = cardsDec.GetAt(0);
            
            await Play(card, to, flyDecConfig);
        }
        
        private async UniTask Play(CardView card, GridProvider.GridPositionInfo target, CardsFlyConfigSO configSo)
        {
            var initialPosition = card.transform.position;
            var initialRotation = card.transform.rotation;
            
            DOTween.To(() => 0f, (t) =>
                {
                    var position = new Vector3(
                        Mathf.LerpUnclamped(initialPosition.x, target.target.position.x,
                            configSo.xMoveCurve.Evaluate(t)),
                        Mathf.LerpUnclamped(initialPosition.y, target.target.position.y,
                            configSo.yMoveCurve.Evaluate(t)),
                        Mathf.LerpUnclamped(initialPosition.z, target.target.position.z, t)
                    );
                    card.transform.position = position;
                }, 1f, configSo.moveDuration)
                .SetEase(Ease.Linear)
                .SetTarget(card)
                .SetDelay(configSo.moveDelay);

            var finalRotation = target.target.rotation;
            card.transform.DORotateQuaternion(finalRotation, configSo.rotationDuration)
                .SetEase(Ease.Linear)
                .SetTarget(card)
                .SetDelay(configSo.rotationDelay);

            var initialLossyScale = card.transform.lossyScale;
            DOTween.To(() => 0f, f =>
                {
                    var targetLossy = target.target.lossyScale;

                    var currentLossy = Vector3.LerpUnclamped(initialLossyScale, targetLossy, configSo.scaleCurve.Evaluate(f));

                    var localScale = CalculateLocalScale(card.transform, currentLossy);
                    card.transform.localScale = localScale;
                }, 1f, configSo.scaleDuration)
                .SetEase(Ease.Linear)
                .SetTarget(card)
                .SetDelay(configSo.scaleDelay);

            DOTween.Sequence().AppendInterval(configSo.changeRootDelay).AppendCallback(() =>
            {
                card.transform.parent = target.target;
            });
        }

        
        // Метод для вычисления localScale по заданному lossyScale
        private static Vector3 CalculateLocalScale(Transform element, Vector3 lossyScale)
        {
            // Получаем родителя объекта
            Transform parentTransform = element.parent;

            // Если у объекта есть родитель
            if (parentTransform != null)
            {
                // Вычисляем масштаб родителя
                Vector3 parentLossyScale = parentTransform.lossyScale;

                // Используем деление элементов векторов для получения localScale
                Vector3 localScale = new Vector3(
                    lossyScale.x / parentLossyScale.x,
                    lossyScale.y / parentLossyScale.y,
                    lossyScale.z / parentLossyScale.z
                );

                return localScale;
            }
            else
            {
                // Если объект не имеет родителя, то localScale равен lossyScale
                return lossyScale;
            }
        }
    }
    
}