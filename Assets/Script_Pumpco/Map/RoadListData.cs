using System.Collections.Generic;
using UnityEngine;

public class RoadListData : MonoBehaviour
{
    //���ڴ�׺�


    [System.Serializable]
    public class Road_Part//���ڴ洢ĳ����·���е�ÿһ�����飩
    {
        //public Road_BaseCube[] baseCube;

        //public GameObject[] baseCube;
        public List<GameObject> baseCube;

        public void AddBaseCube(GameObject newBaseCube)//���ڸ������·����·���
        {
            baseCube.Add(newBaseCube);
            Debug.Log("����Ҽ��˸�" + newBaseCube + newBaseCube.transform.position.x+"-"+newBaseCube.transform.position.y + "�");
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
                Debug.Log("�������û����ร�");
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
            else{Debug.Log("�����û�ҵ�Ҫɾ�Ķ���ร�");}

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
