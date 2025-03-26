using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoadUI : MonoBehaviour
{
    public RoadListManeger roadListManeger;
    public GoodsListUI goodsListUI;
    public UISpawner uISpawner;

    // Start is called before the first frame update
    void Start()
    {
        roadListManeger = GetComponent<RoadListManeger>();
        goodsListUI= GetComponent<GoodsListUI>();
        uISpawner = GetComponentInParent<UISpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRoadUI()
    {
        //UI切到路线页
        LoadRoadData();
    }


    public List<RoadListManeger.Road_Road> roadList;
    public int roadPage;
    public List<RoadListManeger.Road_Part> partList;
    public int partPage;
    public void LoadRoadData()//用于载入路线信息以及显示按键UI
    {
        roadList = roadListManeger.Road_List;
        foreach(var i in roadList)
        {
            //载入路线信息，生成UI按键
        }

    }

    public void OpenPartUI(int num)
    {
        
        partList = roadListManeger.Road_List[num].parts;
        foreach(var i in partList)
        {
            //载入路段信息，生成UI页面
        }
        //生成UI按键

    }




}
