using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerDownHandler //Itme天骄点击按下的事件接口
{
    //自己的x.y值；
    public int Sizex;
    public int Sizey;
    //挂载的组件
    public GameObject Sign;
    public Text NumText;
    public Sprite mineSprite;
    public Image Frame;
    public Transform ShowImage;
    public Image Mask;

    private GameObject bgAnimator;
    private MineNum mineNum;
    private UIRoot uiRoot;
    bool IsSign = false;
    int[,] temp = new int[9, 9];

    private void Start()
    {
        bgAnimator = GameObject.Find("BG/Bg");
        mineNum = GameObject.Find("MineNum").GetComponent<MineNum>();
        uiRoot = GetComponentInParent<UIRoot>();
        InIt();
        CreatNum();
    }
    /// <summary>
    /// 初始化，显示图片的位置
    /// </summary>
    void InIt()
    {
        //相对于初始位置每次向左移动100个像素
        for (int i = 0; i < Sizex; i++)
        {
            ShowImage.position += new Vector3(-100, 0, 0);
        }
        //相对于初始位置每次向上移动100个像素
        for (int i = 0; i < Sizey; i++)
        {
            ShowImage.position += new Vector3(0, 100, 0);
        }
    }
    /// <summary>
    /// 获取当前点不是雷点的点的周围雷的个数；
    /// </summary>
    public void CreatNum()
    {
        if (uiRoot.panl[Sizex, Sizey] != 1)
        {
            int i = 0;

            if (Sizex > 0 && Sizex < 8)
            {
                if (Sizey > 0)
                {
                    if (uiRoot.panl[Sizex - 1, Sizey - 1] == 1)
                    {
                        //是雷+1
                        i++;
                    }
                    if (uiRoot.panl[Sizex, Sizey - 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex + 1, Sizey - 1] == 1)
                    {
                        i++;
                    }
                }
                if (Sizey < 8)
                {
                    if (uiRoot.panl[Sizex - 1, Sizey + 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex, Sizey + 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex + 1, Sizey + 1] == 1)
                    {
                        i++;
                    }
                }
                if (uiRoot.panl[Sizex + 1, Sizey] == 1)
                {
                    i++;
                }
                if (uiRoot.panl[Sizex - 1, Sizey] == 1)
                {
                    i++;
                }
            }
            else if (Sizex == 0)
            {
                if (Sizey > 0)
                {
                    if (uiRoot.panl[Sizex, Sizey - 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex + 1, Sizey - 1] == 1)
                    {
                        i++;
                    }
                }
                if (Sizey < 8)
                {
                    if (uiRoot.panl[Sizex, Sizey + 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex + 1, Sizey + 1] == 1)
                    {
                        i++;
                    }
                }
                if (uiRoot.panl[Sizex + 1, Sizey] == 1)
                {
                    i++;
                }
            }
            else
            {
                if (Sizey > 0)
                {
                    if (uiRoot.panl[Sizex, Sizey - 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex - 1, Sizey - 1] == 1)
                    {
                        i++;
                    }
                }
                if (Sizey < 8)
                {
                    if (uiRoot.panl[Sizex, Sizey + 1] == 1)
                    {
                        i++;
                    }
                    if (uiRoot.panl[Sizex - 1, Sizey + 1] == 1)
                    {
                        i++;
                    }
                }
                if (uiRoot.panl[Sizex - 1, Sizey] == 1)
                {
                    i++;
                }
            }
            if (i == 0)
            {
                //周围没有雷的点显示值为空，并且在二维数组中将这些点标记为3
                NumText.text = string.Empty;
                uiRoot.panl[Sizex, Sizey] = 3;
            }
            else
            {
                NumText.text = i.ToString();
            }
        }
    }
    /// <summary>
    /// 当点击按下时间触发
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //如果按下的是鼠标左键
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //该点被玩家标记为雷或者已经排除，那么retrun
            if (IsSign == true || uiRoot.panl[Sizex, Sizey] == 4)
            {
                return;
            }
            //若这点在二维数值中标记是雷
            if (uiRoot.panl[Sizex, Sizey] == 1)
            {
                //替换Mask组件的图片精灵
                Mask.sprite = mineSprite;
                //将显示图片隐藏，将边框隐藏；
                ShowImage.gameObject.SetActive(false);
                Frame.gameObject.SetActive(false);
                //调用游戏结束；
                uiRoot.GameOver();
            }
            //如果该点是周围一个雷也没有的空白点
            else if (uiRoot.panl[Sizex, Sizey] == 3)
            {
                //执行清除空白点函数；
                ClearNnmTextZero();
                //播放背景动画，和所有Item动画
                bgAnimator.GetComponent<GameAnimator>().PlayBgAnimator();
                uiRoot.PlayAnimator();
            }
            //如果该点是周围有雷的
            else if (uiRoot.panl[Sizex, Sizey] == 2)
            {
                //胜利数组加1
                uiRoot.WinNum++;
                //将Mask的图片显示透明度设置为0，完全透明
                Mask.color = new Color(255, 255, 255, 0);
                //将该点在维尔数组中标记为4.表明它是已经排除过的雷区
                uiRoot.panl[Sizex, Sizey] = 4;

                bgAnimator.GetComponent<GameAnimator>().PlayBgAnimator();
                uiRoot.PlayAnimator();
            }
            //如果胜利数字为71就胜利（将所有的没有雷的地方）
            if (uiRoot.WinNum == 71)
            {
                uiRoot.GameWin();
            }
        }
        //如果按下都是右键
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (uiRoot.panl[Sizex, Sizey] == 4)
            {
                return;
            }
            //没有被玩家标记为有雷
            if (IsSign == false && mineNum.Num > 0)
            {
                //将该点panl数组的标记数赋值给temp数组的该点；
                temp[Sizex, Sizey] = uiRoot.panl[Sizex, Sizey];
                //显示标记图片
                Sign.SetActive(true);
                IsSign = true;
                //将该点排除
                uiRoot.panl[Sizex, Sizey] = 5;
                //雷数减一
                mineNum.Num--;
                //调用显示雷数的函数
                mineNum.PrintNum();
            }
            else if (IsSign == true && uiRoot.panl[Sizex, Sizey] == 5)
            {
                Sign.SetActive(false);
                IsSign = false;
                //将该点temp数组的标记数赋值给数组panl的该点；
                uiRoot.panl[Sizex, Sizey] = temp[Sizex, Sizey];
                mineNum.Num++;
                mineNum.PrintNum();
            }
        }
    }
    /// <summary>
    /// 排除周围没有没有雷的相邻点是有雷的点
    /// </summary>
    public void ClearNumText()
    {
        uiRoot.WinNum++;
        Mask.color = new Color(255, 255, 255, 0);
        uiRoot.panl[Sizex, Sizey] = 4;
    }
    /// <summary>
    /// 排除周围没有雷的点
    /// </summary>
    public void ClearNnmTextZero()
    {
        uiRoot.WinNum++;
        Mask.color = new Color(255, 255, 255, 0);
        uiRoot.panl[Sizex, Sizey] = 4;
        if (Sizex > 0 && Sizex < 8)
        {
            if (Sizey > 0)
            {
                if (uiRoot.panl[Sizex - 1, Sizey - 1] == 3)
                {
                    //深度排除为周围雷数为0的点的左下方周围雷的个数也为0的点
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex - 1, Sizey - 1] == 2)
                {
                    //排除为周围雷数为0的点的左下方周围雷的个数不为0的点
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey - 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex + 1, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex + 1, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey - 1].ClearNumText();
                }
            }
            if (Sizey < 8)
            {
                if (uiRoot.panl[Sizex - 1, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex - 1, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey + 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex + 1, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex + 1, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey + 1].ClearNumText();
                }
            }
            if (uiRoot.panl[Sizex + 1, Sizey] == 3)
            {
                uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey].ClearNnmTextZero();
            }
            else if (uiRoot.panl[Sizex + 1, Sizey] == 2)
            {
                uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey].ClearNumText();
            }
            if (uiRoot.panl[Sizex - 1, Sizey] == 3)
            {
                uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey].ClearNnmTextZero();
            }
            else if (uiRoot.panl[Sizex - 1, Sizey] == 2)
            {
                uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey].ClearNumText();
            }

        }

        else if (Sizex == 0)
        {
            if (Sizey > 0)
            {
                if (uiRoot.panl[Sizex, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex + 1, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex + 1, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey - 1].ClearNumText();
                }
            }
            if (Sizey < 8)
            {
                if (uiRoot.panl[Sizex, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex + 1, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex + 1, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey + 1].ClearNumText();
                }
            }
            if (uiRoot.panl[Sizex + 1, Sizey] == 3)
            {
                uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey].ClearNnmTextZero();
            }
            else if (uiRoot.panl[Sizex + 1, Sizey] == 2)
            {
                uiRoot.ItemDictionary[(Sizex + 1) * 10 + Sizey].ClearNumText();
            }
        }

        else
        {
            if (Sizey > 0)
            {
                if (uiRoot.panl[Sizex, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey - 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex - 1, Sizey - 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey - 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex - 1, Sizey - 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey - 1].ClearNumText();
                }
            }
            if (Sizey < 8)
            {
                if (uiRoot.panl[Sizex, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex) * 10 + Sizey + 1].ClearNumText();
                }
                if (uiRoot.panl[Sizex - 1, Sizey + 1] == 3)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey + 1].ClearNnmTextZero();
                }
                else if (uiRoot.panl[Sizex - 1, Sizey + 1] == 2)
                {
                    uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey + 1].ClearNumText();
                }
            }
            if (uiRoot.panl[Sizex - 1, Sizey] == 3)
            {
                uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey].ClearNnmTextZero();
            }
            else if (uiRoot.panl[Sizex - 1, Sizey] == 2)
            {
                uiRoot.ItemDictionary[(Sizex - 1) * 10 + Sizey].ClearNumText();
            }
        }
    }
}
