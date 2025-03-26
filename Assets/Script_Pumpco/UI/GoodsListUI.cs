using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsListUI : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ValueUp(string name)
    {

    }

    public void ValueDown(string name)
    {

    }

    public void ValueSet(string name, int num)
    {

    }

    public void SaveGoodss()
    {

    }

}
