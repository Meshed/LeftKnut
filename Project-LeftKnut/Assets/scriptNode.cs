using UnityEngine;
using System.Collections;

public class scriptNode : MonoBehaviour {
	public int harvestRate = 10;
	public int totalHarvestRemaining = 100;
	public Transform explosion;

	// Update is called once per frame
	void Update () {
		if(totalHarvestRemaining < 0)
		{
			print("Node is empty");
		}
	}
	
	public int Harvest()
	{
		if(explosion)
		{
			Transform temp = (Transform)Instantiate(explosion, transform.position, transform.rotation);
			Destroy(temp.gameObject, temp.particleSystem.duration + temp.particleSystem.startLifetime);
		}
		
		return harvestRate;
	}
}
