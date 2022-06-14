using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP : MonoBehaviour
{
    Text text;
    public static int sp;
    public static int hp;
    public static int mp;
    public static int sd;
    public static int att;
    public static int def;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        sp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "SP : " + sp;
    }
}
