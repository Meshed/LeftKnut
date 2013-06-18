using UnityEngine;
using System.Collections;

public class scriptSilo : MonoBehaviour {
	public int MaxStorage = 100;
	public int CurrentStorageLevel;
	
	void Update () {
	
	}
	
	public bool Deposit(int quantity)
	{
	    if ((CurrentStorageLevel + quantity) > MaxStorage)
	        return false;

	    CurrentStorageLevel += quantity;

	    return true;
	}
}
