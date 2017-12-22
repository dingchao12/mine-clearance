using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    public GameObject UIRoot;

    private void Awake()
    {
        LoadGame();
    }
    void LoadGame()
    {
        var temp = Instantiate(UIRoot, transform);
    }
}
