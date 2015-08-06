using UnityEngine;
using System.Collections;

public class PartPrefabs : MonoBehaviour
{
	[System.Serializable]
	public class PartPrefab
	{
		[HideInInspector]
		public string label;
		public Data.CircuitPartTypes type;
		public GameObject prefab;

	}

	public PartPrefab[] partPrefabs = new PartPrefab[0];


	public GameObject Get(Data.CircuitPartTypes type)
	{

		foreach (PartPrefab p in partPrefabs)
		{
			if (p.type == type)
			{
				return p.prefab;
			}
		}

		return null;
	}
}
