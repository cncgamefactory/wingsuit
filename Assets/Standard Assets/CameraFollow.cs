using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public Transform target; 

	private Vector3 offset;
	
	// Use this for initialization
	void Start () 
	{
		offset = target.position - transform.position;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = target.position - offset;

		float X_ROT = Utils.MapRange(Mathf.Abs(target.position.x), 0, 35, 11.9f, 17.3f);
		float Y_ROT = Utils.MapRange(Mathf.Abs(target.position.x), 0, 35, 0, 15.58f);
		float Z_ROT = (Utils.MapRange(Mathf.Abs(target.position.x), 0, 35, 0, 32.16f));
		
		if (target.position.x < 0)
		{
			Z_ROT = -Z_ROT;
		}
		
		if (target.position.x > 0)
		{
			Y_ROT = -Y_ROT;
		}
		
		Debug.Log(transform.position.x + ": " + X_ROT+ "," + Y_ROT + "," + Z_ROT);
		
		transform.rotation = Quaternion.Euler(new Vector3(X_ROT, Y_ROT, Z_ROT)); 
	}
}
