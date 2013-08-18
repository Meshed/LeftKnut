using UnityEngine;
using System.Collections;

public class TurretControl : MonoBehaviour {
	
	public Transform _GunForward;
	public Transform[] _Muzzles;
	public GameObject _Bullet;
	public float FireRate = 2;
	public float _AimError = 1f;
    public float AimSpeed = 3.0f;
	public GameObject CurrentTarget;
    public AudioClip TurretFire;

	float _timeSinceLastFiring;
	private bool _isPlaced = false;
	
	void Start () {
		
	}
	void Update () {
	    if (!_isPlaced)
	    {
	        return;
	    }

		if(CurrentTarget == null)
		{
			CurrentTarget = FindClosestEnemy();
		}
		
		if(CurrentTarget != null)
		{
			
			AimTowardsTarget();
			
			_timeSinceLastFiring = _timeSinceLastFiring + Time.deltaTime;
		
			if(_timeSinceLastFiring > FireRate)
			{
			    FireTurret();
			}
		}
	}
    public void PlaceTurret()
    {
        _isPlaced = true;
    }

    private void FireTurret()
    {
        _timeSinceLastFiring = 0;

        foreach (Transform muzzel in _Muzzles)
        {
            Instantiate(_Bullet, muzzel.position, muzzel.rotation);
            audio.PlayOneShot(TurretFire);
        }
    }
	private void AimTowardsTarget()
	{
		var targetDir = CurrentTarget.transform.position - _GunForward.position;
		targetDir = new Vector3(targetDir.x + GetAimError(),targetDir.y + GetAimError(),targetDir.z + GetAimError());
		
		var desiredRotation = Quaternion.LookRotation(targetDir);
		
		_GunForward.rotation =  Quaternion.Lerp(_GunForward.rotation, desiredRotation, AimSpeed * Time.deltaTime);
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
