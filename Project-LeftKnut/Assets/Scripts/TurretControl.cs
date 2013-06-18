using UnityEngine;
using System.Collections;

public class TurretControl : MonoBehaviour {
	
	GameObject _currentTarget;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_currentTarget = FindClosestEnemy();
		
		var turret = transform.Find("TurretForward");
		var targetDir = _currentTarget.transform.position - turret.position;
		
		var step = 1 * Time.deltaTime;
		var direction =  Vector3.RotateTowards(turret.forward, targetDir, step, 0f);

	
	    Debug.DrawRay(turret.position, direction, Color.red);
		
		turret.rotation = Quaternion.LookRotation(direction);
		
		
		
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
