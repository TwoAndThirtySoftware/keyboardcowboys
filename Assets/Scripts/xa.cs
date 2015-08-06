using UnityEngine;
using System.Collections;

public class xa : MonoBehaviour
{
	public static Defines de;
	public static PartPrefabs pp;
    public static bool inCircuitMode = false;


    //A dev hack. 
    //Once this is multiplayer, this will be the circuit that is 
    //currently selected to edit when the circuit UI is pulled up.
	public static Data.Circuit playerCircuit;

	public static int uniqueCircuitIds = 0;


}
