using UnityEngine;
using System.Collections;

public class scriptSilo : MonoBehaviour {
	public int maxStorage = 100;
	public int currentStorageLevel = 0;
	
	void Update () {
	
	}
	
	void Deposit(int quantity)
	{
		if((currentStorageLevel + quantity) > maxStorage)
		{
			print("Storage is full");
		}
		else
		{
			currentStorageLevel += quantity;
		}
	}
}
