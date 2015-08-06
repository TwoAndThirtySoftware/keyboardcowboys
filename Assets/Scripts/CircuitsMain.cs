using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitsMain : MonoBehaviour
{
	public static void CircuitsStart()
	{

	}

	public static void CircuitsUpdate()
	{
		foreach (KeyValuePair<int, Data.Circuit> KvP in Data.circuits) 
		{
			CircuitFuncs.UpdateCircuit(KvP.Value);
		}

	}
}
