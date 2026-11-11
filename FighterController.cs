
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FighterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;
    public float gravity = -20f;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 0.6f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    [Header("Animation")]
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private float nextAttackTime;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        HandleAttack();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, z).normalized;

        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * moveSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );

            if (animator != null)
                animator.SetFloat("Speed", direction.magnitude);
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.J) &&
            Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (animator != null)
                animator.SetTrigger("Punch");

            Collider[] enemies = Physics.OverlapSphere(
                attackPoint.position,
                attackRange,
                enemyLayer
            );

            foreach (Collider enemy in enemies)
            {
                FighterHealth health =
                    enemy.GetComponentInParent<FighterHealth>();

                if (health != null)
                    health.TakeDamage(attackDamage);
            }
        }
    }
}
