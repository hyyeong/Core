using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private Text txtSkillName;
    [SerializeField]
    private Text txtSkillDesc;

    private PlayerController player;
    private Dictionary<string, string> skillDesc = new Dictionary<string, string>();
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void ShowToolTip(string name,bool isPassive, Vector3 _pos)
    {
        SkillUpdate();
        toolTip.SetActive(true);
        toolTip.transform.position = _pos;
        if (isPassive)
        {
            txtSkillName.text = "​<color=#E83772>──┼ </color><color=449053> " + name + " </color><color=#E83772>┼──</color>";
        }
        else
        {
            txtSkillName.text = "​<color=#3B4F87>──┼ </color> " + name + " <color=#3B4F87>┼──</color>";
        }
        txtSkillDesc.text = skillDesc[name];
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }
    void SkillUpdate()
    {
        float atk_damage = player.atk_damage;
        float magic_atk = player.magic_atk;
        float concentration = player.concentration;
        float elemental_atk = player.elemental_atk;
        skillDesc["HealthEnforce"] = $"<color=#E83772>(Passive)</color>플레이어 체력이 ​<color=#04C357><size=23>{500: 0}</size>만큼</color> 증가한다.";
        skillDesc["ArmorEnforce"] = $"<color=#E83772>(Passive)</color>플레이어 방어력이 ​<color=#BCDF0E><size=23>{100: 0}</size>만큼</color> 증가한다.";
        skillDesc["Heal"] = $"<color=#3B4F87>(Active)</color>플레이어의 체력을 ​<color=#04C357><size=23>{atk_damage * 2f: 0}</size>만큼</color> 회복한다.";
        skillDesc["ShieldEnforce"] = $"<color=#E83772>(Passive)</color>플레이어의 쉴드를 ​<color=#A27F0B><size=23>{500: 0}</size>만큼</color> 증가한다.";
        skillDesc["LifeSteal"] = $"<color=#3B4F87>(Active)</color>30초 동안 적에게 가한 피홰량의 ​<color=#04C357><size=23>{10: 0}</size>%</color>를 체력으로 회복한다.";
        skillDesc["ShieldRecoveryEnforce"] = $"<color=#E83772>(Passive)</color>초당 쉴드 회복량을 ​<color=#A27F0B><size=23>{100: 0}</size>만큼</color>를 증가한다.";
        skillDesc["ShieldRecovery"] = $"<color=#3B4F87>(Active)</color>30초 동안 초당 쉴드 회복량을 ​<color=#A27F0B><size=23>{player.recoverySheildPerSec*0.5f: 0}</size>만큼</color> 증가한다.";
        skillDesc["Concentrate"] = $"<color=#3B4F87>(Active)</color>30초 동안 초당 공격력을 ​<color=#D72323><size=23>{atk_damage * 0.5f: 0}</size>만큼</color> 증가한다.";
        skillDesc["Arcana"] = $"<color=#E83772>(Passive)</color>공격시 투사체 ​<color=#D72323><size=23>{3: 0}</size>개</color>가 추가로 날라간다.";
        skillDesc["Esbat"] = $"<color=#E83772>(Passive)</color>공격대기 시간이 1초에서 ​<color=#D72323><size=23>{0.55f}</size>초로</color> 감소한다.";
        skillDesc["Wing"] = $"<color=#E83772>(Passive)</color>이동속도가 ​<color=#FFFFFF><size=23>{50: 0}</size>%</color> 증가한다.";
        skillDesc["Ara"] = $"<color=#3B4F87>(Active)</color>30초 동안 점프속도를 ​<color=#D72323><size=23>{50: 0}</size>%</color> 강화한다.";
        skillDesc["Blink"] = $"<color=#3B4F87>(Active)</color>일정거리를 순간적으로 이동한다.";
        skillDesc["Wand"] = $"<color=#E83772>(Passive)</color>초당 마나회복속도를 ​<color=#003F92><size=23>{8: 0}</size>만큼</color> 증가한다.";
        skillDesc["Recycling"] = $"<color=#3B4F87>(Active)</color>30초 동안 초당 잃은 마나의 ​<color=#003F92><size=23>{1: 0}</size>%</color> 회복한다.";
        skillDesc["ManaDrain"] = $"<color=#E83772>(Passive)</color>가한 피해량의 ​<color=#003F92><size=23>{1: 0}</size>%를</color> 마나로 전환한다.";
        skillDesc["Cycle"] = $"<color=#E83772>(Passive)</color>마나소모량이 ​<color=#003F92><size=23>{25: 0}</size>%가</color> 감소한다.";
        skillDesc["MagicArrow"] = $"<color=#3B4F87>(Active)</color>적에게 ​<color=#D72323><size=23>{atk_damage * (3f + magic_atk) * concentration: 0}</size>의 데미지</color>를 입히는 마법화살 1개 생성한다.";
        skillDesc["Altin"] = $"<color=#3B4F87>(Active)</color>적에게 ​<color=#D72323><size=23>{atk_damage * (2.25f + magic_atk) * concentration: 0}</size>의 데미지</color>를 입히는 구체 5개 생성한다.";
        skillDesc["MagicCircle"] = $"<color=#3B4F87>(Active)</color>2초 동안 적에게 ​<color=#D72323><size=23>{atk_damage * (2f + magic_atk) * concentration: 0}</size>의 데미지</color>를 입히는 마법진을 소환한다.";
        skillDesc["Holy"] = $"<color=#3B4F87>(Active)</color>신성력을 방출하여 적에게 ​<color=#D72323><size=23>{atk_damage * (3.5f + magic_atk) * concentration : 0}</size>의 데미지</color>를 입힌다.";
        skillDesc["MagicEnforce"] = $"<color=#E83772>(Passive)</color>마법 공격력의 ​계수가<color=#003F92><size=23>{2: 0}</size>만큼</color> 증가한다.";

        skillDesc["LightningBall"] = $"<color=#3B4F87>(Active)</color>적에게 ​<color=#D72323><size=23>{atk_damage * 2.45f * elemental_atk * concentration: 0}</size>의 데미지</color>를 입히는 전기 구체 1개 생성한다.";
        skillDesc["LightningArrow"] = $"<color=#3B4F87>(Active)</color>적에게 ​<color=#D72323><size=23>{atk_damage * 5.0f * elemental_atk * concentration: 0}</size>의 데미지</color>를 입히는 전기 화살을 공중에 1개 생성한다.";
        skillDesc["ArtifficialSun"] = $"<color=#3B4F87>(Active)</color>적에게 ​<color=#D72323><size=23>{atk_damage * 8f * elemental_atk * concentration: 0}</size>의 데미지</color>를 입히는 태양을 공중에 1개 생성한다.";
        skillDesc["FireShield"] = $"<color=#3B4F87>(Active)</color>적에게 구체 하나당 ​<color=#D72323><size=23>{atk_damage * 0.6f * elemental_atk * concentration: 0}</size>의 데미지</color>를 입히는 화염구슬을 6개 생성한다.";
        skillDesc["Blizzard"] = $"<color=#3B4F87>(Active)</color>20초동안 적에게 초당 ​<color=#D72323><size=23>{atk_damage * 2f * elemental_atk * concentration: 0}</size>의 데미지</color>를 입히는 눈보라를 생성한다.";
        skillDesc["ElementalEnforce"] = $"<color=#E83772>(Passive)</color>원소 마법 피해량이<color=#003F92><size=23>{35: 0}</size>%만큼</color> 증가한다.";

    }
}
