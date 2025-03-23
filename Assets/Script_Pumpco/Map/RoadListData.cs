using System.Collections.Generic;
using UnityEngine;

public class RoadListData : MonoBehaviour
{
    //用于搭建白盒


    [System.Serializable]
    public class Road_Part//用于存储某段线路（中的每一个方块）
    {
        //public Road_BaseCube[] baseCube;

        //public GameObject[] baseCube;
        public List<GameObject> baseCube;

        public void AddBaseCube(GameObject newBaseCube)//用于给这段线路添加新方块
        {
            baseCube.Add(newBaseCube);
            Debug.Log("表锅我加了个" + newBaseCube + newBaseCube.transform.position.x+"-"+newBaseCube.transform.position.y + "喔");
            RefreshCube();
        }

        public void showCube()
        {
            foreach (var i in baseCube)
            {
                Debug.Log(i.transform.position.x + "-" + i.transform.position.y);
            }
            if (baseCube == null)
            {
                Debug.Log("表锅里面没东西喔？");
            }

        }

        public void RefreshCube()
        {
            foreach(var i in baseCube)
            {
                SpriteRenderer s=i.GetComponent<SpriteRenderer>();
                s.color = Color.cyan;
            }
        }

        public void DeleteCube(GameObject newCube)
        {
            baseCube.Remove(newCube);
        }

        public void DeleteCubePlus(GameObject newCube)
        {
            int n = baseCube.FindIndex(Cube => Cube == newCube);
            if (n > 0)
            {
                baseCube.RemoveRange(n,baseCube.Count - n);
            }
            else{Debug.Log("表锅我没找到要删的东西喔？");}

        }


    }

    public Road_Part part;


    private void OnEnable()
    {
        //MouseTrigger.Road_AddCube += part.AddBaseCube;
    }

    private void OnDisable()
    {
        //MouseTrigger.Road_AddCube -= part.AddBaseCube;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            part.showCube();
 
        }

    }

}
