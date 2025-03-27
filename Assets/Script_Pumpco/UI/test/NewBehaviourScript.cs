using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<GameObject> UIs;
    public GameObject LastUI;
    public GameObject StartList;

    // Start is called before the first frame update
    void Start()
    {
        CloseUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    public void OpenUI(GameObject UI)
    {
        foreach (GameObject go in UIs)
        {
            go.SetActive(false);
        }
        UI.SetActive(true);
        
    }

    public void SetLastUI(GameObject LlstUI)
    {
        LastUI=LlstUI;
    }

    public void CloseUI()
    {
        foreach(GameObject go in UIs)
        {
            go.SetActive(false);
        }
        LastUI.SetActive(true);
    }

    public void GamneStart()
    {
        StartList.SetActive(true);
    }

}
