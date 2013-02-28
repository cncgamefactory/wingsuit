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
			pLoop.AddHeightBoost(4.5f);
//			Debug.Log("HEIGHT BOOST!");
			
			for(int i = 0; i < transform.GetChildCount(); i++)
			{
				GameObject go = transform.GetChild(i).gameObject;
				go.renderer.material.color = Color.white;
			}
		}
	}
}
