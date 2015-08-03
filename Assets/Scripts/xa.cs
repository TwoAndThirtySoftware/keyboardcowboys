using UnityEngine;
using System.Collections;

public class xa : MonoBehaviour
{
    public static Defines de;
    public static bool inCircuitMode = false;


    //A dev hack. 
    //Once this is multiplayer, this will be the circuit that is 
    //currently selected to edit when the circuit UI is pulled up.
    public static CircuitsData.Circuit playerCircuit = new CircuitsData.Circuit();  


    public struct Int2
    {
        public int x;
        public int y;

        public Int2(int xInput, int yInput)
        {
            x = xInput;
            y = yInput;
        }
    }

}
