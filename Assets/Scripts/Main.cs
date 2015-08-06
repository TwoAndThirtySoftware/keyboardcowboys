using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    void Awake()
	{
		xa.de = this.gameObject.GetComponent<Defines>();
		xa.pp = this.gameObject.GetComponent<PartPrefabs>();

    }

    void Start()
    {
		CircuitsMain.CircuitsStart();

		xa.playerCircuit = new Data.Circuit();
		xa.playerCircuit.id = xa.uniqueCircuitIds;
		xa.uniqueCircuitIds++;
		Data.circuits.Add(xa.playerCircuit.id, xa.playerCircuit);
	}

    void Update()
	{

		CircuitsMain.CircuitsUpdate();
    }
}
