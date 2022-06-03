using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    //Outlab 접속
    public Button healButton;
    public Button healthEnforceButton;
    public Button armorEnforceButton;
    PlayerController player;
    public Text skillPointText;
    public Text skillAlert;
    public GameObject Alert;
    // 스킬 공통
    bool healCondition = false; // 스킬을 찍기위한 사전조건
    bool armorEnforceCondition = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    void Update()
    {
        skillPointText.text = $"SkillPoint:{player.skillPoints}";
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
            float health = 250;
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
            float armor = 5f;
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
            skillAlert.text = $"회복스킬을 배웠다!";

            soundEffect();
        }
    }

    public void soundEffect()
    {
        Alert.gameObject.SetActive(true);
        Alert.GetComponent<SkillAlertController>().time = 0f;
    }
}
