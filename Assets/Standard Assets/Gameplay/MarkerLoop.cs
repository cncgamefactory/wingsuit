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
		Debug.Log("Hey I collided with " + col.name);
		if (col.name == "Player")
		{
			PlayerLoop pLoop = col.GetComponent<PlayerLoop>();
			pLoop.AddBoost(4.5f);
			
			for(int i = 0; i < transform.GetChildCount(); i++)
			{
				GameObject go = transform.GetChild(i).gameObject;
				go.renderer.material.color = Color.blue;
			}
		}
	}
}
