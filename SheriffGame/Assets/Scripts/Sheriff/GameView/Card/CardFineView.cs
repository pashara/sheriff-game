using TMPro;
using UnityEngine;

namespace Sheriff.GameView
{
    public class CardFineView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;


        public void SetFine(int fine)
        {
            label.SetText($"{fine}");
        }
    }
}