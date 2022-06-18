using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillToolTip tooltip;
    public string skillName;
    public bool isPassive;
    void Start()
    {
        tooltip = GameObject.Find("SkillToolTip").GetComponent<SkillToolTip>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowToolTip(skillName, isPassive, eventData.position);
    }

    // 마우스 커서가 슬롯에서 나올 때 발동
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideToolTip();
    }
}
