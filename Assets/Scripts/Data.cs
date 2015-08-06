using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{

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

	public enum Stat
	{
		None,
		IsPowerSource,
		PowerGenerated,
		PowerStored,
		End
	}

	public static Dictionary<int, Circuit> circuits = new Dictionary<int, Circuit>();//All circuits in the game

	public class Circuit
	{
		public int id = -1;//id is not a place in a list, like I normally would do it.
		public List<CircuitPart> parts = new List<CircuitPart>();//The parts contained in this circuit
		public Dictionary<Vector2, CircuitPart> partsByPos = new Dictionary<Vector2, CircuitPart>();//The parts contained in this circuit
		public GameObject muzzlePoint;
	}

	public class CircuitPart
	{
		public string name;
		public Data.CircuitPartTypes type;
		public GameObject prefab;
		public int priority = 0;
		public bool done;//This is set to done when the part should be ignored for the rest of the update step. Currently used by wires to prevent inf. looping.
		public Vector2 pos;
		public Vector2[] inPower = new Vector2[0];//Create zero-length arrays. Part will overwrite some of these with longer arrays.
		public Vector2[] outPower = new Vector2[0];
		public Dictionary<Data.Stat, object> stats = new Dictionary<Data.Stat, object>();
		public int width = 1;//must be an odd number
		public int height = 1;//must be an odd number
	}

	public class Power
	{
		public float amount = 0;
	}

	public class Step //The class that is passed between parts as they step through the circuit
	{
		public Circuit circuit;
		public CircuitPart part;
		public Power power;
	}

	public static CircuitPart GetBlankPartOfType(CircuitPartTypes type)
	{
		CircuitPart part = new CircuitPart();
		switch (type)
		{
			case CircuitPartTypes.PowerGenerator1:
				part.name = "Power Generator 1";
				part.type = CircuitPartTypes.PowerGenerator1;
				part.prefab = xa.pp.Get(CircuitPartTypes.PowerGenerator1);
				part.outPower = new Vector2[1];
				part.outPower[0] = new Vector2(0, 2);
				part.stats.Add(Stat.IsPowerSource, (object)true);
				part.stats.Add(Stat.PowerGenerated, (object)10f);
				break;

			case CircuitPartTypes.Button:
				part.name = "Button";
				part.type = CircuitPartTypes.Button;
				part.prefab = xa.pp.Get(CircuitPartTypes.Button);
				part.inPower = new Vector2[1];
				part.inPower[0] = new Vector2(0, -2);
				part.outPower = new Vector2[1];
				part.outPower[0] = new Vector2(0, 2);
				break;

			case CircuitPartTypes.BulletFabricator:
				part.name = "Bullet Fabricator";
				part.type = CircuitPartTypes.BulletFabricator;
				part.prefab = xa.pp.Get(CircuitPartTypes.BulletFabricator);
				part.inPower = new Vector2[1];
				part.inPower[0] = new Vector2(0, -2);
				break;

			case CircuitPartTypes.Battery:
				part.name = "Battery";
				part.type = CircuitPartTypes.Battery;
				part.prefab = xa.pp.Get(CircuitPartTypes.Battery);
				part.stats.Add(Stat.IsPowerSource, (object)true);
				part.stats.Add(Stat.PowerStored, (object)0f);
				break;
			case CircuitPartTypes.Wire:
				part.name = "Wire";
				part.type = CircuitPartTypes.Wire;
				part.prefab = xa.pp.Get(CircuitPartTypes.Wire);
				part.inPower = new Vector2[1];
				part.inPower[0] = new Vector2(0, 0);
				break;


		}
		return part;
	}

}
