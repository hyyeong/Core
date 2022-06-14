using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Global;

namespace Global
{
    public delegate void SkillSet();
}
public class PlayerController : MonoBehaviour
{

    public static float BGspeed = 0.0f;
    public delegate void SkillSet();
    // 좌표, 에니메이터 등
    Rigidbody2D rigid2D;
    Animator animator;

    const int MAX_JUMP = 2; // 2단점프까지
    int isJump = 0; // 점프상태 체크 변수
    AudioSource audioMoveJumpATK;
    AudioSource audioSk;
    public AudioClip audioAtk;
    public AudioClip audioHit;
    public AudioClip audioJump;
    public AudioClip audioJump1;
    public AudioClip audioSkill;
    Transform attackPos;

    public GameObject PlayerAttackPos;
    public GameObject AttackObject;
    public BuffEffectGenerator buffEffect; // 버프 이펙트 관리
    // 이동 방향
    int key = 1;

    // UI
    Text hptext;
    Text sheildtext;
    public Text StatHpText;
    public Text StatManaText;
    public Text StatSheildText;
    public Text StatAtkText;
    public Text StatArmorText;
    public Text StatPointText;
    // Effect
    public GameObject levelUpEffect;
    public GameObject healEffect;
    public GameObject blinkEffect;
    // AttackMagic Skill Effect
    public GameObject magicArrowEffect;
    public GameObject magicCircleEffect;
    public GameObject altinEffect;
    public GameObject holyEffect;
    public GameObject lightningBallEffect;
    public GameObject lightningArrowEffect;
    public GameObject artifficialSunEffect;
    public GameObject fireShieldEffect;
    public GameObject blizzardEffect;

    //스탯 관련
    public float MAX_HP { get; set; } = 3000;
    public float MAX_SHIELD { get; set; } = 6000;
    public float MAX_MANA { get; set; } = 6000;
    public float atk_damage { get; set; } = 250;
    public float jumpForce;
    public float walkForce;
    public float maxWalkSpeed { set; get; } = 7.0f;
    public float attack_speed { set; get; } = 1f;
    public float attack_cool { set; get; } = 0;
    public float armor { set; get; } = 0;
    public float recoverySheildPerSec { set; get; } = 10f;
    public float recoveryManaPerSec { set; get; } = 10f;
    public float manaDrain { set; get; } = 0;
    public float cycle { set; get; } = 1f; // 마나소모량 비율
    float hp ;
    float shield ;
    float mana ;
    // 스탯과 패시브
    public float elemental_atk { set; get; } = 1f;
    public float magic_atk { set; get; } = 0f;
    public bool arcana { set; get; } = false;
    //환경( 경험치, 레벨 등)
    public int level { get; set; } = 1;
    float exp = 0;

