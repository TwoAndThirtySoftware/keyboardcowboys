using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
	float spd = 15;
	void Start()
	{

	}

	void Update()
	{

		//Move forward
		transform.Translate(new Vector3(0, 0, spd * Time.deltaTime));
	}
}
