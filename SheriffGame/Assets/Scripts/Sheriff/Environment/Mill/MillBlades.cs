using UnityEngine;

namespace Sheriff.Environment.Mill
{
    public class MillBlades : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float speed;
        [SerializeField] private float duration;
        private float _offset;

        private void Awake()
        {
            _offset = UnityEngine.Random.Range(0, duration);
        }


        // Update is called once per frame
        private void Update()
        {
            var t = ((Time.time + _offset) % duration) / duration;
            transform.localRotation *= Quaternion.Euler(0, 0, curve.Evaluate(t) * speed * Time.deltaTime);
        }
    }
}
