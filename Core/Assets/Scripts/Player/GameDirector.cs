using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    //Outlab ����
    public Button healButton;
    public Button healthEnforceButton;
    public Button armorEnforceButton;
    PlayerController player;
    public Text skillPointText;
    public Text skillAlert;
    public GameObject Alert;
    // ��ų ����
    bool healCondition = false; // ��ų�� ������� ��������
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
            //��ư ��Ȱ��ȭ
            healthEnforceButton.interactable = false;
            //��ų Ȱ��ȭ
            healCondition = true;
            armorEnforceCondition = true;
            // �нú� �ΰ�ȿ��
            float health = 250;
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
            float armor = 5f;
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
            skillAlert.text = $"ȸ����ų�� �����!";

            soundEffect();
        }
    }

    public void soundEffect()
    {
        Alert.gameObject.SetActive(true);
        Alert.GetComponent<SkillAlertController>().time = 0f;
    }
}
