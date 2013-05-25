using UnityEngine;

public class scriptPlayer : MonoBehaviour {
	public Transform Target;
	public Transform Selected;
	public float RayDistance = 100.0f;
	
	void Update () {
        // Handle left click
		if (Input.GetMouseButtonDown (0)) {
			HandleLeftClick();
		}
        // Handle right click
		else if (Input.GetMouseButtonDown(1))
		{
            HandleRightClick();
		}
	}

    private void HandleLeftClick()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, RayDistance))
        {
            if (Selected)
            {
                Selected.GetComponent<scriptCanSelect>().Deselect();
            }

            Target = hit.transform;
            SelectedLeftClickTarget();
        }
    }
    private void HandleRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, RayDistance))
        {
            if (Selected)
            {
                SelectHarvestNode(hit);
            }
        }
    }
    private void SelectHarvestNode(RaycastHit hit)
    {
        var harvester = Selected.GetComponent<scriptHarvester>();

        // If it is, assign the selected object as the harvester node target
        if (harvester)
        {
            // Make sure the selected object is a node befoe assigning it as the target node
            var node = hit.transform.GetComponent<scriptNode>();

            if (node)
            {
                harvester.TargetNode = hit.transform;
            }
        }
    }
    private void SelectedLeftClickTarget()
    {
        var selectScript = Target.GetComponent<scriptCanSelect>();

        if (selectScript)
        {
            selectScript.Select();
            Selected = Target;
        }
        else
        {
            Selected = null;
        }
    }
}
