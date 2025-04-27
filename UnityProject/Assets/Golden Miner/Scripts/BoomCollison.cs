using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomCollison : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();

    }

    public void Move()
    {
        transform.position += transform.up * -1 * 6* Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PropScript prop = collision.GetComponent<PropScript>();
        if (prop != null)
        {
            GameMode.Instance.BoomFunc();
            Destroy(gameObject);
        }
        Debug.Log(collision.name + "=====");
    }
}
