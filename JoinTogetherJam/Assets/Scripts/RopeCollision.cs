using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollision : MonoBehaviour
{
    LineRenderer rope;
    EdgeCollider2D edgeCollider;
    RopeBehaviour sl;

    Vector3 points;
    Vector2[] points2;

    private void Start()
    {
        transform.position = Vector3.zero; //To solve a problem that causes headache
        edgeCollider = this.gameObject.AddComponent<EdgeCollider2D>();
        rope = this.gameObject.GetComponent<LineRenderer>();
        sl = GetComponent<RopeBehaviour>();
        edgeCollider.isTrigger = true;
        points2 = new Vector2[sl.segmentLength];
        getNewPositions();
        edgeCollider.points = points2;
    }

    private void Update()
    {
        getNewPositions();

        edgeCollider.points = points2;
    }

    void getNewPositions()
    {
        for (int i = 0; i < rope.positionCount; i++)
        {
            points = rope.GetPosition(i);
            points2[i] = new Vector2(points.x, points.y);
        }
    }

}