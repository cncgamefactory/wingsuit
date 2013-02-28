using UnityEngine;
using System.Collections;

public class UI_Missions : MonoBehaviour {

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
		
		/*
		if (mGameState == "Missions")
		{
			Mission m1 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission1Id];
			Mission m2 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission2Id];
			Mission m3 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission3Id];
			
			if(GUI.Button(new Rect(100,100,800,600),"Missions\n" +
				m1.Name + "\n" + 
				m2.Name + "\n" + 
				m3.Name))
			{
				SwitchState("PostGame");
			}
		}
		*/
		
	}
	
	private void ProcessMouseClick(GameObject clicked)
	{
		switch (clicked.name)
		{
			case "btn_Okay":
				Bounce (clicked, "PostGame");
				break;
			default:
				break;
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
