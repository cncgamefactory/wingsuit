using UnityEngine;
using System.Collections;

public class UI_Gameplay : MonoBehaviour {

	public Transform player; 
	
	private TextMesh distance;
	private TextMesh mph;
	private TextMesh height;
	
	// Use this for initialization
	void Start () 
	{
		distance = transform.Find("distance").GetComponent<TextMesh>(); 
		mph = transform.Find("mph").GetComponent<TextMesh>();
		height = transform.Find("height").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float s = player.rigidbody.velocity.magnitude * 2.237f; // mph
		float h = player.position.y * 3.28084f - 14.5f; // height in feet
		
		distance.text = player.position.z.ToString("N0") + "m";
		mph.text = s.ToString("N2") + "mph";
		height.text = h.ToString("N2") + "ft";
	}
}
