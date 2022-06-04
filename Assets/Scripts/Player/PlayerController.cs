using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // ��ǥ, ���ϸ����� ��
    Rigidbody2D rigid2D;
    Animator animator;

    const int MAX_JUMP = 2; // 2����������
    int isJump = 0; // �������� üũ ����

    Transform attackPos;

    public GameObject PlayerAttackPos;
    public GameObject AttackObject;
    public BuffEffectGenerator buffEffect; // ���� ����Ʈ ����
    // �̵� ����
    int key = 1;

    // UI
    Text hptext;
    Text sheildtext;
    // Effect
    public GameObject healEffect;
    public GameObject blinkEffect;

    //���� ����
    public float MAX_HP { get; set; } = 3000;
    public float MAX_SHIELD { get; set; } = 6000;
    public float MAX_MANA { get; set; } = 6000;
    public float atk_damage { get; set; } = 1000;
    public float jumpForce = 600.0f;
    public float walkForce = 70.0f;
    public float maxWalkSpeed = 3.0f;
    public float attack_speed = 1f;
    float attack_cool = 0;
    float hp ;
    float shield ;
    float mana ;

    //ȯ��( ����ġ, ���� ��)
    public int level { get; } = 1;
    double exp = 0;

    // ��ų ����
    delegate void SkillSet();
    SkillSet QSkill;
    SkillSet WSkill;
    SkillSet ESkill;
    SkillSet RSkill;
    SkillSet TSkill;
    // ������ð��� ��ų�� ������ �����ϱ�
    public float qCoolTime { get; set; } = 3f; // ���� ���ð�
    public float wCoolTime { get; set; } = 2f;
    public float eCoolTime { get; set; } = 30f;
    public float rCoolTime { get; set; } = 3f;
    public float tCoolTime { get; set; } = 3f;
    public float currentQCoolTime { get; set; } // ���� ���� ���ð�
    public float currentWCoolTime { get; set; }
    public float currentECoolTime { get; set; }
    public float currentRCoolTime { get; set; }
    public float currentTCoolTime { get; set; }

    public float qMana { get; set; } = 0f;
    public float wMana { get; set; } = 0f;
    public float eMana { get; set; } = 0f;
    public float rMana { get; set; } = 0f;
    public float tMana { get; set; } = 0f;
    // ���� ��ų ����
    float lifeSteal = 0f; // ����� ���� �� �����ϱ�
    float recoveryShield = 0f;
    float ara = 1f;
    float concentration = 1f; //����� ������ �����ϱ�
    float recycling = 0f;
    // ���� ��ų ���� ����
    // ȿ�� ������� ������ get �ʼ�
    public float lifeStealTime { set; get; } = 0f;
    public float recoverySheildTime { set;  get; } = 0f;
    public float araTime { set; get; } = 0f;
    public float concentrationTime { set; get; } = 0f;
    public float recyclingTime { set; get; } = 0f;
    // �޼ҵ�
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>(); // ������ü �׵�
        this.animator = GetComponent<Animator>();
        this.attackPos = PlayerAttackPos.transform;
        this.buffEffect = GameObject.Find("BuffEffectGenerator").GetComponent<BuffEffectGenerator>();
        // UI ��ü �׵�
        /*hptext = GameObject.Find("hpText").GetComponent<Text>();
        sheildtext = GameObject.Find("shieldText").GetComponent<Text>();*/
        // ���� ����
        hp = MAX_HP;
        shield = MAX_SHIELD;
        mana = MAX_MANA;

        // ��ų ����
        QSkill = new SkillSet(SkillRecycling);
        WSkill = new SkillSet(SkillBlink);
        ESkill = new SkillSet(SkillLifeSteal);
        RSkill = new SkillSet(SkillAra);
        TSkill = new SkillSet(SkillConcentration);

        // ��ٿ� ����
        currentQCoolTime = qCoolTime;
        currentWCoolTime = wCoolTime;
        currentECoolTime = eCoolTime;
        currentRCoolTime = rCoolTime;
        currentTCoolTime = tCoolTime;

        // �ڷ�ƾ ����
        StartCoroutine(ManageBuff());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.animator.SetBool("isJump", false);
        Debug.Log($"�浹 ���� false ���� ���� jump : {this.animator.GetBool("isJump")}");
        isJump = 0;// ����Ƚ�� 0
    }
    // Update is called once per frame
    void Update()
    {
        Jump();
        CheckRun();
        // ���� üũ
        Attack();
        RecoveryShield(3f * Time.deltaTime);
        RecoveryMana(1f * Time.deltaTime);
        Die();
        CheckSkill();
        // �ӵ� ����
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce*Time.deltaTime);
        }

        /*UIUpdate();*/
    }

    IEnumerator ManageBuff() // �ڷ�ƾ ��� ��������
    {
        while (true)
        {
            lifeStealTime -= Time.deltaTime;
            recoverySheildTime -= Time.deltaTime;
            araTime -= Time.deltaTime;
            recyclingTime -= Time.deltaTime;
            concentrationTime -= Time.deltaTime;
            if (lifeStealTime < 0)
            {
                lifeSteal = 0f;
            }
            if (recoverySheildTime < 0)
            {
                recoveryShield = 0f;
            }
            if (araTime < 0)
            {
                ara = 1f;
            }
            if (recyclingTime < 0)
            {
                recycling = 0f;
            }
            if (concentrationTime < 0)
            {
                concentration = 1f;
            }
            yield return new WaitForEndOfFrame();
        }
    }
