using UnityEngine;
using System.Collections;

public class TakesDamage : MonoBehaviour 
{
    public int Health = 100;
    public bool IsAlive = true;

	// Update is called once per frame
	void Update () 
    {
        if (!IsAlive)
        {
            Destroy(gameObject);
        }
	}

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            IsAlive = false;
        }
    }
}
