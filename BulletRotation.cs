using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotation : MonoBehaviour {

    /// <summary>
    /// Degree.
    /// </summary>
	public float x=27f;

    /// <summary>
    /// Each frame rotate the shell relative to itself by x degrees.
    /// </summary>
    void Update () 
	{
		
		transform.Rotate (x * Time.deltaTime ,0f, 0f,relativeTo:Space.Self);
	}
}
