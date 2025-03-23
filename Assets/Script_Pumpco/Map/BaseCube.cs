using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCube : MonoBehaviour
{
    public int noX;
    public int noY;
    public bool canCross;
    public bool isBuild;
    public int length;

    public bool roadMode;


    // Start is called before the first frame update
    void Start()
    {
        roadMode = false;
    }

    private void OnEnable()
    {
        EventManeger.ToRoadMode += ChangeRoadMode;
    }

    private void OnDisable()
    {
        EventManeger.ToRoadMode -= ChangeRoadMode;
    }

    // Update is called once per frame
    void Update()
    {
        if (roadMode) 
        {
            


        }



        else { }


    }

    public void ChangeRoadMode(bool x)
    {
        roadMode = x;
        Debug.Log(noX+"-"+noY+":�������·��ģʽ��" + x + "���");

        if (roadMode && !canCross)//���ڱ�ǲ���ͨ���Լ����渴λ
        {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            s.color = Color.black;
        }
        else
        {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            s.color = Color.white;
        }

    }

   

}
