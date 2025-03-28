using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class UISpawner : MonoBehaviour
{
    private RoadListManeger roadListManeger;
    private GoodsListUI goodsListUI;

    [Header("ƽ���ƶ��ٶ�")]
    public float moveDuration = 0.5f;

    private int pageCount;
    private int thisPage;




    [System.Serializable]
    public class RoadUI
    {
        public GameObject UIPrefab;//Ԥ����
        public GameObject AddRoadUIPrefab;//����·�ߵ�Ԥ����
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

            //�����Ǹ���·��������ʾ����
            n++;
        }

        GameObject addRoadUI = Instantiate(roadUISet.AddRoadUIPrefab, roadUISet.parentObject);
        addRoadUI.name = "AddRoadUI";
        //����������ť���ϴ�����·�ߵķ���

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

    [System.Serializable]
    public class GoodListUI
    {
        public GameObject GoodsUI;//���ڸı���Ʒ������UI���Ѿ�����UI�У�
        public GameObject PageUILeft;//��ҳUI
        public GameObject PageUIRight;
    }
    public GoodListUI goodUISet;


    public void PartUISpawn(int num)
    {
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        if(partPanel != null)
        {
            Destroy(partPanel.gameObject);
        }
        GameObject partUIPanel = Instantiate(partUISet.UIPrefab, partUISet.parentObject);
        partUIPanel.name = "PartUIPanel";
        partUIPanel.GetComponent<Image>().enabled = false;

        List<RoadListManeger.Road_Part> partList = roadListManeger.Road_List[num].parts;
        int n = 0;
        pageCount = -1;
        thisPage = 0;
        foreach (var i in partList)//�����б�
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //�����Ǹ���·��������ʾ����
            n++;
            pageCount++;
        }

        goodUISet.GoodsUI.SetActive(true);
        LordGoodList();
        CheckGoodsPage();

    }

    public void EZPartUISpawn(int i)//���ڶ������ԣ�δ���ã�
    {
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        if (partPanel != null)
        {
            Destroy(partPanel.gameObject);
        }
        GameObject partUIPanel = Instantiate(partUISet.UIPrefab, partUISet.parentObject);
        partUIPanel.name = "PartUIPanel";
        partUIPanel.GetComponent<Image>().enabled = false;

        pageCount = -1;
        thisPage = 0;
        int n = 0;
        while(n<i)//�����б�
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //�����Ǹ���·��������ʾ����
            n++;
            pageCount++;
        }

        goodUISet.GoodsUI.SetActive(true);
        LordGoodList();
        CheckGoodsPage();
    }



    public void NextPartPage()
    {
        thisPage++;
        LordGoodList();
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        StartCoroutine(HideGoodsUI());
        for (int i = 0; i < partPanel.childCount; i++)
        {
            RectTransform thisUI=partPanel.GetChild(i).GetComponent<RectTransform>();
            StartCoroutine(SmoothMove(thisUI, thisUI.anchoredPosition, thisUI.anchoredPosition-partUISet.UIOffset));
            Debug.Log(partPanel.GetChild(i).gameObject.name);
        }
        
    }

    public void LatePartPage()
    {
        thisPage--;
        LordGoodList();
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        StartCoroutine(HideGoodsUI());
        for (int i = 0; i < partPanel.childCount; i++)
        {
            RectTransform thisUI = partPanel.GetChild(i).GetComponent<RectTransform>();
            StartCoroutine(SmoothMove(thisUI, thisUI.anchoredPosition, thisUI.anchoredPosition + partUISet.UIOffset));
            Debug.Log(partPanel.GetChild(i).gameObject.name);
        }
        
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 startPos, Vector2 endPos, System.Action onComplete = null)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = endPos;
        onComplete?.Invoke();
    }

    private IEnumerator HideGoodsUI()
    {
        goodUISet.GoodsUI.SetActive(false);
        yield return new WaitForSeconds(moveDuration);
        goodUISet.GoodsUI.SetActive(true);
        CheckGoodsPage();
    }

    public void CheckGoodsPage()
    {
        goodUISet.PageUILeft.SetActive(true);
        goodUISet.PageUIRight.SetActive(true);
        if (thisPage == 0)
        {
            goodUISet.PageUILeft.SetActive(false);
        }
        if (thisPage == pageCount)
        {
            goodUISet.PageUIRight.SetActive(false);
        }
        
    }

    public void LordGoodList()
    {
        //����д���뵱ǰ������·��

        //����д���뵱ǰ·�ε���Ʒ��Ϣ
        
    }

    void Start()
    {
        roadListManeger = GetComponentInChildren<RoadListManeger>();
        goodsListUI = GetComponent<GoodsListUI>();
        gameObject.SetActive(false);
        //EZSpawn();
    }

    private void Update()
    {
        
    }


    void SetUIPosition(GameObject ui, Vector2 localPosition)
    {
        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.localPosition = localPosition;  //������Ը�����
        // �������ź���ת������̳и�����ı任
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
    }

    public void OpenRoadUI()
    {
        OpenUI();
        RoadUISpawn();
    }

    public void OpenUI()//�����ڴ򿪹��ܲ˵��������¹��ܲ˵����ݣ��������򿪹���ʹ�ã�
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()//�����ڹرչ��ܲ˵�
    {
        gameObject.SetActive(false);
    }

    public void ClosePartUI()
    {
        goodUISet.GoodsUI.SetActive(false);
        Transform partPanel = partUISet.parentObject.transform.Find("PartUIPanel");
        if (partPanel != null)
        {
            Destroy(partPanel.gameObject);
        }
    }

    public void CloseRoadUI()//��������ȥ·����
    {
        for (int i = roadUISet.parentObject.childCount - 1; i >= 0; i--)//��ջ����б�
        {
            Destroy(roadUISet.parentObject.GetChild(i).gameObject);
        }
    }


}
