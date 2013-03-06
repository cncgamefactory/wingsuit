using UnityEngine;
using System.Collections;

public class CameraJitter : MonoBehaviour {
	
	static public CameraJitter sharedInstance; 
	
	public float ShakeIntensity = .004f; 
	
	private PlayerLoop pLoop; 
	
	void Awake()
	{
		sharedInstance = this; 
	}
	
	// Use this for initialization
	void Start () {
		pLoop = GameObject.Find("Player").GetComponent<PlayerLoop>(); 
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pLoop.mGameState == "Gameplay")
		{
			transform.position += Random.insideUnitSphere * ShakeIntensity;
    		transform.rotation = new Quaternion(transform.rotation.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
                                  transform.rotation.y + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
                                  transform.rotation.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
                                  transform.rotation.w + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);	
		}
	}
}
