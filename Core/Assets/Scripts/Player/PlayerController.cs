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
    // ��ǥ, ���ϸ����� ��
    Rigidbody2D rigid2D;
    Animator animator;

    const int MAX_JUMP = 2; // 2����������
    int isJump = 0; // �������� üũ ����
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
    public BuffEffectGenerator buffEffect; // ���� ����Ʈ ����
    // �̵� ����
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

    //���� ����
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
    public float cycle { set; get; } = 1f; // �����Ҹ� ����
    float hp ;
    float shield ;
    float mana ;
    // ���Ȱ� �нú�
    public float elemental_atk { set; get; } = 1f;
    public float magic_atk { set; get; } = 0f;
    public bool arcana { set; get; } = false;
    //ȯ��( ����ġ, ���� ��)
    public int level { get; set; } = 1;
    float exp = 0;

    // ��ų ����
    public int statPoints = 10;
    public int skillPoints = 10;
    public SkillSet QSkill { set; get; }
    public SkillSet WSkill { set; get; }
    public SkillSet ESkill { set; get; }
    public SkillSet RSkill { set; get; }
    public SkillSet TSkill { set; get; }
    // ������ð��� ��ų�� ���� �����ϱ�
    public float qCoolTime { get; set; } = 3f; // ���� ���ð�
    public float wCoolTime { get; set; } = 2f;
    public float eCoolTime { get; set; } = 3f;
    public float rCoolTime { get; set; } = 3f;
    public float tCoolTime { get; set; } = 3f;
    public float currentQCoolTime { get; set; } = 0;// ���� ���� ���ð�
    public float currentWCoolTime { get; set; } = 0;
    public float currentECoolTime { get; set; } = 0;
    public float currentRCoolTime { get; set; } = 0;
    public float currentTCoolTime { get; set; } = 0;

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
    float invincibility = 0f;
    // �޼ҵ�
    void Start()
    {
        this.audioMoveJumpATK = gameObject.AddComponent<AudioSource>();
        audioMoveJumpATK.loop = false;
        audioSk = gameObject.AddComponent<AudioSource>();
        audioSk.loop = false;
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
        QSkill = new SkillSet(EmptySkill);
        WSkill = new SkillSet(EmptySkill);
        ESkill = new SkillSet(EmptySkill);
        RSkill = new SkillSet(EmptySkill);
        TSkill = new SkillSet(EmptySkill);

        // �ڷ�ƾ ����
        StartCoroutine(ManageBuff());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Map")||other.CompareTag("Trap"))
        {
            this.animator.SetBool("isJump", false);
            audioMoveJumpATK.clip = audioJump;
            audioMoveJumpATK.Play();
            isJump = 0;// ����Ƚ�� 0
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
        // ���� üũ
        RecoveryShield(recoverySheildPerSec * Time.deltaTime);
        RecoveryMana(recoveryManaPerSec * Time.deltaTime);
        Die();
        CheckSkill();
        UIUpdate();
        // �ӵ� ����
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        BGspeed = (speedx > 0f) ? 0.1f * key * (maxWalkSpeed/3.0f): 0f;
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
/* ĳ���� �ൿ �޼ҵ�*/
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
    // ��ų�� ����ִ� ���
    void EmptySkill()
    {
        Debug.Log("�ش� ��ų�� ����ֽ��ϴ�.");
    }
    // ��ų��
    // ������ų
    public void SkillHeal()
    {
        // ���ݷ� * 2 ��ŭ ȸ��
        hp += atk_damage * 2;
        Quaternion rotate = Quaternion.Euler(-90, 0, 0);
        Instantiate(healEffect, transform.position, rotate);
        Debug.Log("����ų");
    }
    public void SkillLifeSteal()
    {
        // ����� ��� ���� 
        lifeStealTime = 30f; // 30��
        lifeSteal = 0.1f; // ���ط� 10%
        buffEffect.LifeStealEffect(transform);
    }
    public void SkillShieldRecovery()
    {
        // ���� ȸ���� 50%����
        recoverySheildTime = 30f;
        recoveryShield = 0.5f;
        buffEffect.RecoveryShieldEffect(transform);
    }
    public void SkillAra()
    {
        // ���� �� 25%����
        araTime = 30f;
        ara = 1.25f;
        buffEffect.AraEffect(transform);
    }
    public void SkillRecycling()
    {
        // �������� 1% ȸ��
        recyclingTime = 30f;
        recycling = 0.01f;
        buffEffect.RecyclingEffect(transform);
    }
    public void SkillConcentration()
    {
        // ���ϴ´���� 1.5��
        concentrationTime = 30f;
        concentration = 1.5f;
        buffEffect.ConcentrationEffect(transform);
    }
    public void SkillBlink()
    {
        // ª���Ÿ� �����̵�
        float dir = transform.localScale.x > 0 ? 1 : -1;
        Vector3 newLocation = transform.position + new Vector3(dir*7f,0,0);
        Instantiate(blinkEffect, transform.position, transform.rotation);
        transform.position = newLocation;
        Instantiate(blinkEffect, transform.position, transform.rotation);
        Debug.Log("��ũ ��ų");
    }
    // Attack Magic Skill
    public void SkillMagicArrow()
    {
        // ���ݷ� X 2�� ������
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(magicArrowEffect, attackPos.position, effectRotation);
        skillEffect.GetComponent<MagicArrowController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (2f+magic_atk) * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillLightningBall()
    {
        // ���ݷ� X 1.75�� ������
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(lightningBallEffect, attackPos.position, effectRotation);
        skillEffect.GetComponent<LightningBallController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 1.75f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillLightningArrow()
    {
        // ���ݷ� X 3.5�� ������
        float dir = transform.localScale.x > 0 ? -45f : 45f; 
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f,  dir)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(lightningArrowEffect, attackPos.position + new Vector3(0, 15f, 0), effectRotation);
        skillEffect.GetComponent<LightningArrowController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 3.5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillArtifficialSun()
    {
        // ���ݷ� X 5�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, dir)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(artifficialSunEffect, attackPos.position + new Vector3(5f * dir, 15f, 0), effectRotation);
        skillEffect.GetComponent<ArtifficialSunController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillFireSheild()
    {
        // ���ݷ� X 1.5�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(fireShieldEffect, attackPos.position + new Vector3(0f,0f, 0), effectRotation);
        skillEffect.GetComponent<FireShieldController>().setDirection(transform.localScale.x);
        skillEffect.transform.GetChild(2).GetComponent<ATK>().damage = atk_damage * 1.5f * elemental_atk * concentration;
        animator.SetTrigger("attack");
    }
    public void SkillBlizzard()
    {
        // ���ݷ� X 2�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(blizzardEffect, attackPos.position + new Vector3(dir*15f, 5f, 0), effectRotation);
        skillEffect.GetComponent<BlizzardController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * 2f * elemental_atk * concentration / 4f;
        animator.SetTrigger("attack");
    }

    public void SkillMagicCircle()
    {
        // ���ݷ� X 2�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(magicCircleEffect, attackPos.position + new Vector3(0, 5f, 0), effectRotation);
        skillEffect.GetComponent<MagicCircleController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (2f + magic_atk) * concentration / 4f ;
        animator.SetTrigger("attack");
    }

    public void SkillAltin()
    {
        // ���ݷ� X 2.5�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        for (int i = 0; i < 5; i++)
        {
            Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, dir*12.5f*i)); // ���ʹϾ� ���Ϸ��� ���
            GameObject skillEffect = Instantiate(altinEffect, attackPos.position + new Vector3(0, 2f, 0), effectRotation);
            skillEffect.GetComponent<AltinController>().setDirection(transform.localScale.x);
            skillEffect.GetComponent<ATK>().damage = atk_damage * (2.5f + magic_atk) * concentration;
        }
        animator.SetTrigger("attack");
    }

    public void SkillHoly()
    {
        // ���ݷ� X 5.5�� ������
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
        GameObject skillEffect = Instantiate(holyEffect, attackPos.position + new Vector3(0, 0f, 0), effectRotation);
        skillEffect.GetComponent<HolyController>().setDirection(transform.localScale.x);
        skillEffect.GetComponent<ATK>().damage = atk_damage * (5.5f + magic_atk) * concentration;
        animator.SetTrigger("attack");
    }

    public float HpRatio()
    {
        return (float)hp / MAX_HP;
    }
    // ���� ����
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
        //������ �䱸����ġ  50�� �߰�
        if (exp > 50 + 50*level)
        {
            exp -= 50 + 50 * level;
            float dir = transform.localScale.x > 0 ? 1f : -1f;
            Quaternion effectRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f)); // ���ʹϾ� ���Ϸ��� ���
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
            mana += (MAX_MANA - mana) * recycling * Time.deltaTime; // ��������
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
