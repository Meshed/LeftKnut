using UnityEngine;

public class scriptNode : MonoBehaviour {
	public int HarvestRate = 10;
	public int TotalHarvestRemaining = 100;
	public Transform Explosion;

    private bool _isAlive = true;

	void Update () {
	    if (_isAlive == false)
	    {
            print("Node is empty");
            Destroy(gameObject);
	    }
	}

	public int Harvest()
	{
	    if (TotalHarvestRemaining > 0)
	    {
		    if(Explosion)
		    {
			    var temp = (Transform)Instantiate(Explosion, transform.position, transform.rotation);
			    Destroy(temp.gameObject, temp.particleSystem.duration + temp.particleSystem.startLifetime);
		    }

	        int harvestedAmount = GetHarvestAmount();
            DescreaseRemainingHarvest();

	        return harvestedAmount;
	    }

	    return 0;
	}

    private int GetHarvestAmount()
    {
        return TotalHarvestRemaining >= HarvestRate ? HarvestRate : TotalHarvestRemaining;
    }
    private void DescreaseRemainingHarvest()
    {
        TotalHarvestRemaining -= HarvestRate;
        if (TotalHarvestRemaining <= 0)
        {
            _isAlive = false;
        }
    }
}
