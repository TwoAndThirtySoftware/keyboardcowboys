using UnityEngine;
using System.Collections;

public class PartFuncs : MonoBehaviour
{
	public static void CallFuncForPart(Data.Step step)
	{
		switch (step.part.type)
		{
			case Data.CircuitPartTypes.PowerGenerator1: PowerGenerator(step); return;
			case Data.CircuitPartTypes.Wire: Wire(step); return;
			case Data.CircuitPartTypes.Button: Button(step); return;
			case Data.CircuitPartTypes.BulletFabricator: BulletFabricator(step); return;
		}
	}

	public static void Wire(Data.Step step)
	{
		Debug.Log("Stepping through a Wire " + step.part.pos.x + ", " + step.part.pos.y);
		step.part.done = true;

		//Find all nearby parts
		Data.CircuitPart part = null;

		for (int i = 0; i < 4; i++)
		{
			Vector2 offsetPos = Vector2.zero;
			offsetPos.x = step.part.pos.x;
			offsetPos.y = step.part.pos.y;
			if (i == 0) { offsetPos.x++; }
			if (i == 1) { offsetPos.x--; }
			if (i == 2) { offsetPos.y++; }
			if (i == 3) { offsetPos.y--; }

			Data.CircuitPart cp = CircuitFuncs.PartByPos(offsetPos, step.circuit);
			if (cp != null)//If a part exists in this pos
			{
				if (!cp.done)//If this wire hasn't been used yet
				{
					//is this a wire, or a part?
					if (cp.type == Data.CircuitPartTypes.Wire)
					{
						//Ok, check using the offsetPos.
						//wires have inPorts on top of themselves. They *are* an inport.
						if (CircuitFuncs.CheckPosIs_InPower_OfPart(offsetPos, cp))
						{
							part = cp;
							break;
						}
					}
					else
					{
						//Ok, check using this part's pos
						//Parts have inPorts that stick out of themselves, so wire can be built on that tile.
						if (CircuitFuncs.CheckPosIs_InPower_OfPart(step.part.pos, cp))
						{
							part = cp;
							break;
						}
					}
					//Is this pos also on a inPower?
				}
			}
		}


		if (part != null)
		{
			//Debug.Log("Found connecting part at " + part.pos);
			//pass along to this other part
			Data.Step doNext = new Data.Step();
			doNext.circuit = step.circuit;
			doNext.part = part;
			doNext.power = step.power;
			CircuitFuncs.todo.Add(doNext);
		}
		else
		{
			//Debug.Log("Found no connecting part");
		}

	}


	public static void Button(Data.Step step)
	{
		Debug.Log("Stepping through a Button " + step.part.pos.x + ", " + step.part.pos.y);
		step.part.done = true;

		if (Input.GetKey(KeyCode.Q))
		{
			//Find connected parts
			Vector2 outPowerPos = new Vector2(step.part.pos.x + step.part.outPower[0].x, step.part.pos.y + step.part.outPower[0].y);
			Debug.DrawLine(new Vector3(15, 15, 2), new Vector3(outPowerPos.x, outPowerPos.y, 2), Color.red);

			Data.CircuitPart nextPart = CircuitFuncs.PartByPos(outPowerPos, step.circuit);
			if (nextPart != null)//Is there a part here?
			{
				if (nextPart != step.part && !nextPart.done)//If this isn't me & if this part doesn't have it's done flag set.
				{
					//Add this next step to the todo list
					Data.Step doNext = new Data.Step();
					doNext.circuit = step.circuit;
					doNext.part = nextPart;
					doNext.power = step.power;
					CircuitFuncs.todo.Insert(0, doNext);
				}
			}
		}
	}

	public static void BulletFabricator(Data.Step step)
	{
		Debug.Log("Stepping through a BulletFabricator " + step.part.pos.x + ", " + step.part.pos.y);
		step.part.done = true;

		Debug.Log("BANG!");

		GameObject go = (GameObject)Instantiate(xa.de.bulletPrefab, step.circuit.muzzlePoint.transform.position, step.circuit.muzzlePoint.transform.rotation);
	}

	public static void PowerGenerator(Data.Step step)
	{
		step.part.done = true;
		Debug.Log("Stepping through a PowerGenerator at " + step.part.pos.x + ", " + step.part.pos.y);


		//I'm a power generator, so add my powerGenerated stat to power
		if (step.power == null) { step.power = new Data.Power(); }//If there is no power, create a new one
		step.power.amount = (float)step.part.stats[Data.Stat.PowerGenerated];


		//Find connected parts
		Vector2 outPowerPos = new Vector2(step.part.pos.x + step.part.outPower[0].x, step.part.pos.y + step.part.outPower[0].y);
		Debug.DrawLine(new Vector3(15, 15, 2), new Vector3(outPowerPos.x, outPowerPos.y, 2), Color.red);

		//Debug.Log("Looking for connecting parts at " + outPowerPos.x + ", " + outPowerPos.y);
		//pass along via the outPort
		Data.CircuitPart nextPart = CircuitFuncs.PartByPos(outPowerPos, step.circuit);
		if (nextPart != null)//Is there a part here?
		{
			//Debug.Log("Found a " + nextPart.name + " at " + outPowerPos.x + ", " + outPowerPos.y + ", has a pos of " + nextPart.pos.x + ", " + nextPart.pos.y);

			//Debug.DrawLine(new Vector3(step.part.pos.x, step.part.pos.y, 2), new Vector3(nextPart.pos.x, nextPart.pos.y, 2), Color.red,10);

			if (nextPart != step.part && !nextPart.done)//If this isn't me & if this part doesn't have it's done flag set.
			{
				//Is this pos also on a inPower?
				if (CircuitFuncs.CheckPosIs_InPower_OfPart(outPowerPos, nextPart))
				{
					//Add this next step to the todo list
					Data.Step doNext = new Data.Step();
					doNext.circuit = step.circuit;
					doNext.part = nextPart;
					doNext.power = step.power;
					CircuitFuncs.todo.Insert(0, doNext);
				}
			}
		}
		else
		{
			//Debug.Log("Didn't find a connecting part at " + outPowerPos.x + ", " + outPowerPos.y);
		}


	}

}
