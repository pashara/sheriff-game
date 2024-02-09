using UnityEngine;

namespace NaughtyCharacter
{
    public abstract class Controller : MonoBehaviour
    {
        public Character Character { get; set; }

        public abstract void Init();
        public abstract void OnCharacterUpdate();
        public abstract void OnCharacterFixedUpdate();
    }
}
