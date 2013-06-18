using UnityEngine;

public class scriptHarvester : MonoBehaviour {
    public int HarvestRateInSeconds = 5;
    public float MinimumDistanceFromTarget = 7.0f;
    public float MoveSpeed = 1.0f;
    public Transform TargetNode;
    public int MaxInventorySize = 20;
    public float MaxHarvestDistance = 7.0f;

    private float _harvestRateCounter;
    private int _currentInventorySize;
    private float _currentDistanceToTarget;
    private Transform _target;
	
	
	void Update () 
	{
	    GetTarget();
		GetDistanceToTarget();
		MoveToTarget();

	    if (_target == TargetNode)
	    {
	        GatherResources();
	    }
	    else
	    {
	        DepositResources();
	    }
	}

    private void GetTarget()
    {
        _target = _currentInventorySize < MaxInventorySize ? TargetNode : FindClosestSilo();
    }
    private void GetDistanceToTarget()
	{
		if(_target)
		{
			_currentDistanceToTarget = Vector3.Distance(transform.position, _target.position);
		}
	}
	private void MoveToTarget()
	{
		if(_currentDistanceToTarget > MinimumDistanceFromTarget)
		{
			transform.position = Vector3.Lerp(transform.position, _target.position, MoveSpeed * Time.deltaTime);
		}
	}
	private void GatherResources()
	{
	    if (_currentDistanceToTarget <= MaxHarvestDistance)
	    {
		    _harvestRateCounter += Time.deltaTime;
		
		    if(_harvestRateCounter >= HarvestRateInSeconds)
		    {
		        HarvestNode();
		    }
        }
    }
    private void HarvestNode()
    {
        if (_target)
        {
            var script = _target.GetComponent<scriptNode>();

            if (script)
            {
                _currentInventorySize += script.Harvest();
                if (_currentInventorySize >= MaxInventorySize)
                {
                    _currentInventorySize = MaxInventorySize;
                }

                ResetHarvestCounter();
            }            
        }
    }
    private void ResetHarvestCounter()
	{
		_harvestRateCounter = 0.0f;
	}
    private void DepositResources()
    {
        if (_currentDistanceToTarget <= MaxHarvestDistance)
        {
            _harvestRateCounter += Time.deltaTime;

            if (_harvestRateCounter >= HarvestRateInSeconds)
            {
                DepositToSilo();
            }
        }
    }
    private void DepositToSilo()
    {
        var silo = _target.GetComponent<scriptSilo>();

        if (silo)
        {
            if (silo.Deposit(_currentInventorySize))
            {
                _currentInventorySize = 0;
            }
        }
    }
	
	private Transform FindClosestSilo(){
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag("silo");
		GameObject closest = null;
		var distance = Mathf.Infinity;
		var postion = transform.position;
		
		foreach(var go in gos){
			var diff = go.transform.position - postion;
			var curDistance = diff.sqrMagnitude;
			
			if(curDistance < distance){
				closest = go;
				distance = curDistance;
			}
		}
		
		return closest.transform;
	}
}