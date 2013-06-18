using UnityEngine;
using System.Collections;

public class GuiTest : MonoBehaviour {
	
	
	private bool _isObjectAttached = false;
	private GameObject _ObjectAttaced;
		
	// Use this for initialization
	void Start () {
	
	}
	
	void Update(){
		
		
		if(_isObjectAttached && Input.GetMouseButtonDown(0)){
			_isObjectAttached = false;
			_ObjectAttaced = null;
		}
	
		if(_ObjectAttaced)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(ray, out hit))
			{
				_ObjectAttaced.transform.position = hit.point;
				
			}
		}
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,100,50),"Add Turret"))
		{
			_isObjectAttached = true;
			_ObjectAttaced = (GameObject)Instantiate(Resources.Load("Turret"));;
			
		}
	}
}
