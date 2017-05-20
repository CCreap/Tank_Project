using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Complete
{
public class HealthKit : MonoBehaviour
	{
		public float Speed = 30f; //Speed of floating
		public float height = 5f; //Hight of floating

		//Store 3 valuse of Vector3, to save object's position
		float x;
		float y;
		float z;

		void Start () 
		{
			//Getting values from object's current position
			 x = transform.position.x;
			 y = transform.position.y;
			 z = transform.position.z;
		}
		
		void Update () 
		{
			//Turn object around itself by 30 degrees per second
			transform.Rotate (new Vector2 (0f, 30f) * Time.deltaTime);
			//Change the position in y coordinate to make object float in Sin function
			transform.position = new Vector3(x, y + (Mathf.Sin (Time.time) * height),z);
		}
	}
}