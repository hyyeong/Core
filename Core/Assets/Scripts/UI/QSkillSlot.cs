using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class QSkillSlot : MonoBehaviour, IDropHandler
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
            ratio = 1f - (player.currentQCoolTime / player.qCoolTime);
            if (player.currentQCoolTime > 0)
            {
                CoolText.text = $"{Math.Round(player.currentQCoolTime, 1)}";
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
            player.QSkill = dragAndDropContainer.skillfunc; // ��ų ����
            player.qMana = dragAndDropContainer.mana;
            skillImage.sprite = dragAndDropContainer.image.sprite;
            player.qCoolTime = dragAndDropContainer.cooltime;
            dragAndDropContainer.image.sprite = null;
            SoundEffect();
            enable = true;
        }
    }
    void SoundEffect()
    {

    }
}
