using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitsUI : MonoBehaviour
{
	public static CircuitsUI self;

	public static Data.CircuitPartTypes currentPaint;//The current type of 'paint' on the brush, ie, the type of circuit part imprinted on the brush.

	[HideInInspector]
	public GameObject transparentPart;


	void Awake()
	{
		self = this;
	}

	void Start()
	{
		InitCircuitUI();
		SetCircuitUIMode(false);//Dev hack
		SetCircuitUIMode(true);//Dev hack
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			SetCircuitUIMode(!xa.inCircuitMode);
		}

		if (xa.inCircuitMode)
		{
			UpdateCircuitUI();
		}
	}

	public void UpdateCircuitUI()
	{
		SnapMenu();

		if (Input.GetMouseButtonDown(0)) { HandleClickDown(); }
		if (Input.GetMouseButton(0)) { HandleClickHeld(); }
		if (Input.GetMouseButtonUp(0)) { HandleClickUp(); }

		if (Input.GetMouseButtonDown(1)) { HandleRightClickDown(); }
		if (Input.GetMouseButton(1)) { HandleRightClickHeld(); }
		if (Input.GetMouseButtonUp(1)) { HandleRightClickUp(); }

	}

    public void SetCircuitUIMode(bool setModeTo)
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

    public void InitCircuitUI()
    {
        float verSpacing = 0.5f;
        Vector3 pos = new Vector3(0.1f, -0.3f, 1);

        //Populate a menu of circuit part types
		for (int i = 1; i < (int)Data.CircuitPartTypes.End;i++)
		{
			Data.CircuitPart cp = Data.GetBlankPartOfType((Data.CircuitPartTypes)i);
			GameObject go = (GameObject)Instantiate(xa.de.circuitUI_GenericPartButtonPrefab, Vector3.zero, xa.de.circuitUI_GenericPartButtonPrefab.transform.rotation);

			TextMesh textMesh = go.GetComponentInChildren<TextMesh>();
			if (textMesh != null)
			{
				textMesh.text = cp.name;
			}
			else { Debug.Log("textMesh is null"); }

			ButtonInfo buttonInfoScript = go.GetComponentInChildren<ButtonInfo>();
			if (buttonInfoScript != null)
			{
				buttonInfoScript.partType = cp.type;
			}
			else { Debug.Log("buttonInfoScript is null"); }


			go.transform.parent = xa.de.circuitUI_PartList.transform;
			go.transform.localPosition = pos;
			pos.y -= verSpacing;

		}
    }

	public void HandleRightClickDown()
	{
		LayerMask mask = 1 << 11;//circuit part hit box mask
		Ray ray = new Ray();
		RaycastHit hit;
		ray = xa.de.circuitCamera.ScreenPointToRay(Input.mousePosition);

		//Check for part hitboxes
		if (Physics.Raycast(ray, out hit, 100, mask))
		{
			GameObject go = hit.collider.gameObject.transform.parent.gameObject;
			InfoScript infoScript = go.GetComponent<InfoScript>();
			if (infoScript != null)
			{
				Destroy(infoScript.gameObject);
			}
			return;
		}

	}
	public void HandleRightClickHeld()
	{
	}
	public void HandleRightClickUp()
	{
	}

    public void HandleClickDown()
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

		if (currentPaint != Data.CircuitPartTypes.None)
		{
			//Start placing a part
			Vector3 pos = ray.GetPoint(1);

			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);

			Vector2 gridPos = pos;
			//   CircuitFuncs.CreatePart(gridPos, currentPaint);

			transparentPart = (GameObject)Instantiate(Data.GetBlankPartOfType(currentPaint).prefab, pos, Quaternion.Euler(0,0,0));
			Renderer partRenderer = transparentPart.GetComponentInChildren<Renderer>();
			if (partRenderer != null)
			{
				partRenderer.material.color = new Color(partRenderer.material.color[0], partRenderer.material.color[1], partRenderer.material.color[2], 0.5f);

			}
		}
    }

    public void HandleClickHeld()
    {
		if (currentPaint != Data.CircuitPartTypes.None)
		{
			if (transparentPart)
			{
				//Place a part
				LayerMask mask = 1 << 9;//menu hit box mask
				Ray ray = new Ray();
				ray = xa.de.circuitCamera.ScreenPointToRay(Input.mousePosition);
				Vector3 pos = ray.GetPoint(1);
				pos.x = Mathf.RoundToInt(pos.x);
				pos.y = Mathf.RoundToInt(pos.y);
				transparentPart.transform.position = pos;
			}
		}
    }

    public void HandleClickUp()
	{
		if (currentPaint != Data.CircuitPartTypes.None)
		{
			if (transparentPart)
			{
				//Place a part
				LayerMask mask = 1 << 9;//menu hit box mask
				Ray ray = new Ray();
				RaycastHit hit;
				ray = xa.de.circuitCamera.ScreenPointToRay(Input.mousePosition);
				Destroy(transparentPart);
				//Check for menu hitboxes
				if (Physics.Raycast(ray, out hit, 100, mask))
				{
					return;
				}

				Vector2 gridPos = ray.GetPoint(1);
				gridPos.x = Mathf.RoundToInt(gridPos.x);
				gridPos.y = Mathf.RoundToInt(gridPos.y);
				CircuitFuncs.CreatePart(gridPos, currentPaint, xa.playerCircuit);
			}
		}
    }

    public void SnapMenu()
    {
        //Find corners
        Vector3 topLeftCorner = FindCorner(new Vector3(0, Screen.height, 0));
        Vector3 center = FindCorner(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        Vector3 bottomRightCorner = FindCorner(new Vector3(Screen.width, 0, 0));

        //snap the menu
        xa.de.circuitUI_PartList.transform.position = topLeftCorner;

        //Snap the thumb X
    }

    public Vector3 FindCorner(Vector3 input)
    {
        Ray ray = new Ray();
        ray = xa.de.circuitCamera.ScreenPointToRay(input);
        return ray.GetPoint(1);
    }
}
