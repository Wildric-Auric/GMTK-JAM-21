using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject rope;
    public void ClosePanel()
    {
        var b = rope.GetComponentInChildren<RopeBehaviour>();
        panel.GetComponent<Animator>().SetTrigger("closePanel");
        float result = 1f;
        Debug.Log(panel.transform.Find("InputField").Find("Text").GetComponent<Text>().text);
        float.TryParse(panel.transform.Find("InputField").Find("Text").GetComponent<Text>().text, out result);
        if (result == 0)

        {
            result = 1;
        }
        Debug.Log(result);
        b.ropeForce = result;
        StartCoroutine(SETOFF(panel));
       
    }
    public void DeletePanel()
    {
        panel.GetComponent<Animator>().SetTrigger("closePanel");
        StartCoroutine(SETOFF(panel));
        Destroy(rope);
    }
    IEnumerator SETOFF(GameObject obj)
    { 
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
    }
}
