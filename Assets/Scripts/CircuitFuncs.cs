using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitFuncs : MonoBehaviour
{
	public static List<Data.Step> todo = new List<Data.Step>();

	public static void UpdateCircuit(Data.Circuit circuit)
	{
		//Updates this entire circuit.
		Debug.Log("Updating Circuit " + circuit.id);

		string str = "PartsByPos:";
		foreach (Data.CircuitPart cp in circuit.parts)
		{
			str += "(" + cp.pos.x + "," + cp.pos.y + "), ";
		}
		Debug.Log(str);

		todo = new List<Data.Step>();//Reset & wipe the todo list

		//set all part's done flags to false
		foreach (Data.CircuitPart cp in circuit.parts)
		{
			cp.done = false;
		}

		//Add all powerSources to the todo list

		//Adding powersources by priority is broken and I can't be arsed to write it correctly right now.
		//Add powerSources in order of priority number. //ALL POWERSOURCES MUST HAVE DIFFERENT PRIORITIES
		/*for (int i = 0; i < 100; i++)//This means a max of 10000 power sources
		{
			//Go from each power source in turn
			int highestPriority = -1;
			CircuitPart part = null;
			foreach (CircuitPart cp in circuit.parts)
			{
				if (cp.stats.ContainsKey(Stat.IsPowerSource))
				{
					if (cp.priority > highestPriority)
					{
						highestPriority = cp.priority;
						part = cp;
					}
				}
			}
			if (part != null)
			{
				Step step = new Step();
				step.part = part;
				step.circuit = circuit;
				todo.Add(step);
			}
			else { break; }
		}*/

		foreach (Data.CircuitPart cp in circuit.parts)//Add all powersources ignoring priority, (temporary dev hack)
		{
			if (cp.stats.ContainsKey(Data.Stat.IsPowerSource))
			{
				Data.Step step = new Data.Step();
				step.part = cp;
				step.circuit = circuit;
				todo.Add(step);
			}
		}

		Vector3 pos1 = Vector3.zero;
		Vector3 pos2 = Vector3.zero;

		if (todo.Count > 0)
		{
			int cap = 0;//To prevent inf loops
			while (cap < 100 && todo.Count > 0)
			{

				//Take the top step from the stack (todo list)
				Data.Step doThis = todo[0];
				todo.RemoveAt(0);

				Debug.Log("Taking " + doThis.part.name + " from the stack,");

				//Handle part on top of todo list (the one at the 0 index)
				PartFuncs.CallFuncForPart(doThis);//This function should add to the todo list if the part is connected to anyone

				//Debug lines
				pos2 = pos1;
				pos1 = new Vector3(doThis.part.pos.x, doThis.part.pos.y);
				if (pos2 != Vector3.zero) { Debug.DrawLine(pos1, pos2, Color.green); }
				


				cap++;
			}
			if (cap >= 100) { Debug.Log("Hit max loops! In Circuit " + circuit.id); }


		}
		Debug.Log("Updated Circuit " + circuit.id + ". Had " + circuit.parts.Count + " parts.");
	}

	public static Data.CircuitPart PartByPos(Vector2 pos, Data.Circuit circuit)
	{
		if (circuit.partsByPos.ContainsKey(pos))
		{
			return circuit.partsByPos[pos];
		}
		else
		{
			return null;
		}
		/*
		foreach (CircuitPart cp in circuit.parts)
		{
			if (cp.pos.x == pos.x && cp.pos.y == pos.y)
			{
				return cp;
			}
		}
		return null;
		 */
	}

	public static bool ContainsPartByPos(Vector2 pos, Data.Circuit circuit)
	{
		if (circuit.partsByPos.ContainsKey(pos)) { return true; }
		return false;
		/*
		foreach (CircuitPart cp in circuit.parts)
		{
			if (cp.pos.x == pos.x && cp.pos.y == pos.y)
			{
				return true;
			}
		}
		return false;*/
	}

	public static void CreatePart(Vector2 gridPos, Data.CircuitPartTypes partType, Data.Circuit circuit)
	{
		//check that no other part exists on this gridPos
		if (ContainsPartByPos(gridPos, xa.playerCircuit)) { Debug.Log("Failed to create part. Other part already existed on that space."); return; }


		Data.CircuitPart cp = Data.GetBlankPartOfType(partType);
		cp.pos = gridPos;
		GameObject go = (GameObject)Instantiate(cp.prefab, new Vector3(gridPos.x, gridPos.y, 10), Quaternion.Euler(0,0,0));

		circuit.parts.Add(cp);
		
		circuit.partsByPos.Add(cp.pos, cp);

		if (cp.type == Data.CircuitPartTypes.PowerGenerator1 ||
			cp.type == Data.CircuitPartTypes.BulletFabricator ||
			cp.type == Data.CircuitPartTypes.Button)
		{

			circuit.partsByPos.Add(new Vector2(cp.pos.x - 1, cp.pos.y - 1), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x - 1, cp.pos.y), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x - 1, cp.pos.y + 1), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x, cp.pos.y - 1), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x, cp.pos.y + 1), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x + 1, cp.pos.y - 1), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x + 1, cp.pos.y), cp);
			circuit.partsByPos.Add(new Vector2(cp.pos.x + 1, cp.pos.y + 1), cp);

		}


		Debug.Log("Created " + cp.type + " at " + gridPos);

	}

	public static bool CheckPosIs_InPower_OfPart(Vector2 pos, Data.CircuitPart cp)
	{
		foreach (Vector2 v in cp.inPower)
		{
			if (pos == (cp.pos + v))
			{
				return true;
			}
		}
		return false;
	}

}
