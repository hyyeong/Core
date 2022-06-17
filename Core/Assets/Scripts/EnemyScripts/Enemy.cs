using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float maxHp;
    public float nowHp;
    public float atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;
    public float exp;

    public bool alive = true;

    public Text damageText;
    Image nowHpbar;

    public GameObject prfHpBar;
    public GameObject canvas;

    RectTransform hpBar;
    public float height = 1.7f;
    public Animator enemyAnimator;
    float invincibility = 0.25f; //���� �����

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.transform.SetAsFirstSibling();

        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        SetAttackSpeed(atkSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            invincibility += Time.deltaTime;

            if (enemyName.Equals("Slime"))
            {
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
                (new Vector3(transform.position.x, transform.position.y - 1, 0));
                hpBar.position = _hpBarPos;
            }
            if (enemyName.Equals("Wolf")||enemyName.Equals("Boss2"))
            {
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
               (new Vector3(transform.position.x, transform.position.y + 2, 0));
                hpBar.position = _hpBarPos;
            }
            if (enemyName.Equals("Oak"))
            {
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
                (new Vector3(transform.position.x, transform.position.y + 5, 0));
                hpBar.position = _hpBarPos;
            }

            if (enemyName.Equals("Boss"))
            {
                Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
                (new Vector3(transform.position.x, transform.position.y + 7, 0));
                hpBar.position = _hpBarPos;
            }

            nowHpbar.fillAmount = (float)nowHp / (float)maxHp;

            if (nowHp <= 0) // �� ���
            {
                Die();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerAtk"))
        {
            enemyAnimator.SetTrigger("Damage");
            nowHp = nowHp - col.GetComponent<ATK>().damage;
            Text text = Instantiate(damageText, canvas.transform);
            text.text = System.Convert.ToInt32(col.GetComponent<ATK>().damage).ToString();
            text.GetComponent<DamageTextController>().initTransform = col.transform.position;
            text.transform.position = Camera.main.WorldToScreenPoint(col.transform.position);
            GameObject.Find("Player").GetComponent<PlayerController>().LifeSteal(col.GetComponent<ATK>().damage);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("PlayerDotAtk") && invincibility > 0.125f)
        {
            invincibility = 0f;
            enemyAnimator.SetTrigger("Damage");
            nowHp = nowHp - col.GetComponent<ATK>().damage;
            Text text = Instantiate(damageText, canvas.transform);
            text.text = System.Convert.ToInt32(col.GetComponent<ATK>().damage).ToString();
            text.GetComponent<DamageTextController>().initTransform = this.transform.position + new Vector3(0,2f,0);
            text.transform.position = Camera.main.WorldToScreenPoint(col.transform.position);
            GameObject.Find("Player").GetComponent<PlayerController>().LifeSteal(col.GetComponent<ATK>().damage);
        }
    }
    
    void Die() // �����
    {
        alive = false;
        GameObject.Find("Player").GetComponent<PlayerController>().IncreaseExp(exp); // ����ġ ����
        enemyAnimator.SetTrigger("Down");            // �ִϸ��̼� ����
        GetComponent<EnemyAi>().enabled = false;    // ���� ��Ȱ��ȭ
        GetComponent<BoxCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(GetComponent<Rigidbody2D>());       // �߷� ��Ȱ��ȭ
        Destroy(hpBar.gameObject);                  // ü�¹� ����
        Destroy(gameObject, 2);                     // 2���� ����
        transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        if (enemyName.Equals("Boss"))
        {
            Invoke("Clear1", 1f);
        }
        if (enemyName.Equals("Boss2"))
        {
            Invoke("Clear2", 1f);
        }
    }
    void Clear1()
    {
        GameObject.Find("GameDirector").GetComponent<GameDirector>().LoadStage2();
        Debug.Log("CLeE");
    }
    void Clear2()
    {
        GameObject.Find("GameDirector").GetComponent<GameDirector>().LoadStage3();
    }
    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
