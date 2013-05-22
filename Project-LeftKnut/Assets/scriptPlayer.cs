using UnityEngine;
using System.Collections;

public class scriptPlayer : MonoBehaviour {
	public Transform target;
	public Transform selected;
	public float rayDistance = 100.0f;
	
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, rayDistance)) {
				if(selected)
				{
					selected.GetComponent<scriptCanSelect>().Deselect();
				}
				
				target = hit.transform;
				
				scriptCanSelect selectScript = target.GetComponent<scriptCanSelect>();
				if(selectScript)
				{
					selectScript.Select();
					selected = target;
				}
				else
				{
					selected = null;
				}
			}
		}
	}
}
