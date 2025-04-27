using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    public Transform startTrans;    //起始点
    LineRenderer lineRenderer;
    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, startTrans.position);
        lineRenderer.SetPosition(1, transform.position);
    }
}
