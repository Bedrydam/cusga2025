using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : MonoBehaviour
{
    public RoadListManeger roadListManeger;

    [Header("��������")]
    public GameObject uiPrefab;  // RPUIԤ����
    public Transform parentObject;  // ���볡���е�����A��Ϊ������

    [Header("λ������")]
    public Vector2 size;
    public Vector2 firstUILocalPosition;  // ��һ��UI�ı������꣨����ڸ����壩
    public Vector2 secondUIOffset;  // �ڶ���UI����ڵ�һ����ƫ����
    public int totalUICount = 4;  // ����������



    [System.Serializable]
    public class RoadUI
    {
        public GameObject RoadUIPrefab;
        public Transform parentObject;
    }
    public RoadUI roadUISet;

    public void RoadUISpawn()
    {
        for (int i = roadUISet.parentObject.childCount - 1; i >= 0; i--)//��ջ����б�
        {
            Destroy(roadUISet.parentObject.GetChild(i).gameObject);
        }

        List<RoadListManeger.Road_Road> roadList = roadListManeger.Road_List;
        int n = 0;
        foreach(var i in roadList)//�����б�
        {
            GameObject RoadListUI = Instantiate(roadUISet.RoadUIPrefab, roadUISet.parentObject);
            RoadListUI.name = $"RoadList{n}";
            //����·�������޸�����
            n++;
        }

    }





    void Start()
    {
        roadListManeger = GetComponentInChildren<RoadListManeger>();

        if (parentObject == null)
        {
            Debug.LogError("δָ�������壡");
            return;
        }

        //EZSpawn();
    }

    void SpawnChildUIs()
    {
        // ���ɵ�һ��UI
        GameObject firstUI = Instantiate(uiPrefab, parentObject);
        firstUI.name = "RPUI_0";
        SetUIPosition(firstUI, firstUILocalPosition);

        // ���ɵڶ���UI
        if (totalUICount >= 2)
        {
            GameObject secondUI = Instantiate(uiPrefab, parentObject);
            secondUI.name = "RPUI_1";
            SetUIPosition(secondUI, firstUILocalPosition + secondUIOffset);

            // ����ʣ��UI������ͬƫ����������
            for (int i = 2; i < totalUICount; i++)
            {
                GameObject newUI = Instantiate(uiPrefab, parentObject);
                newUI.name = $"RPUI_{i}";
                SetUIPosition(newUI, firstUILocalPosition + (secondUIOffset * i));
            }
        }
    }

    void SetUIPosition(GameObject ui, Vector2 localPosition)
    {
        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.localPosition = localPosition;  // ʹ��localPosition������Ը�����
        rt.sizeDelta = size;

        // �������ź���ת������̳и�����ı任
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
    }

    public void EZSpawn()
    {
        int i = 0;
        while (i < 4)
        {
            i++;
            GameObject firstUI = Instantiate(uiPrefab, parentObject);
            firstUI.name = "RPUI_0";
        }
        

    }


}
