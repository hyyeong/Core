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

    public GameObject PlayerAttack;
    public GameObject AttackObject;
    // �̵� ����
    int key = 1;

    // UI
    Text hptext;
    Text sheildtext;

    //���� ����
    public float MAX_HP { get; set; } = 3000;
    public float MAX_SHIELD { get; set; } = 6000;
    public float MAX_MANA { get; set; } = 6000;
    public float atk_damage { get; set; } = 1000;
    public float jumpForce = 600.0f;
    public float walkForce = 70.0f;
    public float maxWalkSpeed = 3.0f;
    public float attack_speed = 1.0f;

    float hp ;
    float shield ;
    float mana ;

    //ȯ��( ����ġ, ���� ��)
    public int level { get; } = 1;
    double exp = 0;


    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>(); // ������ü �׵�
        this.animator = GetComponent<Animator>();
        this.attackPos = PlayerAttack.transform;
        // UI ��ü �׵�
        /*hptext = GameObject.Find("hpText").GetComponent<Text>();
        sheildtext = GameObject.Find("shieldText").GetComponent<Text>();*/
        // ���� ����
        hp = MAX_HP;
        shield = MAX_SHIELD;
        mana = MAX_MANA;
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            /*Instantiate(AttackObject, attackPos.position, attackPos.rotation).GetComponent<AttackController>().setDirection(transform.localScale.x);*/
            animator.SetTrigger("attack");
        }
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
    public float hp_ratio()
    {
        return (float)hp / MAX_HP;
    }

    public float shield_ratio()
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
