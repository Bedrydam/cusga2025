using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GoodsListUI : MonoBehaviour
{
    public static event Action<int,int,Goods> SaveGoodsChange;

    private UISpawner spawner;
    private bool canSave;

    private int road;
    private int part;

    [System.Serializable]
    public class Goods
    {
        public int wood;
        public int stone;

        public bool CheckWeight(int weight)
        {
            bool overWeight;
            overWeight = (weight > wood * 1 + stone * 1);
            return overWeight;
        }


    }

    public Goods thisGoods;

    [System.Serializable]
    public class Cars
    {
        public int speed;
        public int weight;

        public bool beUsed;
    }

    public List<Cars> carList;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<UISpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWeight()
    {
        int weight=new int();
        //这里调用选中车辆的承重
        canSave=!thisGoods.CheckWeight(weight);
    }

    public void WoodUp(){thisGoods.wood++; CheckWeight(); }

    public void WoodDown(){ thisGoods.wood--; CheckWeight(); }

    public void WoodSet(int num){ thisGoods.wood = num; CheckWeight(); }


    public void SaveGoodss()
    {
        if (canSave)
        {
            SaveGoodsChange?.Invoke(road,part,thisGoods);
        }
        else
        {
            Debug.Log("表锅你这车不够大喔");
            //可以加入弹窗提示
        }
    }

}
