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
    // �̵� ����
    int key = 1;

    // UI
    Text hptext;
    Text sheildtext;
    // Effect
    public GameObject healEffect;

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
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>(); // ������ü �׵�
        this.animator = GetComponent<Animator>();
        this.attackPos = PlayerAttackPos.transform;
        // UI ��ü �׵�
        /*hptext = GameObject.Find("hpText").GetComponent<Text>();
        sheildtext = GameObject.Find("shieldText").GetComponent<Text>();*/
        // ���� ����
        hp = MAX_HP;
        shield = MAX_SHIELD;
        mana = MAX_MANA;

        QSkill = new SkillSet(SkillHeal);
        WSkill = new SkillSet(EmptySkill);
        ESkill = new SkillSet(EmptySkill);
        RSkill = new SkillSet(EmptySkill);
        TSkill = new SkillSet(EmptySkill);
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
        RecoveryShield(3);
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
            this.rigid2D.AddForce(Vector2.up * this.jumpForce * plusJumpForce);
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("isLookUp");
            QSkill();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            WSkill();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ESkill();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RSkill();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TSkill();
        }
    }
    // ��ų�� ����ִ� ���
    void EmptySkill()
    {
        Debug.Log("�ش� ��ų�� ����ֽ��ϴ�.");
    }
    // ��ų��
    void SkillHeal()
    {
        Quaternion rotate = Quaternion.Euler(-90, 0, 0);
        Instantiate(healEffect, transform.position, rotate);
        Debug.Log("����ų");
        
    }
    public float HpRatio()
    {
        return (float)hp / MAX_HP;
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
    void RecoveryShield(float amount = 1)
    {
        if (shield < MAX_SHIELD)
        {
            shield += amount;
        }
    }
}
