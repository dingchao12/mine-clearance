using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLine : MonoBehaviour
{
    public Text TimeNum;
    public float time;
    public bool IsGameOver=false;
    private void Update()
    {
        if (!IsGameOver)
        {
            time += Time.deltaTime;
            ShowTime();
        }
    }
    void ShowTime()
    {
        TimeNum.text = "Time:"+ time.ToString();
    }
}
