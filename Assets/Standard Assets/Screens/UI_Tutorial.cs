using UnityEngine;
using System.Collections;

public class UI_Tutorial : MonoBehaviour {

	public GameObject Logic; 
	private GameLoop gLoop; 
	
	// Use this for initialization
	void Start () 
	{
		gLoop = Logic.GetComponent<GameLoop>(); 
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			
			Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log (hit.collider.gameObject.name);
				ProcessMouseClick(hit.collider.gameObject);
			}
		}
	}
	
	private void ProcessMouseClick(GameObject clicked)
	{
		if (clicked.name == ("btn_Start"))
		{
			PersistentData.mPersistentData.mUserData.DidTutorial = true;
			DelayedSwitch("Gameplay");
		}
	}
	
	private void Bounce(GameObject go, string state)
	{
		float bounceAmt = 0.5f;
		iTween.PunchScale(go, iTween.Hash("x",bounceAmt,"y",bounceAmt,"z",bounceAmt,"onComplete","DelayedSwitch","onCompleteTarget",gameObject,"onCompleteParams",state,"time",0.5f));
	}
	
	private void DelayedSwitch(string state)
	{
		gLoop.SwitchState(state);
	}
}
