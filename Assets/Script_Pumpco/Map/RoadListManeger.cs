using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadListManeger : MonoBehaviour
{
    public static event Action<GameObject> SendLastCube;
    public int thisRoad;//���ڵڼ�����·

    private GoodsListUI goodsListUI;

    [System.Serializable]
    public class Road_Part//���ڴ洢ĳ����·���е�ÿһ�����飩
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
    public class Road_Road//���ڴ洢һ������·
    {
        public List<Road_Part> parts;

        public bool Completed;//�����ж�·���Ƿ���β����

        public void AddPart(List<GameObject> gameObjects,int goLength)//���ڸ���һ������·���һ��С·��
        {
            Debug.Log("����Ҹ����ݿ���˸�����ร�");
            Road_Part newPart = new Road_Part();
            newPart.baseCubes = gameObjects;
            newPart.length=goLength;
            parts.Add(newPart);
        }

        public void CheckComplete()//���ڼ��·���Ƿ����
        {
            Completed = (parts[0].baseCubes[0] == parts[^1].baseCubes[^1]);
        }

        public void ChangeGoods()//���ڸ�����Ʒ
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

    public void SavePart(List<GameObject> gameObjects,int length)//������Ӷ���
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

    public void DeletePart()//����ɾ��ĳ������
    {

    }


    public void DeleteRoad()//ɾ����Ŵ�0��ʼ,����ɾ��ĳ�������·��
    {
        if (thisRoad >= 0 && thisRoad < Road_List.Count)
        {
            Road_List.RemoveAt(thisRoad);
            thisRoad = thisRoad - 1;
            //������ҳ
        }
        else
        {
            Debug.Log("�����ɾ��ͷ���");
        }
    }


    public void refreshCubeColor()//����ˢ�·�����ɫ����ѡ��ؿ��δѡ��ؿ飩
    {
        
    }

    public void CreatNewPart()//�����½��������
    {

    }

    public void SaveGoods(int road,int part,GoodsListUI.Goods goods)
    {
        Road_List[road].parts[part].goods.wood = goods.wood;
        Road_List[road].parts[part].goods.stone = goods.stone;
    }





}
