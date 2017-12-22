using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineNum : MonoBehaviour {

    public Text ShowMineNum;
    public int Num;


    public void PrintNum()
    {
        ShowMineNum.text = Num.ToString();
    }
}

