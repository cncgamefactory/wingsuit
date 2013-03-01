using UnityEngine;
using System.Collections;

public class UI_Missions : MonoBehaviour {

	public GameObject Logic; 
	private GameLoop gLoop; 
	
	public GameObject m1Btn;
	public GameObject m2Btn;
	public GameObject m3Btn;
	public GameObject prevm1Btn;
	public GameObject prevm2Btn;
	public GameObject prevm3Btn;
	
	private Mission m1;
	private Mission m2;
	private Mission m3;
	private Mission prev_m1;
	private Mission prev_m2;
	private Mission prev_m3;
	
	Vector3 placement1;
	Vector3 placement2;
	Vector3 placement3;
	
	float secondsToWait = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		gLoop = Logic.GetComponent<GameLoop>(); 
		
		placement1 = new Vector3(-0.092743f,8.229965f,0);
		placement2 = new Vector3(-0.092743f,4.12f,0);
		placement3 = new Vector3(-0.092743f,-.02f,0);
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
			
	public void RefreshMissionData()
	{
		m1 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission1Id];
		m2 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission2Id];
		m3 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.Mission3Id];
		prev_m1 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.PrevMission1Id];
		prev_m2 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.PrevMission2Id];
		prev_m3 = (Mission)PersistentData.mPersistentData.mMissionData[PersistentData.mPersistentData.mUserData.PrevMission3Id];

		m1Btn.transform.Find("text").GetComponent<TextMesh>().text = m1.Name;
		m2Btn.transform.Find("text").GetComponent<TextMesh>().text = m2.Name;
		m3Btn.transform.Find("text").GetComponent<TextMesh>().text = m3.Name;
		prevm1Btn.transform.Find("text").GetComponent<TextMesh>().text = prev_m1.Name;
		prevm2Btn.transform.Find("text").GetComponent<TextMesh>().text = prev_m2.Name;
		prevm3Btn.transform.Find("text").GetComponent<TextMesh>().text = prev_m3.Name;
		prevm1Btn.transform.Find("check").gameObject.active = false;
		prevm2Btn.transform.Find("check").gameObject.active = false;
		prevm3Btn.transform.Find("check").gameObject.active = false;
		
		prevm1Btn.transform.localPosition = placement1;
		prevm2Btn.transform.localPosition = placement2;
		prevm3Btn.transform.localPosition = placement3;
		
		m1Btn.transform.localPosition = new Vector3(placement1.x+30,placement1.y, placement1.z);
		m2Btn.transform.localPosition = new Vector3(placement2.x+30,placement2.y, placement2.z);
		m3Btn.transform.localPosition = new Vector3(placement3.x+30,placement3.y, placement3.z);
		secondsToWait = .5f;
		
		
	}
	
	
	public void OnScreenLoaded()
	{
		Debug.Log("OnScreenLoaded");
		
		bool needSave = false; 
		
		if (m1.Id != prev_m1.Id)
		{
			StartCoroutine(TransitionNewMission(prevm1Btn,m1Btn));
			PersistentData.mPersistentData.mUserData.PrevMission1Id = m1.Id;
			needSave = true; 
		}

		if (m2.Id != prev_m2.Id)
		{
			StartCoroutine(TransitionNewMission(prevm2Btn,m2Btn));
			PersistentData.mPersistentData.mUserData.PrevMission2Id = m2.Id;
			needSave = true; 
		}
		
		if (m3.Id != prev_m3.Id)
		{
			StartCoroutine(TransitionNewMission(prevm3Btn,m3Btn));
			PersistentData.mPersistentData.mUserData.PrevMission3Id = m3.Id;
			needSave = true; 
		}
		
		if (needSave)
		{
			PersistentData.mPersistentData.mSaveLoad.SaveUser(PersistentData.mPersistentData.mUserData);
		}
		
	}
	
	private IEnumerator TransitionNewMission(GameObject oldObj, GameObject newObj)
	{
		secondsToWait += 0.75f;
		GameObject check = oldObj.transform.Find("check").gameObject;
		check.active = true; 
		iTween.PunchScale(check, new Vector3(.75f,1,.75f),secondsToWait);

		yield return new WaitForSeconds(secondsToWait);
		
		Vector3 newObjPos = oldObj.transform.position;
		iTween.MoveTo(oldObj, new Vector3 (oldObj.transform.position.x - 40,oldObj.transform.position.y,oldObj.transform.position.z), 1.0f);
		iTween.MoveTo(newObj, newObjPos, 0.75f);
		
		
	}
	
	
}
