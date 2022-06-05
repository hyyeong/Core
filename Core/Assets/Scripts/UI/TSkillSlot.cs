using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TSkillSlot : MonoBehaviour, IDropHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public PlayerController player;
    public Text CoolText;
    public Image Cool;

    bool enable = false;
    float ratio;
    void Start()
    {

    }

    void Update()
    {
        if (enable)
        {
            ratio = 1f - (player.currentTCoolTime / player.tCoolTime);
            if (player.currentTCoolTime > 0)
            {
                CoolText.text = $"{Math.Round(player.currentTCoolTime, 1)}";
                Cool.fillAmount = 1 - ratio;
            }
            else
            {
                CoolText.text = "";
                Cool.fillAmount = 0f;
            }
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragAndDropContainer.isActiveAndEnabled)
        {
            player.TSkill = dragAndDropContainer.skillfunc; // 스킬 지정
            player.tMana = dragAndDropContainer.mana;
            skillImage.sprite = dragAndDropContainer.image.sprite;
            player.tCoolTime = dragAndDropContainer.cooltime;
            dragAndDropContainer.image.sprite = null;
            SoundEffect();
            enable = true;
        }
    }
    void SoundEffect()
    {

    }
}
