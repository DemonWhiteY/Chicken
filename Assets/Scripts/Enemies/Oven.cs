using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : BaseEnemy
{
    [SerializeField]
    private ParticleSystem _flamethrowerParticles;

    [SerializeField]
    private Transform _flamethrowerCone;

    [SerializeField]
    //How many times will the flamethrower hit
    private int _attackTimes;

    [SerializeField]
    //How often will the flamethrower hit
    private float _attackInterval;

    public Oven(int health, int attackPower, float attackCooldown) : base(health, attackPower, attackCooldown)
    {
    }

    public override void Attack()
    {
        StartCoroutine(FlamethrowerAttack());
    }

    protected override void Update()
    {
        base.Update();
        if(target != null && !isAttacking)
        {
            //Target is on the left
            if(target.transform.position.x - transform.position.x < 0)
            {
                _flamethrowerCone.localPosition = new Vector2(-0.6f, 0);
                _flamethrowerCone.localEulerAngles = new Vector3(0, 0, -90);
                attackRangePositionOffset.x = -Mathf.Abs(attackRangePositionOffset.x);
            }
            //Target is on the right
            else
            {
                _flamethrowerCone.localPosition = new Vector2(0.6f, 0);
                _flamethrowerCone.localEulerAngles = new Vector3(0, 0, 90);
                attackRangePositionOffset.x = Mathf.Abs(attackRangePositionOffset.x);
            }
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

    private IEnumerator FlamethrowerAttack()
    {
        isAttacking = true;
        _flamethrowerParticles.Play();
        for (int i = 0; i < _attackTimes; i++)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackRangePosition, attackRangeSize, 0);
            if(hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    //We can avoid this check by using a layermask on the OverlapBox
                    if (hit.CompareTag("Player"))
                    {
                        //Here we would need to get the player script from hit and call the takeDamage function
                    }
                }
            }
            yield return new WaitForSeconds(_attackInterval);
        }
        _flamethrowerParticles.Stop();
        yield return new WaitForSeconds(0.35f);
        isAttacking = false;
        RestartCooldown();
    }

}
