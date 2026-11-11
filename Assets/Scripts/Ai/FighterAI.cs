using UnityEngine;

namespace ShadowClash
{
    public sealed class FighterAI : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private FighterCombat combat;
        [SerializeField] private FighterHealth health;
        [SerializeField] private float preferredRange = 1.6f;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float attackInterval = 1.1f;
        [SerializeField] private float blockChance = 0.18f;

        private float nextAction;
        private CharacterController controller;

        private void Awake() => controller = GetComponent<CharacterController>();

        private void Update()
        {
            if (!player || (health && health.IsDefeated)) return;

            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;
            if (toPlayer.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, Quaternion.LookRotation(toPlayer),
                    10f * Time.deltaTime);

            if (distance > preferredRange)
            {
                Vector3 step = toPlayer.normalized * moveSpeed;
                if (controller) controller.Move(step * Time.deltaTime);
                else transform.position += step * Time.deltaTime;
                combat?.SetBlocking(false);
            }
            else if (Time.time >= nextAction)
            {
                nextAction = Time.time + attackInterval + Random.Range(0f, 0.35f);
                bool block = Random.value < blockChance;
                combat?.SetBlocking(block);
                if (!block)
                {
                    if (Random.value < 0.65f) combat?.LightAttack();
                    else combat?.HeavyAttack();
                }
            }
        }

        public void SetPlayer(Transform value) => player = value;
    }
}
