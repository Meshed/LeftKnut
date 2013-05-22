using UnityEngine;
using System.Collections;

public class scriptHarvester : MonoBehaviour {
	public bool selected = false;
	public int maxInventorySize = 20;
	public int currentInventorySize = 0;
	public int harvestRateInSeconds = 5;
	public float harvestRateCounter = 0.0f;
	public Color baseColor;
	public bool selecting = false;
	public Transform target;
	public float rayDistance = 100.0f;
	public float moveSpeed = 1.0f;
	public bool moving = false;
	public float minimumDistanceFromTarget = 2.0f;
	public float currentDistanceToTarget = 0.0f;
	public bool harvestingMode = false;
	public Transform targetNode;
	public Transform targetSilo;
	
	private Vector3 movingStartPosition;

	void Start () {
	}
	
	void Update () 
	{
		if(transform.GetComponent<scriptCanSelect>().selected)
		{
			GetTarget();
		}
		
		GetDistanceToTarget();
		moving = (currentDistanceToTarget > minimumDistanceFromTarget);
		
		if(moving)
		{
			transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
		}
		
		if(target && moving == false)
		{
			if(currentInventorySize < maxInventorySize)
			{
				harvestingMode = true;
				GatherResources();
			}
			else
			{
				target = targetSilo;
			}
		}
	}
	
	void GetTarget()
	{
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, rayDistance)) {
				target = hit.transform;
			}
		}
	}
	
	void GetDistanceToTarget()
	{
		if(target)
		{
			currentDistanceToTarget = Vector3.Distance(transform.position, target.position);
		}
	}
	
	void GatherResources()
	{
		harvestRateCounter += Time.deltaTime;
		
		if(harvestRateCounter >= harvestRateInSeconds)
		{
			print("Gather Resources");
			scriptNode script = target.GetComponent<scriptNode>();
			currentInventorySize += script.Harvest();
			ResetHarvestCounter();
		}
	}
	
	void ResetHarvestCounter()
	{
		harvestRateCounter = 0.0f;
	}
	
	void SetToSelectedColor()
	{
		renderer.material.color = Color.green;
	}
	void SetToStandardColor()
	{
		renderer.material.color = baseColor;
	}
}
