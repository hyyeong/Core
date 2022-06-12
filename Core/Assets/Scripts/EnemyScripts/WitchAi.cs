using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAi : MonoBehaviour
{
    Transform target;
    public GameObject Magic;
    float attackDelay;
    float magicDelay;

    Witch enemy;
    Animator enemyAnimator;

    void Start()
    {
        enemy = GetComponent<Witch>();
        enemyAnimator = enemy.enemyAnimator;
    }

    void Update()
    {
        target = GameObject.Find("Player").transform;
        attackDelay -= Time.deltaTime;
        magicDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;
        if (magicDelay < 0) magicDelay = 0;
        float distance = Vector3.Distance(transform.position, target.position);

        if(magicDelay == 0 && distance <= enemy.fieldOfVision)
        {
            FaceTarget();

            if (distance <= enemy.mgRange) //사거리 안에 목표가 있으면
            {
                MagicTarget();
            }
            else
            {
                if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("magic"))
                {
                    MoveToTarget();
                }
            }
        }
        else if (attackDelay == 0 && distance <= enemy.fieldOfVision)
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
        transform.localScale = new Vector3(dir * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void AttackTarget()
    {
        enemyAnimator.SetTrigger("Attack"); // 공격 애니메이션 실행
        attackDelay = enemy.atkSpeed; // 딜레이 충전
    }

    void MagicTarget()
    {
        float ordx = target.position.x;
        float ordy = target.position.y;
        enemyAnimator.SetTrigger("Magic"); // 마법 애니메이션 실행
        magicDelay = enemy.mgSpeed; // 딜레이 충전
        attackDelay = enemy.atkSpeed;

        GameObject orb = Instantiate(Magic);
        orb.transform.position = new Vector3(ordx, ordy+10, 0);
    }
}
