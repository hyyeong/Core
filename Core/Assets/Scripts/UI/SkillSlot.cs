using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillSlot : MonoBehaviour, IDropHandler
{
    public Image skillImage;
    public DragAndDropContainer dragAndDropContainer;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData);
        skillImage.sprite = dragAndDropContainer.image.sprite;
        dragAndDropContainer.image.sprite = null;
    }
}
