using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HealSkillUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    public Button btn;
    public PlayerController player;
    public PlayerController.SkillSet skillfunc;
    // 스킬 정보
    public float mana = 0;
    public float cooltime = 0f;
    bool isDragging = false;
    void Start()
    {
        skillImage = btn.image;
        skillfunc = player.SkillHeal;

    }

    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 데이터 담기
        dragAndDropContainer.gameObject.SetActive(true);
        dragAndDropContainer.mana = mana;
        dragAndDropContainer.image.sprite = skillImage.sprite;
        dragAndDropContainer.cooltime = cooltime;
        dragAndDropContainer.skillfunc = skillfunc;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            dragAndDropContainer.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        // 리셋
        dragAndDropContainer.image.sprite = null;
        dragAndDropContainer.transform.position = new Vector3(0, 0, 0);
        dragAndDropContainer.mana = 0;
        dragAndDropContainer.skillfunc = null;
        dragAndDropContainer.gameObject.SetActive(false);
    }
    // 드롭 오브젝트에서 발생
    /*public void OnDrop(PointerEventData eventData)
    {
        if (dragAndDropContainer.image.sprite != null)
        {
            Sprite tempSprite = skillImage.sprite;

            dragAndDropContainer.image.sprite = tempSprite;
        }
        else
        {
            dragAndDropContainer.image.sprite = null;
        }
    }*/
}
