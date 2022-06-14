using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Witch : MonoBehaviour
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
    public float mgSpeed;
    public float mgRange;
    public float exp;

    private void SetEnemyStatus(string _enemyName, int _maxHp,
        int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange,
        float _fieldOfVision, float _size, float _mgSpeed, float _mgRange, float _exp)
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
        mgSpeed = _mgSpeed;
        mgRange = _mgRange;
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
        SetEnemyStatus("Witch", 10, 1, 0.3f, 2, 5, 20f, 2, 2, 15, 10);
            //�̸�, hp, ������, ����, �̼�, ���� ����, �þ� ����, ũ��, ���� �ӵ�, ���� �����Ÿ�, ����ġ
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        SetAttackSpeed(atkSpeed, mgSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
        (new Vector3(transform.position.x, transform.position.y + 4, 0));
        hpBar.position = _hpBarPos;

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
            nowHp = nowHp - 2;
            Debug.Log(nowHp);
        }
    }
    void Die()
    {
        enemyAnimator.SetTrigger("Down");            // �ִϸ��̼� ����
        GetComponent<WitchAi>().enabled = false;    // ���� ��Ȱ��ȭ
        GetComponent<BoxCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(GetComponent<Rigidbody2D>());       // �߷� ��Ȱ��ȭ
        Destroy(hpBar.gameObject);                  // ü�¹� ����
        Destroy(gameObject, 2);                     // 2���� ����

        GameObject director = GameObject.Find("Expscript");     // �߰��� �κ�(����ġ)
        director.GetComponent<Expscript>().AddExp(exp);         // �߰��� �κ�
    }

    void SetAttackSpeed(float speed, float magic)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
        enemyAnimator.SetFloat("maigcSpeed", magic);
    }
}
