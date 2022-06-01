using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillSlot : MonoBehaviour, IDropHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public PlayerController player;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        player.QSkill = dragAndDropContainer.skillfunc; // 스킬 지정
        player.qMana = dragAndDropContainer.mana;
        skillImage.sprite = dragAndDropContainer.image.sprite;
        player.qCoolTime = dragAndDropContainer.cooltime;
        dragAndDropContainer.image.sprite = null;
    }
}
