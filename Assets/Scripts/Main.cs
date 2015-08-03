using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    void Awake()
    {
        xa.de = this.gameObject.GetComponent<Defines>();

    }

    void Start()
    {
        CircuitsData.AddAllCircuitParts();//Init all CircuitParts
        CircuitsUI.InitCircuitUI();

        CircuitsUI.SetCircuitUIMode(false);//Dev hack
        CircuitsUI.SetCircuitUIMode(true);//Dev hack
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CircuitsUI.SetCircuitUIMode(!xa.inCircuitMode);
        }


        if (xa.inCircuitMode)
        {
            CircuitsUI.UpdateCircuitUI();
        }
    }
}
