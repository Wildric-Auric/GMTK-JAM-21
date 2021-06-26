using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingStep : MonoBehaviour
{
    public bool settingStep = true;
    [SerializeField] GameObject target;
    bool second;
    bool canPlace = true;
    bool canPlace1 = false;
    float time = 1f;
    GameObject rope;
    BallForce Ball;
    GameObject panel;
    [SerializeField] Transform arrow;
    private void Start()
    {
        Ball = FindObjectOfType<BallForce>();
        panel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
    }
    void Update()
    {
        if (settingStep)
        {
            arrow.gameObject.SetActive(true);
            arrow.transform.position = Ball.transform.position; //It's already 17:02!!!!
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var targ = Ball.forces;
            var rotAngle = Vector2.Angle(Vector3.up, targ);
            Quaternion a = Vector3.Cross(targ, Vector3.up).z < 0 ?
            arrow.rotation = Quaternion.Euler(0, 0, rotAngle) :
            arrow.rotation = Quaternion.Euler(0, 0, -rotAngle);
            
            if (Input.GetMouseButtonUp(0))
            { 
                if (!second && canPlace && !panel.activeSelf)
                {
                    rope = Instantiate(Resources.Load("Rope") as GameObject);
                    rope.transform.Find("manager").GetComponent<RopeBehaviour>().target = target;
                    rope.transform.Find("startPoint").position = mousePos;
                    rope.transform.Find("endPoint").position = mousePos;
                    second = true;
                    canPlace = false;
                    StartCoroutine(WAIT1());
                }
            }
            if (second && canPlace1 && !panel.activeSelf)
            {
                rope.transform.Find("endPoint").position = mousePos;
                if (Input.GetMouseButtonDown(0))
                {
                    second = false;
                    StartCoroutine(WAIT());
                    canPlace1 = false;
                }
            }
        }
        if (Input.GetKeyDown("s") && settingStep)
        {
            arrow.gameObject.SetActive(false);
            panel.SetActive(false);
            settingStep = false;
            Ball.Launch = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ball.renit();
            panel.SetActive(false);
            GameObject[] ropes = GameObject.FindGameObjectsWithTag("rope");
            foreach (GameObject rope in ropes)
            {
                if (rope != null)
                {
                    Destroy(rope);
                }
            }
            settingStep = true;
        }
    }
    IEnumerator WAIT()
    {
        yield return new WaitForSeconds(time);
        canPlace = true;
    }
    IEnumerator WAIT1()
    {
        yield return time;
        canPlace1 = true;
    }
}
