using UnityEngine;
using System.Collections;

public class StickToGround : MonoBehaviour {
	
	public float height; 
	
	// Use this for initialization
	void Start () {
		
		height = transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 newPos = transform.position;
		newPos.y = height; 
		transform.position = newPos; 
	}
}
