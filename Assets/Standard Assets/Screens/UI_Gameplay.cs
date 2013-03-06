using UnityEngine;
using System.Collections;

public class UI_Gameplay : MonoBehaviour {

	public Transform player; 
	
	private SpriteText distance;
	private SpriteText mph;
	private SpriteText height;
	
	// Use this for initialization
	void Start () 
	{
		distance = transform.Find("distance").GetComponent<SpriteText>(); 
		mph = transform.Find("speed").GetComponent<SpriteText>();
		height = transform.Find("height").GetComponent<SpriteText>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float s = player.rigidbody.velocity.magnitude * 2.237f * 7; // mph
		float h = player.position.y; // height
		float d = player.position.z * 10;

		distance.Text = d.ToString("N0");
		mph.Text = s.ToString("N2");
		height.Text = h.ToString("N2");
	}
}
