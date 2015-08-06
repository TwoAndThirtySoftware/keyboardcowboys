using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UpdatePartPrefabLabels : MonoBehaviour
{
	PartPrefabs script;

	void Update()
	{
		if (Application.isPlaying) return;//Don't actually run this script when the game is playing

		if (script == null)
		{
			script = this.gameObject.GetComponent<PartPrefabs>();
		}
		else
		{
			foreach (PartPrefabs.PartPrefab p in script.partPrefabs)
			{
				p.label = "" + p.type;
			}
		}
	}
}
