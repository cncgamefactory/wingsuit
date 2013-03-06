using UnityEngine;
using System.Collections;

public class MarkerLoop : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.name == "Player")
		{
			PlayerLoop pLoop = col.GetComponent<PlayerLoop>();
			pLoop.AddHeightBoost(1.5f);
//			Debug.Log("HEIGHT BOOST!");
			
			pLoop.numHeightBoostersHit++; 
			
			UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_BOING);
			
			GA.API.Design.NewEvent("Boost:Height",transform.position); 
		}
	}
}
