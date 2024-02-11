using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NaughtyCharacter
{
    public static class CharacterAnimatorParamId
    {
        public static readonly int HorizontalSpeed = Animator.StringToHash("HorizontalSpeed");
        public static readonly int VerticalSpeed = Animator.StringToHash("VerticalSpeed");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    }

    public class HorizontalSpeedFilter
    {
        private const int FrameBufferSize = 5;
        private Queue<float> horizontalSpeedBuffer = new Queue<float>();

        public float UpdateState(float horizontalSpeed)
        {
            // Добавляем текущее значение горизонтальной скорости в буфер
            horizontalSpeedBuffer.Enqueue(horizontalSpeed);

            // Удостоверяемся, что буфер не превышает заданный размер
            while (horizontalSpeedBuffer.Count > FrameBufferSize)
            {
                horizontalSpeedBuffer.Dequeue();
            }

            // Вычисляем среднее значение горизонтальной скорости
            float averageHorizontalSpeed = horizontalSpeedBuffer.Sum() / horizontalSpeedBuffer.Count;

            return averageHorizontalSpeed;
        }
    }
    
    public class CharacterAnimator  : MonoBehaviour
    {
        private Animator _animator;
        private Character _character;
        private HorizontalSpeedFilter _horizontalSpeedFilter;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _horizontalSpeedFilter = new HorizontalSpeedFilter();
        }

        public void UpdateState()
        {
            float normHorizontalSpeed = _character.HorizontalVelocity.magnitude / _character.MovementSettings.MaxHorizontalSpeed;
            float filteredHorizontalSpeed = _horizontalSpeedFilter.UpdateState(normHorizontalSpeed);
            _animator.SetFloat(CharacterAnimatorParamId.HorizontalSpeed, filteredHorizontalSpeed);

            float jumpSpeed = _character.MovementSettings.JumpSpeed;
            float normVerticalSpeed = _character.VerticalVelocity.y.Remap(-jumpSpeed, jumpSpeed, -1.0f, 1.0f);
            _animator.SetFloat(CharacterAnimatorParamId.VerticalSpeed, normVerticalSpeed);

            _animator.SetBool(CharacterAnimatorParamId.IsGrounded, _character.IsGrounded);
        }
    }
}
