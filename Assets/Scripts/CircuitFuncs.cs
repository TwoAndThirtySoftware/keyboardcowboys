using UnityEngine;
using System.Collections;

public class CircuitFuncs : MonoBehaviour
{
    public static void CreatePart(xa.Int2 gridPos, CircuitsData.CircuitPartTypes partType)
    {
        CircuitsData.CircuitPart circuitPart = new CircuitsData.CircuitPart();
        circuitPart = CircuitsData.circuitPartTemplates[partType];

        GameObject go = (GameObject)Instantiate(circuitPart.prefab, new Vector3(gridPos.x, gridPos.y, 10), circuitPart.prefab.transform.rotation);

        xa.playerCircuit.parts.Add(circuitPart);
    }

    public static xa.Int2 PosToGrid(Vector2 pos)
    {
        xa.Int2 int2 = new xa.Int2(0,0);

        int2.x = Mathf.RoundToInt(pos.x);
        int2.y = Mathf.RoundToInt(pos.y);

        return int2;
    }
}
