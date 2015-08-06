using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	public GameObject muzzlePoint;

	void Start()
	{
		//Create a circuit for me.
		xa.playerCircuit.muzzlePoint = muzzlePoint;
	}

	void Update()
	{

	}
}
