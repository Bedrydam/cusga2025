using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadListManeger : MonoBehaviour
{
    public int thisPage;//�����ж�ҳ���Լ��ڼ�����·

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

        public void AddPart(List<GameObject> gameObjects,int goLength)//���ڸ���һ������·���һ��С·��
        {
            Debug.Log("����Ҹ����ݿ���˸�����ร�");
            Road_Part newPart = new Road_Part();
            newPart.baseCubes = gameObjects;
            newPart.length=goLength;
            parts.Add(newPart);
        }

        public void ChangeGoods()//���ڸ�����Ʒ
        {

        }

    }

    public List<Road_Road> Road_List;

    private void OnEnable()
    {
        MouseTrigger.SavePart += SavePart;
    }

    private void OnDisable()
    {
        MouseTrigger.SavePart -= SavePart;
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
        if (Road_List.Count <= thisPage)
        {
            Road_List.Add(new Road_Road());
        }
        if (Road_List[thisPage].parts == null)
        {
            Road_List[thisPage].parts = new List<Road_Part>();
        }
        Road_List[thisPage].AddPart(gameObjects,length);
    }

    public void DeletePart()//����ɾ��ĳ������
    {

    }


    public void DeleteRoad()//ɾ����Ŵ�0��ʼ,����ɾ��ĳ�������·��
    {
        if (thisPage >= 0 && thisPage < Road_List.Count)
        {
            Road_List.RemoveAt(thisPage);
            thisPage = thisPage - 1;
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

}
