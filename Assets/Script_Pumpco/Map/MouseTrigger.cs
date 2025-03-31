using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MouseTrigger : MonoBehaviour
{
    public static event Action<List<GameObject>, int> SavePart;
    public static event Action<bool> PartCompleted;

    public bool roadMode;
    public bool completed;//用于判断是否是合法路线
    public bool locked;//用于判断是否锁定

    public GameObject lastCube;
    public GameObject thisCube;
    public int cubeX;
    public int cubeY;
    public BaseCube cubeScript;

    public bool mouseOn;
    public Vector2 mousePosition;

    private bool fristPoint;//用于判断是否是刚开始画
    private float lasttime;//防抖动
    private bool beLoaded;//防抖动










    //临时保存某段线路的方块
    [System.Serializable]
    public class RoadPart//存储这段线路的方块与长度
    {
        public List<GameObject> baseCube;
        public int length;

        public void AddBaseCube(GameObject newBaseCube)//用于给这段线路添加新方块
        {
            baseCube.Add(newBaseCube);
            Debug.Log("表锅我加了个" + newBaseCube + newBaseCube.transform.position.x + "-" + newBaseCube.transform.position.y + "喔");
            RefreshCube();
            LengthCalculated();
        }

        public void LengthCalculated()//用于计算路线长度
        {
            length = 0;
            foreach(var i in baseCube)
            {
                BaseCube BCS = i.GetComponent<BaseCube>();
                length = length + BCS.length;
            }

        }

        public void showCube()//打印所有方块
        {
            foreach (var i in baseCube)
            {
                Debug.Log(i.transform.position.x + "-" + i.transform.position.y);
            }
            if (baseCube == null)
            {
                Debug.Log("表锅里面没东西喔？");
            }
            else
            {
                Debug.Log(length);
            }
        }

        public void RefreshCube()//用于改变方块颜色
        {
            foreach (var i in baseCube)
            {
                SpriteRenderer s = i.GetComponent<SpriteRenderer>();
                s.color = Color.cyan;
            }
        }

        public int SearchCube(GameObject newCube)
        {
            int n = baseCube.FindIndex(Cube => Cube == newCube);
            return n;
        }

        public void DeleteCube(GameObject newCube)//用于删除某个特定的方块
        {
            baseCube.Remove(newCube);
        }

        public void DeleteCubePlus(GameObject newCube)//用于删除某个方块及以后的方块
        {
            int n = SearchCube(newCube);
            if (n > 0)
            {
                foreach (var i in baseCube)//重置方块颜色
                {
                    SpriteRenderer s = i.GetComponent<SpriteRenderer>();
                    s.color = Color.white;
                }
                baseCube.RemoveRange(n, baseCube.Count - n);
            }
            else { Debug.Log("表锅我没找到要删的东西喔？"); }

        }

        
    }

    public RoadPart thisPart;










    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        cubeX = -1;
        cubeY = -1;

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (roadMode)
        {
            RoadSet();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            thisPart.showCube();

        }


    }





    private void OnEnable()
    {
        EventManeger.ToRoadMode += ChangeRoadMode;
        RoadListManeger.SendLastCube += GetLastCube;
    }

    private void OnDisable()
    {
        EventManeger.ToRoadMode -= ChangeRoadMode;
        RoadListManeger.SendLastCube -= GetLastCube;
    }

    public void ChangeRoadMode(bool x)
    {
        roadMode = x;
        fristPoint = true;
        Debug.Log("Trigger:表锅绘制路线模式是" + x + "了喔");
    }

    public void GetLastCube(GameObject last)
    {
        lastCube = last;
    }










    public void getCube()//获取指定的方块以及其basecube脚本
    {
        RaycastHit2D inHit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (inHit.collider != null)
        {
            //Debug.Log("表锅你打中了" + inHit.collider.gameObject.name+"喔");
            if (inHit.collider.CompareTag("Cube"))
            {
                thisCube = inHit.collider.gameObject;
                cubeScript = thisCube.GetComponent<BaseCube>();
                Debug.Log("表锅我选中了" + thisCube.transform.position.x + "-" + thisCube.transform.position.y + "喔");
            }
        }
        else
        {
            Debug.Log("表锅你没打准喔");
        }
    }















    public void RoadSet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("表锅你按下鼠标了喔？");
            getCube();
            
            if (fristPoint&&lastCube==null)//用于判断是否为开头 以及判断是否是新路线
            {
                if (cubeScript.isBuild)
                {
                    mouseOn = true;
                    fristPoint = false;
                    lastCube = null;
                    locked = false;
                }
                else { Debug.Log("表锅你没选中建筑喔？"); }

            }
            else
            {
                if (thisCube == lastCube)//用于判断是否继续往下画
                {
                    mouseOn = true;
                    lastCube = null;
                }
                else if (thisPart.SearchCube(thisCube) > 0)//用于判断是否为已记录方块
                {
                    thisPart.DeleteCubePlus(thisCube);
                    mouseOn = true;
                    lastCube = null;
                    locked = false;
                }
                
                else{ Debug.Log("表锅你没接着往下画喔？"); }

            }

        }

        if (mouseOn)
        {
            //Debug.Log("表锅你还在按喔？");
            getCube();
            if(lastCube != thisCube)//若进入方块，则下一步
            {
                if (cubeScript.canCross)//若可通行，则下一步
                {
                    if (thisPart.SearchCube(thisCube) > 0)//若为已记录方块，则删除后续内容
                    {
                        thisPart.DeleteCubePlus(thisCube);
                        locked = false;
                        lastCube = null;
                    }

                    if (!locked)//若被锁定，则跳过，输出广C老表
                    {
                        thisPart.AddBaseCube(thisCube);

                    }
                    else { Debug.Log("表锅你被锁死了喔？"); }
                }

                if ((!cubeScript.canCross || cubeScript.isBuild) && thisCube != thisPart.baseCube.First())//锁死逻辑（不可通行或是建筑，且不为起点）
                {
                    Debug.Log("表锅我要锁死你喔？");
                    locked = true;
                }

                lastCube = thisCube;//记录当前方块
            }

            
            
            //看情况加防误触
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("表锅你松开鼠标了喔？");
            mouseOn = false;
            BaseCube ends = thisPart.baseCube[^1].GetComponent<BaseCube>();
            completed = ends.isBuild;
            Debug.Log("表锅你的路线完成度是" + completed + "喔");
            //这里写判断代码
        }

    }

    public void SaveThisPart()
    {
        Debug.Log("表锅我保存了喔？");
        SavePart?.Invoke(thisPart.baseCube, thisPart.length);
    }

    public void ShowDetails()//用于鼠标边上显示信息UI
    {

    }

}
