using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotation : MonoBehaviour {


	public float x=27f;

	
	void Update () 
	{
		//Rotate the shell relative to itself by x degrees
		transform.Rotate (x * Time.deltaTime ,0f, 0f,relativeTo:Space.Self);
	}
}
