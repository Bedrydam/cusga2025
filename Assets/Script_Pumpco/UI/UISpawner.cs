using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISpawner : MonoBehaviour
{
    public RoadListManeger roadListManeger;

    [Header("基础设置")]
    public GameObject uiPrefab;  // RPUI预制体
    public Transform parentObject;  // 拖入场景中的物体A作为父物体

    [Header("位置配置")]
    public Vector2 size;
    public Vector2 firstUILocalPosition;  // 第一个UI的本地坐标（相对于父物体）
    public Vector2 secondUIOffset;  // 第二个UI相对于第一个的偏移量
    public int totalUICount = 4;  // 总生成数量



    [System.Serializable]
    public class RoadUI
    {
        public GameObject UIPrefab;//预制体
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


            //根据路线数据修改内容
            n++;
        }

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

        foreach (var i in partList)//生成列表
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //根据路线数据修改内容
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
        while(n<4)//生成列表
        {
            GameObject RoadListUI = Instantiate(partUISet.UIPrefab, partUIPanel.transform);
            RoadListUI.name = $"PartList{n}";
            SetUIPosition(RoadListUI, partUISet.firstUIPosition + n * partUISet.UIOffset);
            //根据路线数据修改内容
            n++;
        }
    }




    void Start()
    {
        roadListManeger = GetComponentInChildren<RoadListManeger>();

        if (parentObject == null)
        {
            Debug.LogError("未指定父物体！");
            return;
        }

        //EZSpawn();
    }

    void SpawnChildUIs()
    {
        // 生成第一个UI
        GameObject firstUI = Instantiate(uiPrefab, parentObject);
        firstUI.name = "RPUI_0";
        SetUIPosition(firstUI, firstUILocalPosition);

        // 生成第二个UI
        if (totalUICount >= 2)
        {
            GameObject secondUI = Instantiate(uiPrefab, parentObject);
            secondUI.name = "RPUI_1";
            SetUIPosition(secondUI, firstUILocalPosition + secondUIOffset);

            // 生成剩余UI（按相同偏移量继续）
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
        rt.localPosition = localPosition;  // 使用localPosition保持相对父物体
        //rt.sizeDelta = size;

        // 重置缩放和旋转，避免继承父物体的变换
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
