using UnityEngine;

public class scriptNode : MonoBehaviour {
	public int HarvestRate = 10;
	public int TotalHarvestRemaining = 100;
	public Transform Explosion;

	void Update () {
		if(TotalHarvestRemaining <= 0)
		{
			print("Node is empty");
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

            DescreaseRemainingHarvest();

            return GetHarvestAmount();
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
        if (TotalHarvestRemaining < 0)
        {
            TotalHarvestRemaining = 0;
        }
    }
}
