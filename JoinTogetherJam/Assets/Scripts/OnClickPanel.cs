using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickPanel : MonoBehaviour
{
    GameObject panel;
    ButtonManager BM;
    Text inputTxt;
    RopeBehaviour RB;
    private void Start()
    {
        panel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        BM = FindObjectOfType<ButtonManager>();
        RB = transform.parent.Find("manager").GetComponent<RopeBehaviour>();
     
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (panel.activeSelf)
            {
                BM.panel = panel;
                BM.rope = transform.parent.gameObject;
            }
            else
            {
                panel.SetActive(true);
                BM.panel = panel;
                BM.rope = transform.parent.gameObject;
            }
        }
    }
    void Update()
    {
        
    }
}
