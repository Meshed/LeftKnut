using UnityEngine;
using System.Collections;

public class scriptCanSelect : MonoBehaviour {
	public bool selected = false;
	
	public void Select()
	{
		selected = true;
	}
	
	public void Deselect()
	{
		selected = false;
	}
}
