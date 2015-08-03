using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitsUI : MonoBehaviour
{
    public static CircuitsData.CircuitPartTypes currentPaint;//The current type of 'paint' on the brush, ie, the type of circuit part imprinted on the brush.

    public static void SetCircuitUIMode(bool setModeTo)
    {
        if (setModeTo)
        {
            //Turn the circuit UI on
            xa.inCircuitMode = true;
            xa.de.circuitCamera.gameObject.SetActive(xa.inCircuitMode);
        }
        else
        {
            //Turn the circuit UI off
            xa.inCircuitMode = false;
            xa.de.circuitCamera.gameObject.SetActive(xa.inCircuitMode);
        }
    }

    public static void InitCircuitUI()
    {
        float verSpacing = 0.5f;
        Vector3 pos = new Vector3(0.1f, -0.3f, 1);

        //Populate a menu of circuit part types
        foreach (KeyValuePair<CircuitsData.CircuitPartTypes, CircuitsData.CircuitPart> KvP in CircuitsData.circuitPartTemplates)
        {
            GameObject go = (GameObject)Instantiate(xa.de.circuitUI_GenericPartButtonPrefab, Vector3.zero, xa.de.circuitUI_GenericPartButtonPrefab.transform.rotation);
            go.transform.parent = xa.de.circuitUI_PartList.transform;
            go.transform.localPosition = pos;
            pos.y -= verSpacing;

            TextMesh textMesh = go.GetComponentInChildren<TextMesh>();
            textMesh.text = KvP.Value.name;

            ButtonInfo buttonInfoScript = go.GetComponentInChildren<ButtonInfo>();
            buttonInfoScript.partType = KvP.Value.type;
        }
    }

    public static void UpdateCircuitUI()
    {
        SnapMenu();

        if (Input.GetMouseButtonDown(0)) { HandleClickDown(); }
        if (Input.GetMouseButton(0)) { HandleClickHeld(); }
        if (Input.GetMouseButtonUp(0)) { HandleClickUp(); }

    }

    public static void HandleClickDown()
    {
        LayerMask mask = 1 << 9;//menu hit box mask
        Ray ray = new Ray();
        RaycastHit hit;
        ray = xa.de.circuitCamera.ScreenPointToRay(Input.mousePosition);


        //Check for menu hitboxes
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            ButtonInfo script = hit.collider.gameObject.GetComponent<ButtonInfo>();
            if (script)
            {
                currentPaint = script.partType;
            }
            return;
        }

        //Place a part
        xa.Int2 gridPos = CircuitFuncs.PosToGrid(ray.GetPoint(1));
        CircuitFuncs.CreatePart(gridPos, currentPaint);
    }

    public static void HandleClickHeld()
    {

    }

    public static void HandleClickUp()
    {

    }

    public static void SnapMenu()
    {
        //Find corners
        Vector3 topLeftCorner = FindCorner(new Vector3(0, Screen.height, 0));
        Vector3 center = FindCorner(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        Vector3 bottomRightCorner = FindCorner(new Vector3(Screen.width, 0, 0));

        //snap the menu
        xa.de.circuitUI_PartList.transform.position = topLeftCorner;

        //Snap the thumb X
    }

    public static Vector3 FindCorner(Vector3 input)
    {
        Ray ray = new Ray();
        ray = xa.de.circuitCamera.ScreenPointToRay(input);
        return ray.GetPoint(1);
    }
}
