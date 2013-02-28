using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Collections;
using EveryplayMiniJSON;

public class Everyplay : MonoBehaviour {

	public delegate void WasClosedDelegate();
	public event WasClosedDelegate WasClosed {
	    add {
	        RealWasClosed += value;
	        wasClosedDelegates.Add(value);
	    }

	    remove {
	        RealWasClosed -= value;
	        wasClosedDelegates.Remove(value);
	    }
	}
	private List<WasClosedDelegate> wasClosedDelegates = new List<WasClosedDelegate>();
	private event WasClosedDelegate RealWasClosed;

	public delegate void RecordingStartedDelegate();
	public event RecordingStartedDelegate RecordingStarted {
	    add {
	        RealRecordingStarted += value;
	        recordingStartedDelegates.Add(value);
	    }

	    remove {
	        RealRecordingStarted -= value;
	        recordingStartedDelegates.Remove(value);
	    }
	}

	private List<RecordingStartedDelegate> recordingStartedDelegates = new List<RecordingStartedDelegate>();
	private event RecordingStartedDelegate RealRecordingStarted;

	public delegate void RecordingStoppedDelegate();
	public event RecordingStoppedDelegate RecordingStopped {
	    add {
	        RealRecordingStopped += value;
	        recordingStoppedDelegates.Add(value);
	    }

	    remove {
	        RealRecordingStopped -= value;
	        recordingStoppedDelegates.Remove(value);
	    }
	}

	private List<RecordingStoppedDelegate> recordingStoppedDelegates = new List<RecordingStoppedDelegate>();
	private event RecordingStoppedDelegate RealRecordingStopped;

	public delegate void ThumbnailReadyAtFilePathDelegate(string filePath);
	public event ThumbnailReadyAtFilePathDelegate ThumbnailReadyAtFilePath {
	    add {
	        RealThumbnailReadyAtFilePath += value;
	        thumbnailReadyAtFilePathDelegates.Add(value);
	    }

	    remove {
	        RealThumbnailReadyAtFilePath -= value;
	        thumbnailReadyAtFilePathDelegates.Remove(value);
	    }
	}

	private List<ThumbnailReadyAtFilePathDelegate> thumbnailReadyAtFilePathDelegates = new List<ThumbnailReadyAtFilePathDelegate>();
	private event ThumbnailReadyAtFilePathDelegate RealThumbnailReadyAtFilePath;

	public delegate void ThumbnailLoadReadyDelegate(Texture2D texture);
	public delegate void ThumbnailLoadFailedDelegate(string error);

	public string clientId;
	public string clientSecret;
	public string redirectURI;

	private static Everyplay sharedInstance = null;
	private static bool appIsClosing = false;

	public static Everyplay SharedInstance {
		get {
			if(!sharedInstance) {
				sharedInstance = (Everyplay) FindObjectOfType(typeof(Everyplay));
				
				if(!sharedInstance) {
					bool shouldCreateInstance = !appIsClosing;

					if(Application.isEditor && !Application.isPlaying) {
						shouldCreateInstance = false;
					}

					if(shouldCreateInstance) {
						GameObject tmp = new GameObject("Everyplay");
						sharedInstance = tmp.AddComponent<Everyplay>();

						Debug.Log("Everyplay was not found on this scene. This is not a problem if you have added it to the first scene and you are currently editing some other scene. Just make sure you add it to the scene where you use it for the first time.");
					}
				}
				
				#if UNITY_IPHONE && !UNITY_EDITOR
				if(sharedInstance) {
					InitEveryplay(sharedInstance.clientId, sharedInstance.clientSecret, sharedInstance.redirectURI);
				}
				#endif
			}

			return sharedInstance;
		}
	}

	void Start() {
		Everyplay[] allInstances = FindObjectsOfType(typeof(Everyplay)) as Everyplay[];
			
		foreach(Everyplay ins in allInstances) {
			if(ins == Everyplay.SharedInstance) {
				DontDestroyOnLoad(gameObject);	
			}
			else if(Everyplay.SharedInstance) {
				Destroy(ins.gameObject);
			}
		}
	}

	void OnApplicationQuit() {
		RemoveAllEvents();
		appIsClosing = true;
		sharedInstance = null;
	}

