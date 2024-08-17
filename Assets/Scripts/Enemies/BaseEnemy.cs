using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int attackPower;

    [SerializeField]
    protected CircleCollider2D targetDetectionCollider;

    [SerializeField]
    protected Vector2 attackRangePositionOffset;
    protected Vector2 attackRangePosition => (Vector2)transform.position + attackRangePositionOffset;
    [SerializeField]
    protected Vector2 attackRangeSize;

    [SerializeField]
    protected float attackCooldown;  // Time between attacks
    
    protected float cooldownTimer;   // Timer to track the cooldown

    protected bool isAttacking;

    protected Transform target;

    public BaseEnemy(int health, int attackPower, float attackCooldown)
    {
        this.health = health;
        this.attackPower = attackPower;
        this.attackCooldown = attackCooldown;
        this.cooldownTimer = 0f;  // Start with no cooldown
    }

    public virtual void UpdateCooldown(float deltaTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
        }
    }

    protected virtual void RestartCooldown()
    {
        cooldownTimer = attackCooldown;
    }

    protected virtual void Update()
    {
        UpdateCooldown(Time.deltaTime);

        if(CanAttack())
        {
            Attack();
        }
    }

    public virtual bool CanAttack()
    {
        return cooldownTimer <= 0 && target != null && !isAttacking;
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Attack();

    public virtual void Die()
    {
        // Default death behavior
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if(targetDetectionCollider != null)
        {
            Gizmos.DrawWireSphere(targetDetectionCollider.transform.position, targetDetectionCollider.radius);
        }
        Gizmos.DrawWireCube(attackRangePosition, attackRangeSize);
    }
}