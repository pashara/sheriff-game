using UnityEngine;

namespace Sheriff.GameFlow.Players
{
    public class PlayerWorldIndicatorRotateHandler : MonoBehaviour
    {
        void Update()
        {
            if (Camera.main != null)
            {
                // Получаем вектор направления от объекта к камере
                Vector3 directionToCamera = Camera.main.transform.position - transform.position;

                // Вычисляем угол поворота вокруг оси y
                float targetAngle = Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;

                // Создаем целевую ориентацию вращения
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                // Интерполируем текущую ориентацию к целевой
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
            }
            else
            {
                Debug.LogError("Camera.Main not found. Make sure your scene has a main camera.");
            }
        }
    }
}
