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
        Debug.Log(noX+"-"+noY+":表锅绘制路线模式是" + x + "了喔");

        if (roadMode && !canCross)//用于标记不可通行以及保存复位
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
