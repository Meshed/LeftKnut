using UnityEngine;
using System.Collections;

public class scriptSilo : MonoBehaviour {
	public int MaxStorage = 500;

	private int _resourceCount;
	
	void Update () {
	}
	
	public bool Deposit(int quantity)
	{
	    if ((_resourceCount + quantity) > MaxStorage)
	        return false;

	    _resourceCount += quantity;

	    return true;
	}

    public int GetResourceCount()
    {
        return _resourceCount;
    }
}