/* ĳ���� �ൿ �޼ҵ�*/
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.A) && attack_speed<=attack_cool)
        {
            Instantiate(AttackObject, attackPos.position, attackPos.rotation).GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
            animator.SetTrigger("attack");
            attack_cool = 0;
        }
        attack_cool += (attack_cool <= 2) ? Time.deltaTime : 0;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isJump < MAX_JUMP)
        {
            ++isJump;
            float plusJumpForce = 1;
            if (isJump == 2) plusJumpForce = 1.5f;
            this.rigid2D.AddForce(Vector2.up * this.jumpForce * plusJumpForce *ara);
            animator.SetBool("isJump", true);
        }
    }
    private void CheckRun()
    {
        key = 0;
        animator.SetBool("isRun", false);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            key = 1;
            if (!animator.GetBool("isJump"))
                animator.SetBool("isRun", true);
            Debug.Log($"��� �̵� key : {key}");
            transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
            if (!animator.GetBool("isJump"))
                animator.SetBool("isRun", true);
            Debug.Log($"�·� �̵� key : {key}");
            transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

    }
    void Die()
    {
        if (hp < 0)
        {
            animator.SetTrigger("die");
            SceneManager.LoadScene("GameOver");
        }
    }
    // Skill Check
    void CheckSkill()
    {
        currentQCoolTime += Time.deltaTime;
        currentWCoolTime += Time.deltaTime;
        currentECoolTime += Time.deltaTime;
        currentRCoolTime += Time.deltaTime;
        currentTCoolTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && currentQCoolTime >= qCoolTime && mana>=qMana)
        {
            animator.SetTrigger("isLookUp");
            QSkill();
            DecreaseMana(qMana);
            currentQCoolTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.W) && currentWCoolTime >= wCoolTime && mana >= wMana)
        {
            animator.SetTrigger("isLookUp");
            WSkill();
            DecreaseMana(wMana);
            currentWCoolTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.E) && currentECoolTime >= eCoolTime && mana >= eMana)
        {
            animator.SetTrigger("isLookUp");
            ESkill();
            DecreaseMana(eMana);
            currentECoolTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.R) && currentRCoolTime >= rCoolTime && mana >= rMana)
        {
            animator.SetTrigger("isLookUp");
            RSkill();
            DecreaseMana(rMana);
            currentRCoolTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.T) && currentTCoolTime >= tCoolTime && mana >= tMana)
        {
            animator.SetTrigger("isLookUp");
            TSkill();
            DecreaseMana(tMana);
            currentTCoolTime = 0f;
        }
    }
    // ��ų�� ����ִ� ���
    void EmptySkill()
    {
        Debug.Log("�ش� ��ų�� ����ֽ��ϴ�.");
    }
    // ��ų��
    // ������ų
    void SkillHeal()
    {
        // ���ݷ� * 2 ��ŭ ȸ��
        hp += atk_damage * 2;
        Quaternion rotate = Quaternion.Euler(-90, 0, 0);
        Instantiate(healEffect, transform.position, rotate);
        Debug.Log("����ų");
    }
    void SkillLifeSteal()
    {
        // ������ ���� ���� 
        lifeStealTime = 30f; // 30��
        lifeSteal = 0.1f; // ���ط� 10%
        buffEffect.LifeStealEffect(transform);
    }
    void SkillShieldRecovery()
    {
        // ���� ȸ���� 50%����
        recoverySheildTime = 30f;
        recoveryShield = 0.5f;
        buffEffect.RecoveryShieldEffect(transform);
    }
    void SkillAra()
    {
        // ���� �� 25%����
        araTime = 30f;
        ara = 1.25f;
        buffEffect.AraEffect(transform);
    }
    void SkillRecycling()
    {
        // �������� 1% ȸ��
        recyclingTime = 30f;
        recycling = 0.01f;
        buffEffect.RecyclingEffect(transform);
    }
    void SkillConcentration()
    {
        // ���ϴ´���� 1.5��
        concentrationTime = 30f;
        concentration = 1.5f;
        buffEffect.ConcentrationEffect(transform);
    }
    void SkillBlink()
    {
        // ª���Ÿ� �����̵�
        float dir = transform.localScale.x > 0 ? 1 : -1;
        Vector3 newLocation = transform.position + new Vector3(dir*4f,0,0);
        Instantiate(blinkEffect, transform.position, transform.rotation);
        transform.position = newLocation;
        Instantiate(blinkEffect, transform.position, transform.rotation);
        Debug.Log("����ũ ��ų");
    }
    public float HpRatio()
    {
        return (float)hp / MAX_HP;
    }
    // ���� ����
    public float DecreaseMana(float mana)
    {
        this.mana -= mana;
        return mana;
    }
    public float SheildRatio()
    {
        return (float)shield / MAX_SHIELD;
    }
    void UIUpdate()
    {
        hptext.text = $"{hp}/{MAX_HP}";
        sheildtext.text = $"{shield}/{MAX_SHIELD}";
    }
    public void Damaged(float damage)
    {
        float dam = damage;
        if (shield < dam)
        {
            dam -= shield;
            shield = 0;
            hp -= dam;
        }
        else
        {
            shield -= dam;
        }
    }
    void RecoveryShield(float amount = 0.5f)
    {
        if (shield < MAX_SHIELD)
        {
            shield = shield + amount * (1f + recoveryShield);
        }
        else
        {
            shield = MAX_SHIELD;
        }
    }
    void RecoveryMana(float amount = 0.5f)
    {
        if (mana < MAX_MANA)
        {
            mana += amount * (1f);
            mana += (MAX_MANA - mana) * recycling * Time.deltaTime; // ��������
        }
        else
        {
            mana = MAX_MANA;
        }
    }
}