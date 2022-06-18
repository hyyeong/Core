using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    //Outlab 접속
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

    // 넘어갈때도 유지
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

    // 스킬 공통
    bool healCondition = false; // 스킬을 찍기위한 사전조건
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
            //버튼 비활성화
            elementalEnforceButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            float ele = 0.33f;
            player.elemental_atk += ele;
            //알림
            skillAlert.text = $"원소마법의 데미지가 {ele*100}% 만큼 상승합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnBlizzard()
    {
        if (player.skillPoints > 0 && blizzardCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            blizzardButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            //알림
            skillAlert.text = $"Blizzard : 타격당 공격력 X 2의 데미지를 주는 눈보라를 생성합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnFireSheild()
    {
        if (player.skillPoints > 0 && fireSheildCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            fireSheildButton.interactable = false;
            //스킬 활성화
            blizzardCondition = true;
            elementalEnforceCondition = true;
            // 패시브 부과효과
            //알림
            skillAlert.text = $"FireSheild : 구체당 공격력 X 1.5의 데미지를 주는 화염구체 6개를 생성합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnArtifficialSun()
    {
        if (player.skillPoints > 0 && artifficialSunCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            artifficialSunButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과
            //알림
            skillAlert.text = $"Artifficial Sun : 공격력 X 5의 데미지를 주는 태양을 소환합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnLightningArrow()
    {
        if (player.skillPoints > 0&& lightningArrowCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            lightningArrowButton.interactable = false;
            //스킬 활성화
            fireSheildCondition = true;
            // 패시브 부과효과
            //알림
            skillAlert.text = $"LightningArrow : 공격력 X 3.5의 데미지를 주는 번개화살을 소환합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnLightningBall()
    {
        if (player.skillPoints > 0 )
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            lightningBallButton.interactable = false;
            //스킬 활성화
            lightningArrowCondition = true;
            artifficialSunCondition = true;
            // 패시브 부과효과
            //알림
            skillAlert.text = $"LightningBall : 공격력 X 1.75의 데미지를 주는 번개 구체를 소환합니다. ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnMagicEnforce()
    {
        if (player.skillPoints > 0 && magicEnforceCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            magicEnforceButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            float matk = 2.0f;
            player.magic_atk += matk;
            //알림
            skillAlert.text = $"마법 공격력의 공격력계수가 {matk}만큼 증가합니다. ";
            // 공통효과음
            soundEffect();
        }
    }

    public void LearnHoly()
    {
        if (player.skillPoints > 0 && holyCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            holyButton.interactable = false;
            //스킬 활성화
            magicEnforceCondition = true;
            // 패시브 부과효과
            //알림
            skillAlert.text = $"Holy : 신성력을 방출하여 공격력 X 5.5의 데미지를 주위의 적에 입힘 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnMagicCircle()
    {
        if (player.skillPoints > 0 && magicCircleCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            magicCircleButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과
            //알림
            skillAlert.text = $"MagicCircle : 공격력 X 2의 데미지를 입히는 마법진들 소환 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnAltin()
    {
        if (player.skillPoints > 0 && altinCondition) 
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            altinButton.interactable = false;
            //스킬 활성화
            holyCondition = true;

            // 패시브 부과효과
            //알림
            skillAlert.text = $"Altin : 공격력 X 2.5의 데미지를 입히는 구체 5개 생성 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnMagicArrow()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            magicArrowButton.interactable = false;
            //스킬 활성화

            altinCondition = true;
            magicCircleCondition = true;

            // 패시브 부과효과
            //알림
            skillAlert.text = $"MagicArrow : 공격력 X 3의 데미지를 입히는 화살 소환 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnCycling()
    {
        if (player.skillPoints > 0 && cycleCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            cycleButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            float cyc = 0.25f;
            player.cycle -= cyc;
            //알림
            skillAlert.text = $"마나소모량 {cyc * 100}% 만큼 감소";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnRecycling()
    {
        if (player.skillPoints > 0 && recycleCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            recyclingButton.interactable = false;
            //스킬 활성화
            cycleCondition = true;
            // 패시브 부과효과
            //알림
            skillAlert.text = $"recycling : 지속시간동안 잃은 마나의 일정량 회복";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnManaDrain()
    {
        if (player.skillPoints > 0 && manaDrainCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            manaDrainButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            float drain = 0.01f;
            player.manaDrain += drain;
            //알림
            skillAlert.text = $"마나흡수량 가한피해량의 {drain*100}%만큼 증가 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnWand()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            wandButton.interactable = false;
            //스킬 활성화
            manaDrainCondition = true;
            recycleCondition = true;
            // 패시브 부과효과
            float mana = 8f;
            player.recoveryManaPerSec += mana;
            //알림
            skillAlert.text = $"마나회복량 초당 {mana} 만큼 증가 ";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnBlink()
    {
        if (player.skillPoints > 0 && blinkCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            blinkButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            //알림
            skillAlert.text = $"blink : 일정거리 순간이동.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnAra()
    {
        if (player.skillPoints > 0 && araCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            araButton.interactable = false;
            //스킬 활성화
            // 패시브 부과효과
            //알림
            skillAlert.text = $"Ara : 점프속도 강화.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnWing()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            wingButton.interactable = false;
            //스킬 활성화

            araCondition = true;
            blinkCondition = true;
            // 패시브 부과효과
            player.walkForce += 0f;
            player.maxWalkSpeed += 3.0f;
            //알림
            skillAlert.text = $"이동속도가 증가했습니다.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnEsbat()
    {
        if (player.skillPoints > 0 && esbatCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            esbatButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과
            player.attack_speed -= 0.55f;
            //알림
            skillAlert.text = $"공격속도가 1초에서 0.25초로 변경되었습니다.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnArcana()
    {
        if (player.skillPoints > 0 && arcanaCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            arcanaButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과
            player.arcana = true;
            //알림
            skillAlert.text = $"공격투사체가 추가적으로 발생합니다.";
            // 공통효과음
            soundEffect();
        }
    }

    public void LearnConcentrate()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            concentrateButton.interactable = false;
            //스킬 활성화
            arcanaCondition = true;
            esbatCondition = true;

            // 패시브 부과효과

            //알림
            skillAlert.text = $"Concentration : 일정시간동안 공격력이 증가합니다.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnSheildRecovery()
    {
        if (player.skillPoints > 0 && sheildRecoveryCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            sheildRecoveryButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과

            //알림
            skillAlert.text = $"SheildRecovery : 쉴드재생 50% 증가";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnSheildRecoveryEnforce()
    {
        if (player.skillPoints > 0 && sheildRecvoeryEnforceCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            sheildRecoveryEnforceButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과
            float recovery = 100f;
            player.recoverySheildPerSec+= recovery;
            //알림
            skillAlert.text = $"쉴드재생 초당 ${recovery} 증가.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnlifeSteal()
    {
        if (player.skillPoints > 0 && lifeStealCondition)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            lifeStealButton.interactable = false;
            //스킬 활성화

            // 패시브 부과효과

            //알림
            skillAlert.text = $"lifeSteal : 가한피해량의 일부를 회복.";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnSheildEnforce()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            shieldEnforceButton.interactable = false;
            //스킬 활성화
            lifeStealCondition = true;
            sheildRecoveryCondition = true;
            sheildRecvoeryEnforceCondition = true;
            // 패시브 부과효과
            float sheild = 500;
            player.MAX_SHIELD += sheild;

            //알림
            skillAlert.text = $"플레이어 쉴드 {sheild} 증가";
            // 공통효과음
            soundEffect();
        }
    }

    public void LearnHealthEnforce()
    {
        if (player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            healthEnforceButton.interactable = false;
            //스킬 활성화
            healCondition = true;
            armorEnforceCondition = true;
            // 패시브 부과효과
            float health = 500;
            player.MAX_HP += health;

            //알림
            skillAlert.text = $"플레이어 체력 {health} 증가";
            // 공통효과음
            soundEffect();
        }
    }
    public void LearnArmor()
    {
        if (armorEnforceCondition && player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            armorEnforceButton.interactable = false;
            //패시브 부과효과
            float armor = 100f;
            player.armor +=armor;

            //알림
            skillAlert.text=$"플레이어 방어력 {armor} 증가";
            //공통효과음
            soundEffect();
        }
    }

    public void LearnHeal()
    {
        if (healCondition && player.skillPoints > 0)
        {
            player.skillPoints -= 1;
            //버튼 비활성화
            healButton.interactable = false;

            //패시브 부과효과

            //알림
            skillAlert.text = $"Heal : 체력을 회복";

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
