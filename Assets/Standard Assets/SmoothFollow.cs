using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour {

	public Transform target;

	public float X_OFFSET = 0.0f;
	public float Y_OFFSET = 3.3f;
	public float Z_OFFSET = -5.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () 
	{
		
		/*
		 * 
		Vector3 myPos = target.transform.position;
		myPos.z -= OFFSET_BEHIND;
		myPos.y += OFFSET_ABOVE;
		transform.position = myPos; 

		*/

		Vector3 playerMarker = target.position; 
		playerMarker.x += X_OFFSET;
		playerMarker.y += Y_OFFSET;
		playerMarker.z += Z_OFFSET;
		
		if(playerMarker.y < 0)
			playerMarker.y = 0; 
		
		transform.position = Vector3.Lerp(transform.position, playerMarker, Time.deltaTime * 10);
//    	transform.rotation = Quaternion.Lerp(transform.rotation, playerMarker.rotation, Time.deltaTime * 100);
		
	}

}
 
