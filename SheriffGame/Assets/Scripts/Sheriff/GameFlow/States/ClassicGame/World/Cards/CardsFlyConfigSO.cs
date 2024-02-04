using UnityEngine;

namespace Sheriff.GameFlow.States.ClassicGame.World.Cards
{
    [CreateAssetMenu]
    public class CardsFlyConfigSO : ScriptableObject
    {
        public AnimationCurve xMoveCurve;
        public AnimationCurve yMoveCurve;

        public float moveDelay;
        public float moveDuration;

        public float rotationDelay;
        public float rotationDuration;
        
        public AnimationCurve scaleCurve;
        public float scaleDuration;
        public float scaleDelay;

        public float changeRootDelay;
    }
}