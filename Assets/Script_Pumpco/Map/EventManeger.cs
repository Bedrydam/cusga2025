using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManeger : MonoBehaviour
{
    public static event Action<bool> ToRoadMode;
    public bool isInRoadMode;

    // Start is called before the first frame update
    void Start()
    {
        isInRoadMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeRoadMode();
        }

    }

    public void ChangeRoadMode()
    {
        isInRoadMode = !isInRoadMode;
        Debug.Log("表锅我切换模式了喔");
        ToRoadMode?.Invoke(isInRoadMode);
    }



}
