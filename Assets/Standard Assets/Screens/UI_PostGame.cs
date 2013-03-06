using UnityEngine;
using System.Collections;

public class UI_PostGame : MonoBehaviour {
	
	public GameObject Logic; 
	private GameLoop gLoop; 
	
	public GameObject text_Distance;
	public GameObject text_MPH; 
	public GameObject text_Altitude;
	
	public GameObject best_Distance; 
	public GameObject best_MPH; 
	public GameObject best_Altitude; 
	
	
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
	
	public void OnScreenLoaded()
	{
		Debug.Log ("Loaded post game screen");
		
		iTween.PunchScale(best_Distance,new Vector3(.5f,.5f,.5f),1.0f);
		iTween.PunchScale(best_MPH,new Vector3(.5f,.5f,.5f),1.0f);
		iTween.PunchScale(best_Altitude,new Vector3(.5f,.5f,.5f),1.0f);

	}
	
	public void RefreshScore(float distance, float speed, float altitude)
	{
		text_Distance.GetComponent<SpriteText>().Text = distance.ToString("N0") + "m";
		text_MPH.GetComponent<SpriteText>().Text = "Top Speed: " + speed.ToString("N2") + "mph";
		text_Altitude.GetComponent<SpriteText>().Text = "Max Altitude: " + altitude.ToString("N2") + "m";
		
		best_Altitude.active = false;
		best_Distance.active = false; 
		best_MPH.active = false; 
		
		if (distance > PersistentData.mPersistentData.mUserData.BestDistance)
		{
			best_Distance.active = true; 
		}
		if (speed > PersistentData.mPersistentData.mUserData.BestSpeed)
		{
			best_MPH.active = true; 
		}
		if (altitude > PersistentData.mPersistentData.mUserData.BestHeight)
		{
			best_Altitude.active = true; 
		}
	}
	
	private void ProcessMouseClick(GameObject clicked)
	{
		switch (clicked.name)
		{
			case "btn_Missions":
				Bounce (clicked, "Missions");
				break;
			case "btn_PlayAgain":
				Bounce (clicked, "Gameplay");
				break;
			case "btn_Home":
				Bounce (clicked, "MainMenu");
				break;
			case "btn_Settings":
				Bounce (clicked, "Settings");
				break;
			case "btn_Polaroid":
				Everyplay.SharedInstance.PlayLastRecording();
				break;
			default:
				break;
		}
	}
	
	private void Bounce(GameObject go, string state)
	{
		UI_SFX.SharedInstance.Play(UI_SFX.SharedInstance.SFX_BOING); 

		float bounceAmt = 0.5f;
		iTween.PunchScale(go, iTween.Hash("x",bounceAmt,"y",bounceAmt,"z",bounceAmt,"onComplete","DelayedSwitch","onCompleteTarget",gameObject,"onCompleteParams",state,"time",0.5f));
	}
	
	private void DelayedSwitch(string state)
	{
		gLoop.SwitchState(state);
	}
}
