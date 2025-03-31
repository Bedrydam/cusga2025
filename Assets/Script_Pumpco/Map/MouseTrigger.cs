using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MouseTrigger : MonoBehaviour
{
    public static event Action<List<GameObject>, int> SavePart;
    public static event Action<bool> PartCompleted;

    public bool roadMode;
    public bool completed;//�����ж��Ƿ��ǺϷ�·��
    public bool locked;//�����ж��Ƿ�����

    public GameObject lastCube;
    public GameObject thisCube;
    public int cubeX;
    public int cubeY;
    public BaseCube cubeScript;

    public bool mouseOn;
    public Vector2 mousePosition;

    private bool fristPoint;//�����ж��Ƿ��Ǹտ�ʼ��
    private float lasttime;//������
    private bool beLoaded;//������










    //��ʱ����ĳ����·�ķ���
    [System.Serializable]
    public class RoadPart//�洢�����·�ķ����볤��
    {
        public List<GameObject> baseCube;
        public int length;

        public void AddBaseCube(GameObject newBaseCube)//���ڸ������·����·���
        {
            baseCube.Add(newBaseCube);
            Debug.Log("����Ҽ��˸�" + newBaseCube + newBaseCube.transform.position.x + "-" + newBaseCube.transform.position.y + "�");
            RefreshCube();
            LengthCalculated();
        }

        public void LengthCalculated()//���ڼ���·�߳���
        {
            length = 0;
            foreach(var i in baseCube)
            {
                BaseCube BCS = i.GetComponent<BaseCube>();
                length = length + BCS.length;
            }

        }

        public void showCube()//��ӡ���з���
        {
            foreach (var i in baseCube)
            {
                Debug.Log(i.transform.position.x + "-" + i.transform.position.y);
            }
            if (baseCube == null)
            {
                Debug.Log("�������û����ร�");
            }
            else
            {
                Debug.Log(length);
            }
        }

        public void RefreshCube()//���ڸı䷽����ɫ
        {
            foreach (var i in baseCube)
            {
                SpriteRenderer s = i.GetComponent<SpriteRenderer>();
                s.color = Color.cyan;
            }
        }

        public int SearchCube(GameObject newCube)
        {
            int n = baseCube.FindIndex(Cube => Cube == newCube);
            return n;
        }

        public void DeleteCube(GameObject newCube)//����ɾ��ĳ���ض��ķ���
        {
            baseCube.Remove(newCube);
        }

        public void DeleteCubePlus(GameObject newCube)//����ɾ��ĳ�����鼰�Ժ�ķ���
        {
            int n = SearchCube(newCube);
            if (n > 0)
            {
                foreach (var i in baseCube)//���÷�����ɫ
                {
                    SpriteRenderer s = i.GetComponent<SpriteRenderer>();
                    s.color = Color.white;
                }
                baseCube.RemoveRange(n, baseCube.Count - n);
            }
            else { Debug.Log("�����û�ҵ�Ҫɾ�Ķ���ร�"); }

        }

        
    }

    public RoadPart thisPart;










    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        cubeX = -1;
        cubeY = -1;

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (roadMode)
        {
            RoadSet();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            thisPart.showCube();

        }


    }





    private void OnEnable()
    {
        EventManeger.ToRoadMode += ChangeRoadMode;
        RoadListManeger.SendLastCube += GetLastCube;
    }

    private void OnDisable()
    {
        EventManeger.ToRoadMode -= ChangeRoadMode;
        RoadListManeger.SendLastCube -= GetLastCube;
    }

    public void ChangeRoadMode(bool x)
    {
        roadMode = x;
        fristPoint = true;
        Debug.Log("Trigger:�������·��ģʽ��" + x + "���");
    }

    public void GetLastCube(GameObject last)
    {
        lastCube = last;
    }










    public void getCube()//��ȡָ���ķ����Լ���basecube�ű�
    {
        RaycastHit2D inHit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (inHit.collider != null)
        {
            //Debug.Log("����������" + inHit.collider.gameObject.name+"�");
            if (inHit.collider.CompareTag("Cube"))
            {
                thisCube = inHit.collider.gameObject;
                cubeScript = thisCube.GetComponent<BaseCube>();
                Debug.Log("�����ѡ����" + thisCube.transform.position.x + "-" + thisCube.transform.position.y + "�");
            }
        }
        else
        {
            Debug.Log("�����û��׼�");
        }
    }















    public void RoadSet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("����㰴�������ร�");
            getCube();
            
            if (fristPoint&&lastCube==null)//�����ж��Ƿ�Ϊ��ͷ �Լ��ж��Ƿ�����·��
            {
                if (cubeScript.isBuild)
                {
                    mouseOn = true;
                    fristPoint = false;
                    lastCube = null;
                    locked = false;
                }
                else { Debug.Log("�����ûѡ�н���ร�"); }

            }
            else
            {
                if (thisCube == lastCube)//�����ж��Ƿ�������»�
                {
                    mouseOn = true;
                    lastCube = null;
                }
                else if (thisPart.SearchCube(thisCube) > 0)//�����ж��Ƿ�Ϊ�Ѽ�¼����
                {
                    thisPart.DeleteCubePlus(thisCube);
                    mouseOn = true;
                    lastCube = null;
                    locked = false;
                }
                
                else{ Debug.Log("�����û�������»�ร�"); }

            }

        }

        if (mouseOn)
        {
            //Debug.Log("����㻹�ڰ�ร�");
            getCube();
            if(lastCube != thisCube)//�����뷽�飬����һ��
            {
                if (cubeScript.canCross)//����ͨ�У�����һ��
                {
                    if (thisPart.SearchCube(thisCube) > 0)//��Ϊ�Ѽ�¼���飬��ɾ����������
                    {
                        thisPart.DeleteCubePlus(thisCube);
                        locked = false;
                        lastCube = null;
                    }

                    if (!locked)//�����������������������C�ϱ�
                    {
                        thisPart.AddBaseCube(thisCube);

                    }
                    else { Debug.Log("����㱻������ร�"); }
                }

                if ((!cubeScript.canCross || cubeScript.isBuild) && thisCube != thisPart.baseCube.First())//�����߼�������ͨ�л��ǽ������Ҳ�Ϊ��㣩
                {
                    Debug.Log("�����Ҫ������ร�");
                    locked = true;
                }

                lastCube = thisCube;//��¼��ǰ����
            }

            
            
            //������ӷ���
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("������ɿ������ร�");
            mouseOn = false;
            BaseCube ends = thisPart.baseCube[^1].GetComponent<BaseCube>();
            completed = ends.isBuild;
            Debug.Log("������·����ɶ���" + completed + "�");
            //����д�жϴ���
        }

    }

    public void SaveThisPart()
    {
        Debug.Log("����ұ�����ร�");
        SavePart?.Invoke(thisPart.baseCube, thisPart.length);
    }

    public void ShowDetails()//������������ʾ��ϢUI
    {

    }

}
