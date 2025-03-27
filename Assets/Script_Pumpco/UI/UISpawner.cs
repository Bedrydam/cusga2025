using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public GameObject UIPrefab;//Ԥ����
        public Transform parentObject;//�����壨����content�ϣ�
    }
    public RoadUI roadUISet;

    public void RoadUISpawn()
    {
        UISpawner spawner = this;
        for (int i = roadUISet.parentObject.childCount - 1; i >= 0; i--)//��ջ����б�
        {
            Destroy(roadUISet.parentObject.GetChild(i).gameObject);
        }

        List<RoadListManeger.Road_Road> roadList = roadListManeger.Road_List;
        int n = 0;
        foreach(var i in roadList)//�����б�
        {
            //������һ�°�
            GameObject RoadListUI = Instantiate(roadUISet.UIPrefab, roadUISet.parentObject);
            RoadListUI.name = $"RoadList{n}";

            //�����޸�Ԥ������Button�¼�
            Button targetButton=RoadListUI.GetComponent<Button>();
            if (targetButton != null)
            {
                Debug.Log(targetButton + " " + spawner);
                targetButton.onClick.RemoveAllListeners();//ˢ�¼���
                targetButton.onClick.AddListener(() =>
                {
                    spawner.PartUISpawn(n); // ����C�������������
                });
            }


            //����·�������޸�����
            n++;
        }

    }





    [System.Serializable]
    public class PartUI
    {
        public GameObject UIPrefab;
        public Transform parentObject;//����menu��
        public Vector2 UIsize;//UI��С
        public Vector2 firstUIPosition;//��ʼUIλ��
        public Vector2 UIOffset;  // ���ƫ����
    }
    public PartUI partUISet;



    public void PartUISpawn(int num)
    {
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        if(partPanel != null)
        {
            Destroy(partPanel.gameObject);
        }
        GameObject partUIPanel = Instantiate(partUISet.UIPrefab, partUISet.parentObject);
        partUIPanel.name = "PartUIPanel";

        List<RoadListManeger.Road_Part> partList = roadListManeger.Road_List[num].parts;
        int n = 0;

        foreach (var i in partList)//�����б�
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //����·�������޸�����
            n++;
        }
    }

    public void EZPartUISpawn()
    {
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        if (partPanel != null)
        {
            Destroy(partPanel.gameObject);
        }
        GameObject partUIPanel = Instantiate(partUISet.UIPrefab, partUISet.parentObject);
        partUIPanel.name = "PartUIPanel";

        int n = 0;
        while(n<4)//�����б�
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
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
        //rt.sizeDelta = size;

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