    // 스킬 관련
    public int statPoints = 10;
    public int skillPoints = 10;
    public SkillSet QSkill { set; get; }
    public SkillSet WSkill { set; get; }
    public SkillSet ESkill { set; get; }
    public SkillSet RSkill { set; get; }
    public SkillSet TSkill { set; get; }
    // 재사용대기시간은 스킬을 고를떄 관리하기
    public float qCoolTime { get; set; } = 3f; // 재사용 대기시간
    public float wCoolTime { get; set; } = 2f;
    public float eCoolTime { get; set; } = 3f;
    public float rCoolTime { get; set; } = 3f;
    public float tCoolTime { get; set; } = 3f;
    public float currentQCoolTime { get; set; } = 0;// 현재 재사용 대기시간
    public float currentWCoolTime { get; set; } = 0;
    public float currentECoolTime { get; set; } = 0;
    public float currentRCoolTime { get; set; } = 0;
    public float currentTCoolTime { get; set; } = 0;

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
    float invincibility = 0f;
    // 메소드
    void Start()
    {
        this.audioMoveJumpATK = gameObject.AddComponent<AudioSource>();
        audioMoveJumpATK.loop = false;
        audioSk = gameObject.AddComponent<AudioSource>();
        audioSk.loop = false;
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
        QSkill = new SkillSet(EmptySkill);
        WSkill = new SkillSet(EmptySkill);
        ESkill = new SkillSet(EmptySkill);
        RSkill = new SkillSet(EmptySkill);
        TSkill = new SkillSet(EmptySkill);

        // 코루틴 설정
        StartCoroutine(ManageBuff());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Map")||other.CompareTag("Trap"))
        {
            this.animator.SetBool("isJump", false);
            audioMoveJumpATK.clip = audioJump;
            audioMoveJumpATK.Play();
            isJump = 0;// 점프횟수 0
        }
        if (other.CompareTag("enemyattack"))
        {
            Debug.Log($"{other.GetComponent<ATK>().damage}");
            Damaged(other.GetComponent<ATK>().damage);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap")&&invincibility>0.5f)
        {
            invincibility = 0f;
            Damaged(100);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Attack();
        Jump();
        CheckRun();
        // 공격 체크
        RecoveryShield(recoverySheildPerSec * Time.deltaTime);
        RecoveryMana(recoveryManaPerSec * Time.deltaTime);
        Die();
        CheckSkill();
        UIUpdate();
        // 속도 제한
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        BGspeed = (speedx > 0f) ? 0.1f * key * (maxWalkSpeed/3.0f): 0f;
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
            invincibility += Time.deltaTime;
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
            float atk = concentration * atk_damage;
            if (arcana)
            {
                float dir= transform.localScale.x < 0 ? -1 : 1;
                Quaternion rotate1 = Quaternion.Euler(new Vector3(0, 0, dir * 105f));
                Quaternion rotate2 = Quaternion.Euler(new Vector3(0, 0, dir * 95f));
                Quaternion rotate3 = Quaternion.Euler(new Vector3(0, 0, dir * 85f));
                Quaternion rotate4 = Quaternion.Euler(new Vector3(0, 0, dir * 75f));
                GameObject a1 = Instantiate(AttackObject, attackPos.position, rotate1);
                a1.GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
                a1.GetComponent<ATK>().damage = atk;
                a1 = Instantiate(AttackObject, attackPos.position, rotate2);
                a1.GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
                a1.GetComponent<ATK>().damage = atk;
                a1 = Instantiate(AttackObject, attackPos.position, rotate3);
                a1.GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
                a1.GetComponent<ATK>().damage = atk;
                a1 = Instantiate(AttackObject, attackPos.position, rotate4);
                a1.GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
                a1.GetComponent<ATK>().damage = atk;
            }
            else
            {
                GameObject a1 = Instantiate(AttackObject, attackPos.position, attackPos.rotation);
                a1.GetComponent<NormalAttackController>().setDirection(transform.localScale.x);
                a1.GetComponent<ATK>().damage = atk;
            }
            animator.SetTrigger("attack");
            attack_cool = 0;
            audioSk.clip = audioAtk;
            audioSk.Play();
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
            audioMoveJumpATK.clip = audioJump1;
            audioMoveJumpATK.Play();
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
            transform.localScale = new Vector3(key * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            key = -1;
            if (!animator.GetBool("isJump"))
                animator.SetBool("isRun", true);
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
        currentQCoolTime -= Time.deltaTime;
        currentWCoolTime -= Time.deltaTime;
        currentECoolTime -= Time.deltaTime;
        currentRCoolTime -= Time.deltaTime;
        currentTCoolTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && currentQCoolTime <= 0 && mana>=qMana)
        {
            Debug.Log("q");
            animator.SetTrigger("isLookUp");
            QSkill();
            DecreaseMana(qMana);
            currentQCoolTime = qCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.W) && currentWCoolTime <= 0 && mana >= wMana)
        {
            Debug.Log("w");
            animator.SetTrigger("isLookUp");
            WSkill();
            DecreaseMana(wMana);
            currentWCoolTime = wCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.E) && currentECoolTime <= 0 && mana >= eMana)
        {
            Debug.Log("e");
            animator.SetTrigger("isLookUp");
            ESkill();
            DecreaseMana(eMana);
            currentECoolTime = eCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.R) && currentRCoolTime <= 0 && mana >= rMana)
        {
            Debug.Log("r");
            animator.SetTrigger("isLookUp");
            RSkill();
            DecreaseMana(rMana);
            currentRCoolTime = rCoolTime;
        }
        if (Input.GetKeyDown(KeyCode.T) && currentTCoolTime <= 0 && mana >= tMana)
        {
            Debug.Log("t");
            animator.SetTrigger("isLookUp");
            TSkill();
            DecreaseMana(tMana);
            currentTCoolTime = tCoolTime;
        }
    }
    // 스킬이 비어있는 경우
    void EmptySkill()
    {
        Debug.Log("해당 스킬은 비어있습니다.");
    }
    // 스킬들
    // 보조스킬
    public void SkillHeal()
    {
        // 공격력 * 2 만큼 회복
        hp += atk_damage * 2;
        Quaternion rotate = Quaternion.Euler(-90, 0, 0);
        Instantiate(healEffect, transform.position, rotate);
        Debug.Log("힐스킬");
    }
    public void SkillLifeSteal()
    {
        // 생명력 흡수 버프 
        lifeStealTime = 30f; // 30초
        lifeSteal = 0.1f; // 피해량 10%
        buffEffect.LifeStealEffect(transform);
    }
    public void SkillShieldRecovery()
    {
        // 쉴드 회복량 50%증가
        recoverySheildTime = 30f;
        recoveryShield = 0.5f;
        buffEffect.RecoveryShieldEffect(transform);
    }
    public void SkillAra()
    {
        // 점프 량 25%증가
        araTime = 30f;
        ara = 1.25f;
        buffEffect.AraEffect(transform);
    }
    public void SkillRecycling()
    {
        // 잃은마나 1% 회복
        recyclingTime = 30f;
        recycling = 0.01f;
        buffEffect.RecyclingEffect(transform);
    }
    public void SkillConcentration()
    {
        // 가하는대미지 1.5배
        concentrationTime = 30f;
        concentration = 1.5f;
        buffEffect.ConcentrationEffect(transform);
    }
    public void SkillBlink()
    {
        // 짧은거리 순간이동
        float dir = transform.localScale.x > 0 ? 1 : -1;
        Vector3 newLocation = transform.position + new Vector3(dir*7f,0,0);
        Instantiate(blinkEffect, transform.position, transform.rotation);
        transform.position = newLocation;
        Instantiate(blinkEffect, transform.position, transform.rotation);
        Debug.Log("블링크 스킬");
    }
    // Attack Magic Skill
    public void SkillMagicArrow()
    {
        // 공격력 X 2의 데미지
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(magicArrowEffect, attackPos.position, effectRotation);
        skillEffect.GetComponent<MagicArrowController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (2f+magic_atk) * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillLightningBall()
    {
        // 공격력 X 1.75의 데미지
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(lightningBallEffect, attackPos.position, effectRotation);
        skillEffect.GetComponent<LightningBallController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 1.75f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillLightningArrow()
    {
        // 공격력 X 3.5의 데미지
        float dir = transform.localScale.x > 0 ? -45f : 45f; 
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f,  dir)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(lightningArrowEffect, attackPos.position + new Vector3(0, 15f, 0), effectRotation);
        skillEffect.GetComponent<LightningArrowController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 3.5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillArtifficialSun()
    {
        // 공격력 X 5의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, dir)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(artifficialSunEffect, attackPos.position + new Vector3(5f * dir, 15f, 0), effectRotation);
        skillEffect.GetComponent<ArtifficialSunController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillFireSheild()
    {
        // 공격력 X 1.5의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(fireShieldEffect, attackPos.position + new Vector3(0f,0f, 0), effectRotation);
        skillEffect.GetComponent<FireShieldController>().setDirection(transform.localScale.x);
        skillEffect.transform.GetChild(2).GetComponent<ATK>().damage = atk_damage * 1.5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillBlizzard()
    {
        // 공격력 X 2의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(blizzardEffect, attackPos.position + new Vector3(dir*15f, 5f, 0), effectRotation);
        skillEffect.GetComponent<BlizzardController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 2f * elemental_atk * concentration / 4f;
        animator.SetTrigger("attack");
    }

    public void SkillMagicCircle()
    {
        // 공격력 X 2의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(magicCircleEffect, attackPos.position + new Vector3(0, 5f, 0), effectRotation);
        skillEffect.GetComponent<MagicCircleController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (2f + magic_atk) * concentration / 4f ;
        animator.SetTrigger("attack");
    }

    public void SkillAltin()
    {
        // 공격력 X 2.5의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        for (int i = 0; i < 5; i++)
        {
            Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, dir*12.5f*i)); // 쿼터니언 오일러각 사용
            GameObject skillEffect = Instantiate(altinEffect, attackPos.position + new Vector3(0, 2f, 0), effectRotation);
            skillEffect.GetComponent<AltinController>().setDirection(transform.localScale.x);
            skillEffect.GetComponent<ATK>().damage = atk_damage * (2.5f + magic_atk) * concentration;
        }
        animator.SetTrigger("attack");
    }

    public void SkillHoly()
    {
        // 공격력 X 5.5의 데미지
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
        GameObject skillEffect = Instantiate(holyEffect, attackPos.position + new Vector3(0, 0f, 0), effectRotation);
        skillEffect.GetComponent<HolyController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (5.5f + magic_atk) * concentration;
        animator.SetTrigger("attack");
    }

    public float HpRatio()
    {
        return (float)hp / MAX_HP;
    }
    // 스텟 조절
    public float DecreaseMana(float mana)
    {
        this.mana -= mana * cycle;
        audioSk.clip = audioSkill;
        audioSk.Play();
        return mana;
    }
    public float SheildRatio()
    {
        return (float)shield / MAX_SHIELD;
    }
    void UIUpdate()
    {
        //hptext.text = $"{hp}/{MAX_HP}";
        //sheildtext.text = $"{shield}/{MAX_SHIELD}";
        StatHpText.text = $"{MAX_HP}";
        StatManaText.text = $"{MAX_MANA}";
        StatSheildText.text = $"{MAX_SHIELD}";
        StatAtkText.text = $"{atk_damage}";
        StatArmorText.text = $"{armor}";
        StatPointText.text = $"SP : {statPoints}";
    }
    public void Damaged(float damage)
    {
        float dam = damage-armor;
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
        animator.SetTrigger("hurt");
        audioMoveJumpATK.clip = audioHit;
        audioMoveJumpATK.Play();
    }
    public void IncreaseExp(float exp_)
    {
        this.exp += exp_;
        //레벨당 요구경험치  50씩 추가
        if (exp > 50 + 50*level)
        {
            exp -= 50 + 50 * level;
            float dir = transform.localScale.x > 0 ? 1f : -1f;
            Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // 쿼터니언 오일러각 사용
            GameObject skillEffect = Instantiate(levelUpEffect, attackPos.position + new Vector3(0f, 0f, 0), effectRotation);
            level += 1;
            skillPoints += 2;
            statPoints += 2;
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

    public void StatHp()
    {
        if(statPoints>0)
        {
            statPoints--;
            MAX_HP += 150;
            hp += 150;
        }
    }
    public void StatSheild()
    {
        if (statPoints > 0)
        {
            statPoints--;
            MAX_SHIELD += 300;
            shield += 300;
        }
    }

    public void StatMana()
    {
        if (statPoints > 0)
        {
            statPoints--;
            MAX_MANA += 300;
            mana += 300;
        }
    }

    public void StatAttack()
    {
        if (statPoints > 0)
        {
            statPoints--;
            atk_damage += 50;
        }
    }

    public void StatArmor()
    {
        if (statPoints > 0)
        {
            statPoints--;
            armor += 10;
        }
    }
}
