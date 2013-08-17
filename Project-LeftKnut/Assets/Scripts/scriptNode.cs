using UnityEngine;

public class scriptNode : MonoBehaviour {
	public int HarvestRate = 10;
	public int TotalHarvestRemaining = 100;

    private bool _isAlive = true;

	void Update () {
	    if (!_isAlive)
	    {
	        Destroy(gameObject);
	    }
	}

	public int Harvest()
	{
	    if (TotalHarvestRemaining > 0)
	    {
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
