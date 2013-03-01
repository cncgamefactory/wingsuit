using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
	
	private Vector3 scale;
	// Use this for initialization
	void Start () 
	{
		scale = gameObject.transform.localScale;
		
		ScaleDown();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void ScaleUp()
	{
		iTween.ScaleTo(gameObject, iTween.Hash("x",scale.x,"y",scale.y,"z",scale.z,"easeType",iTween.EaseType.linear, "onComplete","ScaleDown","onCompleteTarget",gameObject,"time",2.0f));
	}

	void ScaleDown()
	{
		iTween.ScaleTo(gameObject, iTween.Hash("x",scale.x *.9f,"y",scale.y *.9f,"z",scale.z *.9f,"easeType",iTween.EaseType.linear,"onComplete","ScaleUp","onCompleteTarget",gameObject,"time",2.0f));
	}
}
