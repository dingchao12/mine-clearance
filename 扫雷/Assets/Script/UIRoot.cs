using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建立一个值类型的类，用来记录游戏中的平面点=>（X,Y）
/// </summary>
struct Position
{
    public int PointX;
    public int PointY;

    public Position(int _pointx, int _pointy)
    {
        PointX = _pointx;
        PointY = _pointy;
    }
}

public class UIRoot : MonoBehaviour
{
    //需要挂载的组件
    public GameObject Item;
    public GameObject Plane;
    public Button ReLive;
    public GameObject Die;
    public GameObject Win;
    public MineNum MineNumber;
    public TimeLine TimeLine;

    //创建一个和雷区一样大小的二维数组
    public int[,] panl = new int[9, 9];

    public Dictionary<int, Item> ItemDictionary = new Dictionary<int, Item>();
    public int WinNum;

   //建立列表储存游戏中所有平面点；
    private List<Position> point = new List<Position>();
    private void Awake()
    {
        Init();
        CreatMine();
        ReLive.onClick.AddListener(OnClickRelve);
    }

    void Init()
    {
        //清空列表；若不清空，重来的时候会出现相同点加载两次的情况
        point.Clear();
        //实例化9X9的Item组件
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                var mask = Instantiate(Item, Plane.transform);
                //将J的赋值给Item的Sizex；
                mask.GetComponent<Item>().Sizex = j;
                mask.GetComponent<Item>().Sizey = i;

                //将该Item写入字典，j*10+i定义为Key值来保证key不重复而且与坐标位子相关
                ItemDictionary.Add(j * 10 + i, mask.GetComponent<Item>());
                //标记该位置的Item在二维数组中标记数为2；
                panl[j, i] = 2;
                //new 一个x=j,y=i的position
                var position = new Position(j, i);
                //添加到点列表里
                point.Add(position);
            }
        }
    }

    /// <summary>
    /// 设置雷点位置
    /// </summary>
    void CreatMine()
    {
        for (int j = 0; j < 10; j++)
        {
            //雷个数每次加1；
            MineNumber.Num++;
            //随机从0到Point列表长度的数字间任意选中一个数；
            int a = Random.Range(0, point.Count);
            //让panl数组中与该点相同位置的点重新标记为1；
            panl[point[a].PointX, point[a].PointY] = 1;
            //从列表中删除该店，保证不会随机到相同点；
            point.RemoveAt(a);
        }
        //调用MineNumber脚本中的PrintNum函数显示雷的个数；
        MineNumber.PrintNum();
    }
    /// <summary>
    /// 重来
    /// </summary>
    void OnClickRelve()
    {
        ItemDictionary.Clear();
        //地雷数归零记时归零
        MineNumber.Num = 0;
        TimeLine.time = 0;
        //摧毁Plane下的所有子节点Itme
        for (int i = 0; i < Plane.transform.childCount; i++)
        {
            Destroy(Plane.transform.GetChild(i).gameObject);
        }
        //将die 和win界面隐藏
        Die.SetActive(false);
        Win.SetActive(false);
        //winNum归零
        WinNum = 0;
        Init();
        CreatMine();
    }

    public void GameWin()
    {
        TimeLine.IsGameOver = true;
        Win.SetActive(true);
    }
    public void GameOver()
    {
        TimeLine.IsGameOver = true;
        Die.SetActive(true);
    }
    /// <summary>
    /// 播放Plane下面所有子节点的动画
    /// </summary>
    public void PlayAnimator()
    {
        for (int i = 0; i < Plane.transform.childCount; i++)
        {
            Plane.transform.GetChild(i).GetComponent<Item>().ShowImage.GetComponent<GameAnimator>().PlayAnimator();
        }
    }
}
