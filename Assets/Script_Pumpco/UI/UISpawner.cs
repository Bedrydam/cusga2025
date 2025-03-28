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

    [Header("平滑移动速度")]
    public float moveDuration = 0.5f;

    private int pageCount;
    private int thisPage;




    [System.Serializable]
    public class RoadUI
    {
        public GameObject UIPrefab;//预制体
        public GameObject AddRoadUIPrefab;//增加路线的预制体
        public Transform parentObject;//父物体（挂载content上）
    }
    public RoadUI roadUISet;

    public void RoadUISpawn()
    {
        UISpawner spawner = this;
        for (int i = roadUISet.parentObject.childCount - 1; i >= 0; i--)//清空缓存列表
        {
            Destroy(roadUISet.parentObject.GetChild(i).gameObject);
        }

        List<RoadListManeger.Road_Road> roadList = roadListManeger.Road_List;
        int n = 0;
        foreach(var i in roadList)//生成列表
        {
            //简单生成一下叭
            GameObject RoadListUI = Instantiate(roadUISet.UIPrefab, roadUISet.parentObject);
            RoadListUI.name = $"RoadList{n}";

            //用于修改预制体内Button事件
            Button targetButton=RoadListUI.GetComponent<Button>();
            if (targetButton != null)
            {
                Debug.Log(targetButton + " " + spawner);
                targetButton.onClick.RemoveAllListeners();//刷新监听
                targetButton.onClick.AddListener(() =>
                {
                    spawner.PartUISpawn(n); // 调用C方法并传入参数
                });
            }

            //这里是根据路线数据显示内容
            n++;
        }

        GameObject addRoadUI = Instantiate(roadUISet.AddRoadUIPrefab, roadUISet.parentObject);
        addRoadUI.name = "AddRoadUI";
        //这里给这个按钮加上创建新路线的方法

    }





    [System.Serializable]
    public class PartUI
    {
        public GameObject UIPrefab;
        public Transform parentObject;//挂在menu上
        public Vector2 UIsize;//UI大小
        public Vector2 firstUIPosition;//初始UI位置
        public Vector2 UIOffset;  // 相对偏移量
    }
    public PartUI partUISet;

    [System.Serializable]
    public class GoodListUI
    {
        public GameObject GoodsUI;//用于改变物品数量的UI（已经放在UI中）
        public GameObject PageUILeft;//翻页UI
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
        foreach (var i in partList)//生成列表
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //这里是根据路线数据显示内容
            n++;
            pageCount++;
        }

        goodUISet.GoodsUI.SetActive(true);
        LordGoodList();
        CheckGoodsPage();

    }

    public void EZPartUISpawn(int i)//用于独立测试（未采用）
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
        while(n<i)//生成列表
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //这里是根据路线数据显示内容
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
        //这里写载入当前是哪条路段

        //这里写载入当前路段的商品信息
        
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
        rt.localPosition = localPosition;  //保持相对父物体
        // 重置缩放和旋转，避免继承父物体的变换
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
    }

    public void OpenRoadUI()
    {
        OpenUI();
        RoadUISpawn();
    }

    public void OpenUI()//仅用于打开功能菜单（不更新功能菜单内容）（需搭配打开功能使用）
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()//仅用于关闭功能菜单
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

    public void CloseRoadUI()//仅用于消去路线列
    {
        for (int i = roadUISet.parentObject.childCount - 1; i >= 0; i--)//清空缓存列表
        {
            Destroy(roadUISet.parentObject.GetChild(i).gameObject);
        }
    }


}
