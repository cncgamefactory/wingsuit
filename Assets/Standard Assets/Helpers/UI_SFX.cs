using UnityEngine;
using System.Collections;

public class UI_SFX : MonoBehaviour {

	public static UI_SFX SharedInstance;
	
	public AudioClip SFX_THUMP; 
	public AudioClip SFX_WHOOSH; 
	public AudioClip SFX_BOING; 
	public AudioClip SFX_WHISP; 
	public AudioClip SFX_CHECK; 
	
	private AudioSource mAudioSrc;
	
	// Use this for initialization
	void Awake()
	{
		SharedInstance = this; 
	}
	
	void Start () 
	{
		mAudioSrc = gameObject.GetComponent<AudioSource>(); 			
	}
	
	public void Play(AudioClip clip)
	{
		mAudioSrc.PlayOneShot(clip);
	}
	
}