	public void Show() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayShow();
		#endif
	}	
	
	public void StartRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayStartRecording();
		#endif
	}

	public void StopRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayStopRecording();
		#endif
	}	

	public void PauseRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayPauseRecording();
		#endif
	}	
	
	public void ResumeRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayResumeRecording();
		#endif
	}	

	public bool IsRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		return EveryplayIsRecording();
		#else
		return false;
		#endif
	}

	public bool IsPaused() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		return EveryplayIsPaused();
		#else
		return false;
		#endif
	}

	public bool SnapshotRenderbuffer() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		return EveryplaySnapshotRenderbuffer();
		#else
		return false;
		#endif
	}
	
	public bool IsSupported() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		return EveryplayIsSupported();
		#else
		return false;
		#endif
	}

	public void PlayLastRecording() {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplayPlayLastRecording();
		#endif
	}

	public void SetMetadata(string key, object val) {
		#if UNITY_IPHONE && !UNITY_EDITOR
		if(key != null && val != null) {
			Dictionary<string,object> dict = new Dictionary<string, object>();
			dict.Add(key, val);
			EveryplaySetMetadata(Json.Serialize(dict));
		}
		#endif
	}

	public void SetMetadata(Dictionary<string,object> dict) {
		#if UNITY_IPHONE && !UNITY_EDITOR
		if(dict != null) {
			if(dict.Count > 0) {
				EveryplaySetMetadata(Json.Serialize(dict));
			}
		}
		#endif
	}

	public void SetMaxRecordingMinutesLength(int minutes) {
		#if UNITY_IPHONE && !UNITY_EDITOR
		EveryplaySetMaxRecordingMinutesLength(minutes);
		#endif
	}

	public void LoadThumbnailFromFilePath(string filePath, ThumbnailLoadReadyDelegate readyDelegate, ThumbnailLoadFailedDelegate failedDelegate) {
		#if UNITY_IPHONE && !UNITY_EDITOR
		if(filePath != null) {
				StartCoroutine(LoadThumbnailEnumerator(filePath, readyDelegate, failedDelegate));
		}
		else {
			failedDelegate("Everyplay error: Thumbnail is not ready.");
		}
		#endif	
	}

	private void RemoveAllEvents() {
	    foreach(RecordingStartedDelegate del in recordingStartedDelegates) {
	        RealRecordingStarted -= del;
	    }
	    recordingStartedDelegates.Clear();

	    foreach(RecordingStoppedDelegate del in recordingStoppedDelegates) {
	        RealRecordingStopped -= del;
	    }
	    recordingStoppedDelegates.Clear();

	    foreach(ThumbnailReadyAtFilePathDelegate del in thumbnailReadyAtFilePathDelegates) {
	        RealThumbnailReadyAtFilePath -= del;
	    }
	    thumbnailReadyAtFilePathDelegates.Clear();
	}

	private IEnumerator LoadThumbnailEnumerator(string fileName, ThumbnailLoadReadyDelegate readyDelegate, ThumbnailLoadFailedDelegate failedDelegate) {
		WWW www = new WWW("file://" + fileName);
	
		yield return www;
	
		if(www.error != null) {
			failedDelegate("Everyplay error: " + www.error);
		}
		else {
			if(www.texture) {
				 readyDelegate(www.texture);
			}
			else {
				failedDelegate("Everyplay error: Loading thumbnail failed.");
			}
		}
	}
	
	private void EveryplayHidden(string msg) {
		if(RealWasClosed != null) {
			RealWasClosed();
		}
	}

	private void EveryplayRecordingStarted(string msg) {
		if(RealRecordingStarted != null) {
			RealRecordingStarted();
		}
	}
	
	private void EveryplayRecordingStopped(string msg) {
		if(RealRecordingStopped != null) {
			RealRecordingStopped();
		}
	}
	
	private void EveryplayThumbnailReadyAtFilePath(string filePath) {
		if(RealThumbnailReadyAtFilePath != null) {
			RealThumbnailReadyAtFilePath(filePath);
		}
	}
	
	#if UNITY_IPHONE && !UNITY_EDITOR

	[DllImport("__Internal")]
	private static extern void InitEveryplay(string clientId, string clientSecret, string redirectURI);
	
	[DllImport("__Internal")]
	private static extern void EveryplayShow();

	[DllImport("__Internal")]
	private static extern void EveryplayStartRecording();

	[DllImport("__Internal")]
	private static extern void EveryplayStopRecording();

	[DllImport("__Internal")]
	private static extern void EveryplayPauseRecording();

	[DllImport("__Internal")]
	private static extern void EveryplayResumeRecording();

	[DllImport("__Internal")]
	private static extern bool EveryplayIsRecording();

	[DllImport("__Internal")]
	private static extern bool EveryplayIsPaused();

	[DllImport("__Internal")]
	private static extern bool EveryplaySnapshotRenderbuffer();

	[DllImport("__Internal")]
	private static extern void EveryplayPlayLastRecording();

	[DllImport("__Internal")]
	private static extern void EveryplaySetMetadata(string json);

	[DllImport("__Internal")]
	private static extern void EveryplaySetMaxRecordingMinutesLength(int minutes);

	[DllImport("__Internal")]
	private static extern bool EveryplayIsSupported();

	#endif
}