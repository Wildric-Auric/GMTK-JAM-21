using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class alwaysFloat : MonoBehaviour
{
    Text txt;
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float result = 1f;
        float.TryParse(txt.ToString(), out result);
        if (result < 0.1f || result > 2f)
        {
            result = 1f;
        }
        txt.text = result.ToString();
    }
}
