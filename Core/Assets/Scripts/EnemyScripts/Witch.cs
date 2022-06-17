using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Witch : MonoBehaviour
{
    public string enemyName;
    public float maxHp;
    public float nowHp;
    public float atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;
    public float mgSpeed;
    public float mgRange;

    public bool alive = true;
    float invincibility = 0.25f; //지속 대미지

    Image nowHpbar;

    public Text damageText;
    public GameObject prfHpBar;
    GameObject canvas;

    RectTransform hpBar;
    public float height = 1.7f;
    public Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        hpBar.transform.SetAsFirstSibling();
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

        SetAttackSpeed(atkSpeed, mgSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            invincibility += Time.deltaTime;
            Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
            (new Vector3(transform.position.x, transform.position.y + 4, 0));
            hpBar.position = _hpBarPos;

            nowHpbar.fillAmount = (float)nowHp / (float)maxHp;

            if (nowHp <= 0) // 적 사망
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
            Text text = Instantiate(damageText, canvas.transform);
            text.text = System.Convert.ToInt32(col.GetComponent<ATK>().damage).ToString();
            text.GetComponent<DamageTextController>().initTransform = col.transform.position;
            text.transform.position = Camera.main.WorldToScreenPoint(col.transform.position);
            nowHp = nowHp - col.GetComponent<ATK>().damage;
            GameObject.Find("Player").GetComponent<PlayerController>().LifeSteal(col.GetComponent<ATK>().damage);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("PlayerDotAtk") && invincibility > 0.125f)
        {
            invincibility = 0f;
            Text text = Instantiate(damageText, canvas.transform);
            text.text = System.Convert.ToInt32(col.GetComponent<ATK>().damage).ToString();
            text.GetComponent<DamageTextController>().initTransform = this.transform.position + new Vector3(0, 2f, 0);
            text.transform.position = Camera.main.WorldToScreenPoint(col.transform.position);
            enemyAnimator.SetTrigger("Damage");
            nowHp = nowHp - col.GetComponent<ATK>().damage;
            GameObject.Find("Player").GetComponent<PlayerController>().LifeSteal(col.GetComponent<ATK>().damage);
        }
    }

    void Die()
    {
        alive = false;
        enemyAnimator.SetTrigger("Down");            // 애니메이션 실행
        GetComponent<WitchAi>().enabled = false;    // 추적 비활성화
        GetComponent<BoxCollider2D>().enabled = false; // 충돌체 비활성화
        Destroy(GetComponent<Rigidbody2D>());       // 중력 비활성화
        Destroy(hpBar.gameObject);                  // 체력바 제거
        Destroy(gameObject, 2);                     // 2초후 제거
        Invoke("Clear", 1f); // 2초후 클리어
    }
    void Clear()
    {

        GameObject.Find("GameDirector").GetComponent<GameDirector>().DestroyObject();
        SceneManager.LoadScene("Winer");
    }
    void SetAttackSpeed(float speed, float magic)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
        enemyAnimator.SetFloat("maigcSpeed", magic);
    }
}
