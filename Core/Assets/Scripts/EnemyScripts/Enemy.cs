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
        int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision, float _size , float _exp/*�߰��� �κ�*/)
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
        exp = _exp; //�߰��� �κ�
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

        //�ɷ�ġ ����
        if (name.Equals("Slime"))
        {
            SetEnemyStatus("Slime", 4, 1, 1, 4, 7, 20f, 4, 10);  //�̸�, hp, ������, ����, �̼�, ���� ����, �þ� ����, ũ��, ����ġ
        }
        if (name.Equals("Wolf"))
        {
            SetEnemyStatus("Wolf", 6, 3, 1, 6, 7, 25f, 3, 10);  //�̸�, hp, ������, ����, �̼�, ���� ����, �þ� ����, ũ��, ����ġ
        }
        if (name.Equals("Oak"))
        {
            SetEnemyStatus("Oak", 8, 5, 1, 3, 8, 20f, 3, 10);  //�̸�, hp, ������, ����, �̼�, ���� ����, �þ� ����, ũ��, ����ġ
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

        if (nowHp <= 0) // �� ���
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

    void Die() // �����
    {
        enemyAnimator.SetTrigger("Down");            // �ִϸ��̼� ����
        GetComponent<EnemyAi>().enabled = false;    // ���� ��Ȱ��ȭ
        GetComponent<BoxCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(GetComponent<Rigidbody2D>());       // �߷� ��Ȱ��ȭ
        Destroy(hpBar.gameObject);                  // ü�¹� ����
        Destroy(gameObject, 2);                     // 2���� ����

        GameObject director = GameObject.Find("Expscript");     // �߰��� �κ�(����ġ)
        director.GetComponent<Expscript>().AddExp(exp);         // �߰��� �κ�
    }

    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
