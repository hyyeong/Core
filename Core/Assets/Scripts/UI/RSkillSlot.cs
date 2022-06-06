using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class RSkillSlot : MonoBehaviour, IDropHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public PlayerController player;
    public Text CoolText;
    public Image Cool;

    public AudioClip slotAudioClip;
    AudioSource slotAudio;

    bool enable = false;
    float ratio;
    void Start()
    {
        slotAudio = gameObject.AddComponent<AudioSource>();
        slotAudio.loop = false;
        slotAudio.clip = slotAudioClip;

    }

    void Update()
    {
        if (enable)
        {
            ratio = 1f - (player.currentRCoolTime / player.rCoolTime);
            if (player.currentRCoolTime > 0)
            {
                CoolText.text = $"{Math.Round(player.currentRCoolTime, 1)}";
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
            player.RSkill = dragAndDropContainer.skillfunc; // 스킬 지정
            player.rMana = dragAndDropContainer.mana;
            skillImage.sprite = dragAndDropContainer.image.sprite;
            player.rCoolTime = dragAndDropContainer.cooltime;
            dragAndDropContainer.image.sprite = null;
            SoundEffect();
            enable = true;
        }
    }
    void SoundEffect()
    {
        slotAudio.Play();

    }
}

