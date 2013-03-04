using UnityEngine;
using System.Collections;

public class FontColor : MonoBehaviour {
	
	private TextMesh myMesh;
	public Color THIS_COLOR; 

	// Use this for initialization
	void Start () 
	{
		myMesh = gameObject.GetComponent<TextMesh>(); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
