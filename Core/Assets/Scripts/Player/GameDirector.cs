using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    //Outlab ����
    public Button healButton;
    public Button healthEnforceButton;
    public Button armorEnforceButton;
    public Button shieldEnforceButton;
    public Button lifeStealButton;
    public Button sheildRecoveryEnforceButton;
    public Button sheildRecoveryButton;

    public Button concentrateButton;
    public Button arcanaButton;
    public Button esbatButton;
    public Button wingButton;
    public Button araButton;
    public Button blinkButton;
    public Button wandButton;
    public Button cycleButton;
    public Button manaDrainButton;
    public Button recyclingButton;

    public Button magicArrowButton;
    public Button altinButton;
    public Button holyButton;
    public Button magicCircleButton;
    public Button magicEnforceButton;
    public Button lightningBallButton;
    public Button lightningArrowButton;
    public Button artifficialSunButton;
    public Button fireSheildButton;
    public Button blizzardButton;
    public Button elementalEnforceButton;

    PlayerController player;
    public Text skillPointText;
    public Text skillAlert;
    public GameObject Alert;

    // �Ѿ���� ����
    public GameObject Player;
    public Canvas Canvas;
    public GameObject BuffEffectGenerator;
    public AudioClip learnAudioClip;
    AudioSource learnAudio;
    [System.Serializable]
    public struct Bgm
    {
        public string name;
        public AudioClip audio;
    }
    public Bgm[] BGMList;
    int bgmLoop = 0;
    private AudioSource BGM;
    private string NowBGMname = "";
    public float volume=0.5f;

    // ��ų ����
    bool healCondition = false; // ��ų�� ������� ��������
    bool armorEnforceCondition = false;
    bool lifeStealCondition = false;
    bool sheildRecoveryCondition = false;
    bool sheildRecvoeryEnforceCondition = false;

    bool arcanaCondition = false;
    bool esbatCondition = false;
    bool araCondition = false;
    bool blinkCondition = false;
    bool cycleCondition = false;
    bool manaDrainCondition = false;
    bool recycleCondition = false;

    bool altinCondition = false;
    bool magicCircleCondition = false;
    bool holyCondition = false;
    bool magicEnforceCondition = false;
    bool lightningArrowCondition = false;
    bool artifficialSunCondition = false;
    bool fireSheildCondition = false;
    bool blizzardCondition = false;
    bool elementalEnforceCondition = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>(); 
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = false;
        learnAudio = gameObject.AddComponent<AudioSource>();
        learnAudio.loop = false;
        learnAudio.clip = learnAudioClip;

        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Canvas);
        DontDestroyOnLoad(BuffEffectGenerator);
    }

    void Update()
    {
        skillPointText.text = $"SkillPoint:{player.skillPoints}";
        if (!BGM.isPlaying)
        {
            BGM.clip = BGMList[bgmLoop].audio;
            BGM.volume = volume;
            BGM.Play();
            NowBGMname = name;
            bgmLoop++;
            if (bgmLoop >= BGMList.Length)
                bgmLoop = 0;
        }
    }
    public void LoadStage2()
    {
        Destroy(GameObject.Find("Enemy"));
        for (int i = 0; i < Canvas.transform.childCount; i++) 
        {
            if (Canvas.transform.GetChild(i).CompareTag("hpbar"))
            {
                Destroy(Canvas.transform.GetChild(i).gameObject);
            }
        }

        SceneManager.LoadScene("Stage2");
        Player.transform.position = new Vector3(0, -5, 0);
    }
    public void LoadStage3()
    {
        Destroy(GameObject.Find("Enemy"));
        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            if (Canvas.transform.GetChild(i).CompareTag("hpbar"))
            {
                Destroy(Canvas.transform.GetChild(i).gameObject);
            }
        }

        SceneManager.LoadScene("Stage3");
        Player.transform.position = new Vector3(0, -5, 0);
    }
    public void DestroyObject()
    {
        Destroy(Player);
        Destroy(this);
        Destroy(Canvas.gameObject);
        Destroy(BuffEffectGenerator);
    }
    public void LearnElementalEnforce()
    {
        if (player.skillPoints > 0 && elementalEnforceCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            elementalEnforceButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            float ele = 0.33f;
            player.elemental_atk += ele;
            //�˸�
            skillAlert.text = $"���Ҹ����� �������� {ele*100}% ��ŭ ����մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnBlizzard()
    {
        if (player.skillPoints > 0 && blizzardCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            blizzardButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"Blizzard : Ÿ�ݴ� ���ݷ� X 2�� �������� �ִ� ������ �����մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnFireSheild()
    {
        if (player.skillPoints > 0 && fireSheildCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            fireSheildButton.interactable = false;
            //��ų Ȱ��ȭ
            blizzardCondition = true;
            elementalEnforceCondition = true;
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"FireSheild : ��ü�� ���ݷ� X 1.5�� �������� �ִ� ȭ����ü 6���� �����մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnArtifficialSun()
    {
        if (player.skillPoints > 0 && artifficialSunCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            artifficialSunButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"Artifficial Sun : ���ݷ� X 5�� �������� �ִ� �¾��� ��ȯ�մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnLightningArrow()
    {
        if (player.skillPoints > 0&& lightningArrowCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            lightningArrowButton.interactable = false;
            //��ų Ȱ��ȭ
            fireSheildCondition = true;
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"LightningArrow : ���ݷ� X 3.5�� �������� �ִ� ����ȭ���� ��ȯ�մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnLightningBall()
    {
        if (player.skillPoints > 0 )
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            lightningBallButton.interactable = false;
            //��ų Ȱ��ȭ
            lightningArrowCondition = true;
            artifficialSunCondition = true;
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"LightningBall : ���ݷ� X 1.75�� �������� �ִ� ���� ��ü�� ��ȯ�մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnMagicEnforce()
    {
        if (player.skillPoints > 0 && magicEnforceCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            magicEnforceButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            float matk = 2.0f;
            player.magic_atk += matk;
            //�˸�
            skillAlert.text = $"���� ���ݷ��� ���ݷ°���� {matk}��ŭ �����մϴ�. ";
            // ����ȿ����
            soundEffect();
        }
    }

    public void LearnHoly()
    {
        if (player.skillPoints > 0 && holyCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            holyButton.interactable = false;
            //��ų Ȱ��ȭ
            magicEnforceCondition = true;
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"Holy : �ż����� �����Ͽ� ���ݷ� X 5.5�� �������� ������ ���� ���� ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnMagicCircle()
    {
        if (player.skillPoints > 0 && magicCircleCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            magicCircleButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"MagicCircle : ���ݷ� X 2�� �������� ������ �������� ��ȯ ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnAltin()
    {
        if (player.skillPoints > 0 && altinCondition) 
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            altinButton.interactable = false;
            //��ų Ȱ��ȭ
            holyCondition = true;

            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"Altin : ���ݷ� X 2.5�� �������� ������ ��ü 5�� ���� ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnMagicArrow()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            magicArrowButton.interactable = false;
            //��ų Ȱ��ȭ

            altinCondition = true;
            magicCircleCondition = true;

            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"MagicArrow : ���ݷ� X 3�� �������� ������ ȭ�� ��ȯ ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnCycling()
    {
        if (player.skillPoints > 0 && cycleCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            cycleButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            float cyc = 0.25f;
            player.cycle -= cyc;
            //�˸�
            skillAlert.text = $"�����Ҹ� {cyc * 100}% ��ŭ ����";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnRecycling()
    {
        if (player.skillPoints > 0 && recycleCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            recyclingButton.interactable = false;
            //��ų Ȱ��ȭ
            cycleCondition = true;
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"recycling : ���ӽð����� ���� ������ ������ ȸ��";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnManaDrain()
    {
        if (player.skillPoints > 0 && manaDrainCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            manaDrainButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            float drain = 0.01f;
            player.manaDrain += drain;
            //�˸�
            skillAlert.text = $"��������� �������ط��� {drain*100}%��ŭ ���� ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnWand()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            wandButton.interactable = false;
            //��ų Ȱ��ȭ
            manaDrainCondition = true;
            recycleCondition = true;
            // �нú� �ΰ�ȿ��
            float mana = 8f;
            player.recoveryManaPerSec += mana;
            //�˸�
            skillAlert.text = $"����ȸ���� �ʴ� {mana} ��ŭ ���� ";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnBlink()
    {
        if (player.skillPoints > 0 && blinkCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            blinkButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"blink : �����Ÿ� �����̵�.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnAra()
    {
        if (player.skillPoints > 0 && araCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            araButton.interactable = false;
            //��ų Ȱ��ȭ
            // �нú� �ΰ�ȿ��
            //�˸�
            skillAlert.text = $"Ara : �����ӵ� ��ȭ.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnWing()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            wingButton.interactable = false;
            //��ų Ȱ��ȭ

            araCondition = true;
            blinkCondition = true;
            // �нú� �ΰ�ȿ��
            player.walkForce += 0f;
            player.maxWalkSpeed += 3.0f;
            //�˸�
            skillAlert.text = $"�̵��ӵ��� �����߽��ϴ�.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnEsbat()
    {
        if (player.skillPoints > 0 && esbatCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            esbatButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��
            player.attack_speed -= 0.55f;
            //�˸�
            skillAlert.text = $"���ݼӵ��� 1�ʿ��� 0.25�ʷ� ����Ǿ����ϴ�.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnArcana()
    {
        if (player.skillPoints > 0 && arcanaCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            arcanaButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��
            player.arcana = true;
            //�˸�
            skillAlert.text = $"��������ü�� �߰������� �߻��մϴ�.";
            // ����ȿ����
            soundEffect();
        }
    }

    public void LearnConcentrate()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            concentrateButton.interactable = false;
            //��ų Ȱ��ȭ
            arcanaCondition = true;
            esbatCondition = true;

            // �нú� �ΰ�ȿ��

            //�˸�
            skillAlert.text = $"Concentration : �����ð����� ���ݷ��� �����մϴ�.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnSheildRecovery()
    {
        if (player.skillPoints > 0 && sheildRecoveryCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            sheildRecoveryButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��

            //�˸�
            skillAlert.text = $"SheildRecovery : ������� 50% ����";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnSheildRecoveryEnforce()
    {
        if (player.skillPoints > 0 && sheildRecvoeryEnforceCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            sheildRecoveryEnforceButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��
            float recovery = 100f;
            player.recoverySheildPerSec+= recovery;
            //�˸�
            skillAlert.text = $"������� �ʴ� ${recovery} ����.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnlifeSteal()
    {
        if (player.skillPoints > 0 && lifeStealCondition)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            lifeStealButton.interactable = false;
            //��ų Ȱ��ȭ

            // �нú� �ΰ�ȿ��

            //�˸�
            skillAlert.text = $"lifeSteal : �������ط��� �Ϻθ� ȸ��.";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnSheildEnforce()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            shieldEnforceButton.interactable = false;
            //��ų Ȱ��ȭ
            lifeStealCondition = true;
            sheildRecoveryCondition = true;
            sheildRecvoeryEnforceCondition = true;
            // �нú� �ΰ�ȿ��
            float sheild = 500;
            player.MAX_SHIELD += sheild;

            //�˸�
            skillAlert.text = $"�÷��̾� ���� {sheild} ����";
            // ����ȿ����
            soundEffect();
        }
    }

    public void LearnHealthEnforce()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            healthEnforceButton.interactable = false;
            //��ų Ȱ��ȭ
            healCondition = true;
            armorEnforceCondition = true;
            // �нú� �ΰ�ȿ��
            float health = 500;
            player.MAX_HP += health;

            //�˸�
            skillAlert.text = $"�÷��̾� ü�� {health} ����";
            // ����ȿ����
            soundEffect();
        }
    }
    public void LearnArmor()
    {
        if (armorEnforceCondition && player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            armorEnforceButton.interactable = false;
            //�нú� �ΰ�ȿ��
            float armor = 100f;
            player.armor +=armor;

            //�˸�
            skillAlert.text=$"�÷��̾� ���� {armor} ����";
            //����ȿ����
            soundEffect();
        }
    }

    public void LearnHeal()
    {
        if (healCondition && player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //��ư ��Ȱ��ȭ
            healButton.interactable = false;

            //�нú� �ΰ�ȿ��

            //�˸�
            skillAlert.text = $"Heal : ü���� ȸ��";

            soundEffect();
        }
    }

    public void soundEffect()
    {
        Alert.gameObject.SetActive(true);
        Alert.GetComponent<SkillAlertController>().time = 0f;
        learnAudio.Play();
    }
}
