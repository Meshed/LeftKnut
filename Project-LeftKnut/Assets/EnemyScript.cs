using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public GameObject ClosestTarget;
    public float MoveSpeed = 1.0f;
    public int Health = 100;
    public bool IsAlive = true;
    public float MinimumDistanceFromTarget = 14.0f;
    public int DamageDealtPerAttack = 50;
    public float AttackDelay = 2f;

    private float _currentDistanceToTarget;
    private float _timeSinceLastAttack = 2f;

	// Use this for initialization
	void Start ()
	{
        GetTarget();
	}

    void Update()
    {
        if (IsAlive)
        {
            if (ClosestTarget)
            {

                GetDistanceToTarget();

                if (_currentDistanceToTarget > MinimumDistanceFromTarget)
                    MoveToTarget();
                else
                    AttackTarget();
            }
            else
            {
                GetTarget();
            }
        }
        else
        {
            Destroy(gameObject);
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");

            if (playerGameObject)
            {
                var script = playerGameObject.GetComponent<scriptPlayer>();

                if (script)
                {
                    script.IncreaseScore();
                }
            }
        }
    }
	
	void OnTriggerEnter(Collider collision) 
	{
        TakeDamage(2);
        Destroy(collision);
	}
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            IsAlive = false;
        }
    }
    
    private void AttackTarget()
    {
        if (ClosestTarget)
        {
            var script = ClosestTarget.GetComponent<TakesDamage>();

            if (script)
            {
                _timeSinceLastAttack += Time.deltaTime;

                if (_timeSinceLastAttack > AttackDelay)
                {
                    script.TakeDamage(DamageDealtPerAttack);
                    _timeSinceLastAttack = 0f;
                }
            }
        }
    }
    private void MoveToTarget()
    {
        if (ClosestTarget)
        {
                transform.position = Vector3.Lerp(transform.position, ClosestTarget.transform.position, MoveSpeed * Time.deltaTime);
        }
    }
    private void GetDistanceToTarget()
    {
        if (ClosestTarget)
        {
            _currentDistanceToTarget = Vector3.Distance(transform.position, ClosestTarget.transform.position);
        }
    }
    private void GetTarget()
    {
        SetClosestResourceTarget();
        SetClosestSiloTarget();
        SetClosestTurretTarget();
        SetClosestHarvesterTarget();
    }
    private void SetClosestResourceTarget()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("resource");
        SetClosestTarget(gameObjects);
    }
    private void SetClosestSiloTarget()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("silo");
        SetClosestTarget(gameObjects);
    }
    private void SetClosestTurretTarget()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("turret");
        SetClosestTarget(gameObjects);
    }
    private void SetClosestHarvesterTarget()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("harvester");
        SetClosestTarget(gameObjects);
    }
    private void SetClosestTarget(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject target in gameObjects)
        {
            if (ClosestTarget)
            {
                ClosestTarget = GetGameObjectDistance(ClosestTarget) < GetGameObjectDistance(target)
                                    ? ClosestTarget
                                    : target;
            }
            else
            {
                ClosestTarget = target;
            }
        }        
    }
    private float GetGameObjectDistance(GameObject target)
    {
        Vector3 diff = target.transform.position - transform.position;
        float curDistance = diff.sqrMagnitude;

        return curDistance;
    }
}
