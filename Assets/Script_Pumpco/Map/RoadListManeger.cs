using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadListManeger : MonoBehaviour
{
    public static event Action<GameObject> SendLastCube;
    public int thisRoad;//用于第几条线路

    private GoodsListUI goodsListUI;

    [System.Serializable]
    public class Road_Part//用于存储某段线路（中的每一个方块）
    {
        public List<GameObject> baseCubes;
        public int length;
        public Road_Good goods;
    }

    [System.Serializable]
    public class Road_Good
    {
        public int wood;
        public int stone;
    }

    [System.Serializable]
    public class Road_Road//用于存储一整条线路
    {
        public List<Road_Part> parts;

        public bool Completed;//用于判断路线是否首尾相连

        public void AddPart(List<GameObject> gameObjects,int goLength)//用于给这一整条线路添加一段小路段
        {
            Debug.Log("表锅我给数据库加了个东西喔？");
            Road_Part newPart = new Road_Part();
            newPart.baseCubes = gameObjects;
            newPart.length=goLength;
            parts.Add(newPart);
        }

        public void CheckComplete()//用于检测路线是否完成
        {
            Completed = (parts[0].baseCubes[0] == parts[^1].baseCubes[^1]);
        }

        public void ChangeGoods()//用于更改商品
        {

        }

    }

    public List<Road_Road> Road_List;

    private void OnEnable()
    {
        EventManeger.ToRoadMode += LoadLastCube;
        MouseTrigger.SavePart += SavePart;
        GoodsListUI.SaveGoodsChange += SaveGoods;
    }

    private void OnDisable()
    {

        EventManeger.ToRoadMode -= LoadLastCube;
        MouseTrigger.SavePart -= SavePart;
        GoodsListUI.SaveGoodsChange -= SaveGoods;
    }

    public void LoadLastCube(bool b)
    {
        if (b)
        {
            if (Road_List[thisRoad] == null)
            {
                SendLastCube?.Invoke(null);
            }
            else
            {
                SendLastCube?.Invoke(Road_List[thisRoad].parts[^1].baseCubes[^1]);
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SavePart(List<GameObject> gameObjects,int length)//用于添加段落
    {
        if (Road_List.Count <= thisRoad)
        {
            Road_List.Add(new Road_Road());
        }
        if (Road_List[thisRoad].parts == null)
        {
            Road_List[thisRoad].parts = new List<Road_Part>();
        }
        Road_List[thisRoad].AddPart(gameObjects,length);
        Road_List[thisRoad].CheckComplete();
    }

    public void DeletePart()//用于删除某个段落
    {

    }


    public void DeleteRoad()//删除序号从0开始,用于删除某条已完成路线
    {
        if (thisRoad >= 0 && thisRoad < Road_List.Count)
        {
            Road_List.RemoveAt(thisRoad);
            thisRoad = thisRoad - 1;
            //触发翻页
        }
        else
        {
            Debug.Log("表锅你删过头了喔");
        }
    }


    public void refreshCubeColor()//用于刷新方块颜色（已选择地块和未选择地块）
    {
        
    }

    public void CreatNewPart()//用于新建缓存段落
    {

    }

    public void SaveGoods(int road,int part,GoodsListUI.Goods goods)
    {
        Road_List[road].parts[part].goods.wood = goods.wood;
        Road_List[road].parts[part].goods.stone = goods.stone;
    }





}
