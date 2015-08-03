using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitsData : MonoBehaviour
{
    public static Dictionary<int, Circuit> circuits = new Dictionary<int, Circuit>();//All circuits in the game
    public static Dictionary<CircuitPartTypes, CircuitPart> circuitPartTemplates = new Dictionary<CircuitPartTypes, CircuitPart>();//A dictionary of all circuit part prototypes/templates/static examples

    public class Circuit
    {
        public HashSet<CircuitPart> parts = new HashSet<CircuitPart>();//The parts contained in this circuit

    }

    public class CircuitPart
    {
        public string name;
        public CircuitPartTypes type;
        public GameObject prefab;

    }

    //All circuit parts need to be added to this enum
    public enum CircuitPartTypes
    {
        None,
        PowerGenerator1,
        Button,
        BulletFabricator,
        Battery,
        Wire,

        End
    }

    public static void AddAllCircuitParts()
    {
        CircuitPart part;

        //Power Generator 1
        part = new CircuitPart();
        part.name = "Power Generator 1";
        part.type = CircuitPartTypes.PowerGenerator1;
        part.prefab = xa.de.redCube_PartPrefab;
        circuitPartTemplates.Add(part.type, part);//Add to global list

        //Button
        part = new CircuitPart();
        part.name = "Button";
        part.type = CircuitPartTypes.Button;
        part.prefab = xa.de.redCube_PartPrefab;
        circuitPartTemplates.Add(part.type, part);//Add to global list

        //Bullet Fabricator
        part = new CircuitPart();
        part.name = "Bullet Fabricator";
        part.type = CircuitPartTypes.BulletFabricator;
        part.prefab = xa.de.redCube_PartPrefab;
        circuitPartTemplates.Add(part.type, part);//Add to global list

        //Battery
        part = new CircuitPart();
        part.name = "Battery";
        part.type = CircuitPartTypes.Battery;
        part.prefab = xa.de.redCube_PartPrefab;
        circuitPartTemplates.Add(part.type, part);//Add to global list

        //Wire
        part = new CircuitPart();
        part.name = "Wire";
        part.type = CircuitPartTypes.Wire;
        part.prefab = xa.de.wire_Prefab;
        circuitPartTemplates.Add(part.type, part);//Add to global list



    }

}
