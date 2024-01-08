using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheriff.GameStructures
{
    [CreateAssetMenu]
    public class CardInfoSO : ScriptableObject
    {
        [HideLabel]
        [SerializeField] private CardInfo cardInfo;

        public CardInfo CardInfo => cardInfo;
    }
}