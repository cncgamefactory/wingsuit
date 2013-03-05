using UnityEngine;
using System.Collections;

public class RingRotate : MonoBehaviour {
	
	public float XROTAMT = 50.0f;
	public float YROTAMT = 0.0f;
	public float ZROTAMT = 0.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		    transform.Rotate (XROTAMT*Time.deltaTime,YROTAMT*Time.deltaTime,ZROTAMT*Time.deltaTime); //rotates 50 degrees per second around z axis

	}
}
