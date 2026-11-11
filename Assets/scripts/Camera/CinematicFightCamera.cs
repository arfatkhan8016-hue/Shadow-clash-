using UnityEngine;

namespace ShadowClash
{
    public sealed class CinematicFightCamera : MonoBehaviour
    {
        [SerializeField] private Transform fighterA;
        [SerializeField] private Transform fighterB;
        [SerializeField] private Vector3 offset = new Vector3(0f, 3.2f, -7f);
        [SerializeField] private float followSharpness = 5f;
        [SerializeField] private float lookHeight = 1.2f;
        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 9f;

        private void LateUpdate()
        {
            if (!fighterA || !fighterB) return;

            Vector3 midpoint = (fighterA.position + fighterB.position) * 0.5f;
            Vector3 between = fighterB.position - fighterA.position;
            between.y = 0f;
            float distance = Mathf.Clamp(between.magnitude, minDistance, maxDistance);

            Vector3 backward = -between.normalized;
            Vector3 desired = midpoint + backward * distance + Vector3.up * offset.y;
            transform.position = Vector3.Lerp(
                transform.position, desired, followSharpness * Time.deltaTime);

            Vector3 lookAt = midpoint + Vector3.up * lookHeight;
            Quaternion rotation = Quaternion.LookRotation(lookAt - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, rotation, followSharpness * Time.deltaTime);
        }

        public void SetFighters(Transform a, Transform b)
        {
            fighterA = a;
            fighterB = b;
        }
    }
}
