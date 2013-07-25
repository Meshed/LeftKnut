using UnityEngine;
using System.Collections;

public class Gun_Projectile : MonoBehaviour {

	public float _Speed = 10;
	public float _Range = 10;
	
	float distance = 0;
	
	// Update is called once per frame
	void Update () {
	
		transform.Translate(Vector3.forward *_Speed * Time.deltaTime);
		
		//Debug.DrawLine(transform.position, transform.forward * 10);
		
		distance += _Speed * Time.deltaTime;
		
		if(distance > _Range)
			Destroy(gameObject);
	}
}
