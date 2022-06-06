using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class ESkillSlot : MonoBehaviour, IDropHandler
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
            ratio = 1f - (player.currentECoolTime / player.eCoolTime);
            if (player.currentECoolTime > 0)
            {
                CoolText.text = $"{Math.Round(player.currentECoolTime, 1)}";
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
            player.ESkill = dragAndDropContainer.skillfunc; // 스킬 지정
            player.eMana = dragAndDropContainer.mana;
            skillImage.sprite = dragAndDropContainer.image.sprite;
            player.eCoolTime = dragAndDropContainer.cooltime;
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
