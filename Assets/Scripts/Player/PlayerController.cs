using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // 좌표, 에니메이터 등
    Rigidbody2D rigid2D;
    Animator animator;

    const int MAX_JUMP = 2; // 2단점프까지
    int isJump = 0; // 점프상태 체크 변수

    Transform attackPos;

    public GameObject PlayerAttackPos;
    public GameObject AttackObject;
    public BuffEffectGenerator buffEffect; // 버프 이펙트 관리
    // 이동 방향
    int key = 1;

    // UI
    Text hptext;
    Text sheildtext;
    // Effect
    public GameObject healEffect;
    public GameObject blinkEffect;

    //스탯 관련
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

    //환경( 경험치, 레벨 등)
    public int level { get; } = 1;
    double exp = 0;

    // 스킬 관련
    delegate void SkillSet();
    SkillSet QSkill;
    SkillSet WSkill;
    SkillSet ESkill;
    SkillSet RSkill;
    SkillSet TSkill;
    // 재사용대기시간은 스킬을 고를떄 관리하기
    public float qCoolTime { get; set; } = 3f; // 재사용 대기시간
    public float wCoolTime { get; set; } = 2f;
    public float eCoolTime { get; set; } = 30f;
    public float rCoolTime { get; set; } = 3f;
    public float tCoolTime { get; set; } = 3f;
    public float currentQCoolTime { get; set; } // 현재 재사용 대기시간
    public float currentWCoolTime { get; set; }
    public float currentECoolTime { get; set; }
    public float currentRCoolTime { get; set; }
    public float currentTCoolTime { get; set; }

    public float qMana { get; set; } = 0f;
    public float wMana { get; set; } = 0f;
    public float eMana { get; set; } = 0f;
    public float rMana { get; set; } = 0f;
    public float tMana { get; set; } = 0f;
    // 버프 스킬 변수
    float lifeSteal = 0f; // 대미지 넣을 때 구현하기
    float recoveryShield = 0f;
    float ara = 1f;
    float concentration = 1f; //대미지 넣을때 구현하기
    float recycling = 0f;
    // 버프 스킬 관리 변수
    // 효과 종료시점 때문에 get 필수
    public float lifeStealTime { set; get; } = 0f;
    public float recoverySheildTime { set;  get; } = 0f;
    public float araTime { set; get; } = 0f;
    public float concentrationTime { set; get; } = 0f;
    public float recyclingTime { set; get; } = 0f;
    // 메소드
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>(); // 물리객체 휙득
        this.animator = GetComponent<Animator>();
        this.attackPos = PlayerAttackPos.transform;
        this.buffEffect = GameObject.Find("BuffEffectGenerator").GetComponent<BuffEffectGenerator>();
        // UI 객체 휙득
        /*hptext = GameObject.Find("hpText").GetComponent<Text>();
        sheildtext = GameObject.Find("shieldText").GetComponent<Text>();*/
        // 스텟 설정
        hp = MAX_HP;
        shield = MAX_SHIELD;
        mana = MAX_MANA;

        // 스킬 설정
        QSkill = new SkillSet(SkillRecycling);
        WSkill = new SkillSet(SkillBlink);
        ESkill = new SkillSet(SkillLifeSteal);
        RSkill = new SkillSet(SkillAra);
        TSkill = new SkillSet(SkillConcentration);

        // 쿨다운 설정
        currentQCoolTime = qCoolTime;
        currentWCoolTime = wCoolTime;
        currentECoolTime = eCoolTime;
        currentRCoolTime = rCoolTime;
        currentTCoolTime = tCoolTime;

        // 코루틴 설정
        StartCoroutine(ManageBuff());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        this.animator.SetBool("isJump", false);
        Debug.Log($"충돌 점프 false 설정 실제 jump : {this.animator.GetBool("isJump")}");
        isJump = 0;// 점프횟수 0
    }
    // Update is called once per frame
    void Update()
    {
        Jump();
        CheckRun();
        // 공격 체크
        Attack();
        RecoveryShield(3f * Time.deltaTime);
        RecoveryMana(1f * Time.deltaTime);
        Die();
        CheckSkill();
        // 속도 제한
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce*Time.deltaTime);
        }

        /*UIUpdate();*/
    }

    IEnumerator ManageBuff() // 코루틴 사용 버프관리
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
/* 캐릭터 행동 메소드*/
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
            Debug.Log($"우로 이동 key : {key}");
            transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
            if (!animator.GetBool("isJump"))
                animator.SetBool("isRun", true);
            Debug.Log($"좌로 이동 key : {key}");
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
    // 스킬이 비어있는 경우
    void EmptySkill()
    {
        Debug.Log("해당 스킬은 비어있습니다.");
    }
    // 스킬들
    // 보조스킬
    void SkillHeal()
    {
        // 공격력 * 2 만큼 회복
        hp += atk_damage * 2;
        Quaternion rotate = Quaternion.Euler(-90, 0, 0);
        Instantiate(healEffect, transform.position, rotate);
        Debug.Log("힐스킬");
    }
    void SkillLifeSteal()
    {
        // 생명력 흡수 버프 
        lifeStealTime = 30f; // 30초
        lifeSteal = 0.1f; // 피해량 10%
        buffEffect.LifeStealEffect(transform);
    }
    void SkillShieldRecovery()
    {
        // 쉴드 회복량 50%증가
        recoverySheildTime = 30f;
        recoveryShield = 0.5f;
        buffEffect.RecoveryShieldEffect(transform);
    }
    void SkillAra()
    {
        // 점프 량 25%증가
        araTime = 30f;
        ara = 1.25f;
        buffEffect.AraEffect(transform);
    }
    void SkillRecycling()
    {
        // 잃은마나 1% 회복
        recyclingTime = 30f;
        recycling = 0.01f;
        buffEffect.RecyclingEffect(transform);
    }
    void SkillConcentration()
    {
        // 가하는대미지 1.5배
        concentrationTime = 30f;
        concentration = 1.5f;
        buffEffect.ConcentrationEffect(transform);
    }
    void SkillBlink()
    {
        // 짧은거리 순간이동
        float dir = transform.localScale.x > 0 ? 1 : -1;
        Vector3 newLocation = transform.position + new Vector3(dir*4f,0,0);
        Instantiate(blinkEffect, transform.position, transform.rotation);
        transform.position = newLocation;
        Instantiate(blinkEffect, transform.position, transform.rotation);
        Debug.Log("블링크 스킬");
    }
    public float HpRatio()
    {
        return (float)hp / MAX_HP;
    }
    // 스텟 조절
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
            mana += (MAX_MANA - mana) * recycling * Time.deltaTime; // 잃은마나
        }
        else
        {
            mana = MAX_MANA;
        }
    }
}
