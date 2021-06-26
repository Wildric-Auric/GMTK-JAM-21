using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;

/// WELCOME TO THE HOLY MESSY CODE! 
public class RopeBehaviour : MonoBehaviour {

    public Transform StartPoint;
    public Transform EndPoint;

    LineRenderer lineRenderer;
    List<RopeSegment> ropeSegments = new List<RopeSegment>();

    [Header("rope properties")]
    [SerializeField] float lineWidth = 0.1f;
    [FormerlySerializedAs("LengthOfSegments")] float ropeSegLen = 0.25f;
    public int segmentLength = 35;

    bool MoveToTarget = false;
    int indexMousePos;

    [Space(9)]
    [Header("Collision")]
    public GameObject target;
    [SerializeField] float speedOfLerp = 0.1f;
    [SerializeField] float speedOfLerpLowVel = 1;
    public float ropeForce = 1f;
    [SerializeField] float deadTime = 1f;

    GameObject arrow;

    public float BonusLerp = 0.2f;
    Vector2 perp; 
    bool coIsRunning; //Checking is coroutine is running in OnCollision
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = StartPoint.position;
        arrow = GameObject.Find("arrow").transform.Find("Arrow").gameObject;
        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }
    void Update()
    {
        this.DrawRope();
        //Calculate the slope; should change it to do it just once
        Vector2 diff = StartPoint.position - EndPoint.position;
        perp = Vector2.Perpendicular(diff);
        // Alternatively use perp = Vector3.Cross(diff, Vector3.forward);
        float xStart = StartPoint.position.x;
        float xEnd = EndPoint.position.x;
        float currX = target.transform.position.x;

        float ratio = (currX - xStart) / (xEnd - xStart);
        if (ratio > 0) {
            this.indexMousePos = (int)(this.segmentLength * ratio);
        }
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }

            if (this.MoveToTarget && i == indexMousePos) {
                RopeSegment segment = this.ropeSegments[i];
                RopeSegment segment2 = this.ropeSegments[i + 1];
                //segment.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                //segment2.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                segment.posNow = new Vector2(target.transform.position.x, target.transform.position.y);
                segment2.posNow = new Vector2(target.transform.position.x, target.transform.position.y);
                this.ropeSegments[i] = segment;
                this.ropeSegments[i + 1] = segment2;
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
    /////COLLISION BEHAVIOUR
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!coIsRunning)
        {
            arrow.SetActive(true);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            StartCoroutine(ROPECOL(rb, rb.velocity, rb.gravityScale));
        }
    }
    private IEnumerator ROPECOL(Rigidbody2D rb, Vector2 initialVel, float initialGrav)
    {
        coIsRunning = true;
        MoveToTarget = true;
        rb.gravityScale = 0;
        var magn = initialVel.magnitude;
        var newVec = (initialVel - new Vector2(GameObject.Find("Circle").transform.position.x, GameObject.Find("Circle").transform.position.y)).normalized;
        while (rb.velocity != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newVec +=new Vector2(newVec.x, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                newVec += new Vector2( -newVec.x, 0);
            }
            var targ = -newVec;
            var rotAngle = Vector2.Angle(Vector3.up, targ);
            Quaternion a = Vector3.Cross(targ, Vector3.up).z < 0 ?
            arrow.transform.rotation = Quaternion.Euler(0, 0, rotAngle) :
            arrow.transform.rotation = Quaternion.Euler(0, 0, -rotAngle);

            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, speedOfLerp*(Convert.ToInt16(rb.velocity.magnitude>1)) 
            + speedOfLerpLowVel* (Convert.ToInt16(rb.velocity.magnitude < 1)));
            yield return null;
        }
        rb.velocity = -(newVec).normalized*ropeForce*magn; ////SHOULD CHANGE IT LATER
        rb.gravityScale = initialGrav;
        MoveToTarget = false;
        arrow.SetActive(false);
        yield return new WaitForSeconds(deadTime);
        coIsRunning = false;
        
    }
}
