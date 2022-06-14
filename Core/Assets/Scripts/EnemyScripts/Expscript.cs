using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expscript : MonoBehaviour
{
    GameObject ExpGauge;
    GameObject level;
    int LV;
    int x;
    float total;

    void Start()
    {
        total = 0;
        LV = 1;
        this.ExpGauge = GameObject.Find("ex_bar");
        this.level = GameObject.Find("LVpoint");
        this.ExpGauge.GetComponent<Image>().fillAmount = total;
    }

    void Update()
    {
        this.level.GetComponent<Text>().text = "Lv : " + LV;
    }

    public void AddExp(float exp)
    {
        total += exp/10/LV;
        if(total >= 1)
        {
            LV++;
            total = total - 1;
            this.ExpGauge.GetComponent<Image>().fillAmount = total;
        }
        else
        {
            this.ExpGauge.GetComponent<Image>().fillAmount += total;
        }
        
    }
}
