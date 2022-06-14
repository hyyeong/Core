using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;
    public float size;
    public float exp;

    private void SetEnemyStatus(string _enemyName, int _maxHp, 
        int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision, float _size , float _exp/*추가된 부분*/)
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
        size = _size;
        exp = _exp; //추가된 부분
    }


    Image nowHpbar;

    public GameObject prfHpBar;
    public GameObject canvas;

    RectTransform hpBar;
    public float height = 1.7f;
    public Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();

        //능력치 설정
        if (name.Equals("Slime"))
        {
            SetEnemyStatus("Slime", 4, 1, 1, 4, 7, 20f, 4, 10);  //이름, hp, 데미지, 공속, 이속, 공격 범위, 시아 범위, 크기, 경험치
        }
        if (name.Equals("Wolf"))
        {
            SetEnemyStatus("Wolf", 6, 3, 1, 6, 7, 25f, 3, 10);  //이름, hp, 데미지, 공속, 이속, 공격 범위, 시아 범위, 크기, 경험치
        }
        if (name.Equals("Oak"))
        {
            SetEnemyStatus("Oak", 8, 5, 1, 3, 8, 20f, 3, 10);  //이름, hp, 데미지, 공속, 이속, 공격 범위, 시아 범위, 크기, 경험치
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        SetAttackSpeed(atkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (name.Equals("Slime"))
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
            (new Vector3(transform.position.x, transform.position.y - 1, 0));
            hpBar.position = _hpBarPos;
        }
        if (name.Equals("Wolf"))
        {
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
           (new Vector3(transform.position.x, transform.position.y + 2, 0));
            hpBar.position = _hpBarPos;
        }
        if (name.Equals("Oak"))
        {
           Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
           (new Vector3(transform.position.x, transform.position.y + 5, 0));
           hpBar.position = _hpBarPos;
        }
        

        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;

        if (nowHp <= 0) // 적 사망
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Untagged"))
        {
            enemyAnimator.SetTrigger("Damage");
            nowHp = nowHp -2;
            Debug.Log(nowHp);
        }
    }

    void Die() // 사망시
    {
        enemyAnimator.SetTrigger("Down");            // 애니메이션 실행
        GetComponent<EnemyAi>().enabled = false;    // 추적 비활성화
        GetComponent<BoxCollider2D>().enabled = false; // 충돌체 비활성화
        Destroy(GetComponent<Rigidbody2D>());       // 중력 비활성화
        Destroy(hpBar.gameObject);                  // 체력바 제거
        Destroy(gameObject, 2);                     // 2초후 제거

        GameObject director = GameObject.Find("Expscript");     // 추가된 부분(경험치)
        director.GetComponent<Expscript>().AddExp(exp);         // 추가된 부분
    }

    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
