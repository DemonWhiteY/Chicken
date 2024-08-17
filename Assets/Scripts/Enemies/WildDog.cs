using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildDog : BaseEnemy
{
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private float speed;

    public WildDog(int health, int attackPower, float attackCooldown) : base(health, attackPower, attackCooldown)
    {
    }

    public override bool CanAttack()
    {
        return cooldownTimer <= 0 && target != null && !isAttacking && Vector2.Distance(target.position, transform.position) < 3;
    }

    protected void FixedUpdate()
    {
        if (target != null && !isAttacking && Vector2.Distance(target.position, transform.position) > 3)
        {
            //Target is on the left
            if (target.position.x - transform.position.x < 0)
            {
                _rb.velocity = new Vector2(-speed * 100 * Time.deltaTime, _rb.velocity.y);
            }
            //Target is on the right
            else if (target.position.x - transform.position.x > 0)
            {
                _rb.velocity = new Vector2(speed * 100 * Time.deltaTime, _rb.velocity.y);
            }
        }
        else
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
        }
    }

    public override void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackRangePosition, attackRangeSize, 0);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                //We can avoid this check by using a layermask on the OverlapBox
                if (hit.CompareTag("Player"))
                {
                    print("Attacked the player!");
                    //Here we would need to get the player script from hit and call the takeDamage function
                }
            }
        }
        yield return new WaitForSeconds(1);
        RestartCooldown();
        isAttacking = false;
    }
}
