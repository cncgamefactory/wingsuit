using UnityEngine;
using System.Collections;

public class EveryplayTest : MonoBehaviour {
	
	private bool recording = false;
	private bool recordingDone = false;

	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 138, 48), "Everyplay")) {
			Everyplay.SharedInstance.Show();
#if UNITY_EDITOR
			Debug.Log("Everyplay view is not available in the Unity editor. Please compile and run on a device.");
#endif
		}
		
		if(recording && GUI.Button(new Rect(10, 64, 138, 48), "Stop Recording")) {
			Everyplay.SharedInstance.StopRecording();
			recording = false;
			recordingDone = true;
#if UNITY_EDITOR
			Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
		}
		else if(!recording && GUI.Button(new Rect(10, 64, 138, 48), "Start Recording")) {
			Everyplay.SharedInstance.StartRecording();	
			recording = true;
			recordingDone = false;
#if UNITY_EDITOR
			Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
#endif
		}

		if(recordingDone && GUI.Button(new Rect(10, 118, 138, 48), "Play Last Recording")) {
			Everyplay.SharedInstance.PlayLastRecording();
#if UNITY_EDITOR
			Debug.Log("The video playback is not available in the Unity editor. Please compile and run on a device.");
#endif
		}
	}
}