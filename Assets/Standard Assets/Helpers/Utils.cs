using UnityEngine;
using System.Collections;

static public class Utils {

	// Use this for initialization
	static public float MapRange(float srcVal, float srcMin, float srcMax, float tgtMin, float tgtMax)
	{
		return Normalize (srcVal,srcMin, srcMax) * (tgtMax - tgtMin) + tgtMin;		
	}
	
	static public float Normalize(float srcVal, float low,  float high)
	{
		return srcVal/(high + low); 
	}
}
