using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameDirector : MonoBehaviour
{
    public Button healSkillButton;
    PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    void Update()
    {
        
    }

    public void LearnHealSkill()
    {
        player.MAX_HP += 50;
        //버튼 비활성화
        healSkillButton.interactable = false;
    }
}
