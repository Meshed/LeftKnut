using UnityEngine;
using System.Collections;

public class TurretControl : MonoBehaviour {
	
	public Transform _GunForward;
	public Transform[] _Muzzles;
	public GameObject _Bullet;
	public float _FireRate = 2;
	public float _AimError = 1f;
	
	GameObject _currentTarget;
	float _timeSinceLastFiring;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_currentTarget == null)
		{
			_currentTarget = FindClosestEnemy();
		}
		
		if(_currentTarget != null)
		{
			
			AimTowardsTarget(_currentTarget);
			
			_timeSinceLastFiring = _timeSinceLastFiring + Time.deltaTime;
		
			if(_timeSinceLastFiring > _FireRate)
			{
				_timeSinceLastFiring = 0;
				
				foreach(Transform muzzel in _Muzzles)
				{
					Instantiate(_Bullet, muzzel.position, muzzel.rotation);
				}
			}
		}
	}
	
	private void AimTowardsTarget(GameObject currentTarget)
	{
		var targetDir = _currentTarget.transform.position - _GunForward.position;
		targetDir = new Vector3(targetDir.x + GetAimError(),targetDir.y + GetAimError(),targetDir.z + GetAimError());
		
		var desiredRotation = Quaternion.LookRotation(targetDir);
		
		var step = 1 * Time.deltaTime;
		_GunForward.rotation =  Quaternion.Lerp(_GunForward.rotation, desiredRotation, step);
	}
	
	private float GetAimError()
	{
		return Random.Range(-_AimError,_AimError);
	}
	
	private GameObject FindClosestEnemy(){
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag("enemy");
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
		
		return closest;
	}
	
	
}
