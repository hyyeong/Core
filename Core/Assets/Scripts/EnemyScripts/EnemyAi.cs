using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    Transform target;
    float attackDelay;
    public GameObject AttackObject;
    Enemy enemy;
    Animator enemyAnimator;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        enemy = GetComponent<Enemy>();
        enemyAnimator = enemy.enemyAnimator;
        AttackObject.GetComponent<ATK>().damage = enemy.atkDmg;
        AttackObject.SetActive(false);
    }

    void Update()
    {
        if (enemy.alive)
        {
            attackDelay -= Time.deltaTime;
            if (attackDelay < 0) attackDelay = 0;
            if (attackDelay < enemy.atkSpeed - 0.2) AttackObject.SetActive(false);
            float distance = Vector3.Distance(transform.position, target.position);

            if (attackDelay == 0 && distance <= enemy.fieldOfVision)
            {
                FaceTarget();

                if (distance <= enemy.atkRange)
                {
                    AttackTarget();
                }
                else
                {
                    if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        MoveToTarget();
                    }
                }
            }
            else
            {
                enemyAnimator.SetBool("moving", false);
            }
        }
        else
        {
            Destroy(AttackObject);
        }
    }

    void MoveToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        transform.Translate(new Vector2(dir, 0) * enemy.moveSpeed * Time.deltaTime);
        enemyAnimator.SetBool("moving", true);
    }

    void FaceTarget()
    {
        float dir;
        if (target.position.x - transform.position.x < 0) // 타겟이 왼쪽에 있을 때
        {
            dir = 1;
        }
        else // 타겟이 오른쪽에 있을 때
        {
            dir = -1;
        }
        transform.localScale = new Vector3(dir*Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void AttackTarget()
    {
        if (enemy.alive)
        {
            AttackObject.SetActive(true);
            enemyAnimator.SetTrigger("Attack"); // 공격 애니메이션 실행
            attackDelay = enemy.atkSpeed; // 딜레이 충전
        }
    }
}
