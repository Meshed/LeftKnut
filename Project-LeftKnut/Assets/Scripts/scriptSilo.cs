using UnityEngine;
using System.Collections;

public class scriptSilo : MonoBehaviour {
	public int MaxStorage = 500;
    public int CostPerTurret = 50;

    private int _resourceCount = 100;
    private bool _isObjectAttached;
    private GameObject _objectAttaced;
	
	void Update ()
	{
	    PlaceTurretAtMousePosition();
	    MoveTurretWithMouse();
	}
    void OnGUI()
    {
        //var script = gameObject.GetComponent<scriptCanSelect>();

        //if (script)
        //{
        //    if (script.Selected || _isObjectAttached)
        //    {
                HandleAddTurretButton();                
        //    }
        //}
    }

    private void PlaceTurretAtMousePosition()
    {
        if (_isObjectAttached && Input.GetMouseButtonDown(0))
        {
            var turretScript = _objectAttaced.transform.GetComponent<TurretControl>();
            turretScript.PlaceTurret();
            _isObjectAttached = false;
            _objectAttaced = null;
            _resourceCount -= CostPerTurret;
        }
    }
    private void MoveTurretWithMouse()
    {
        if (_objectAttaced)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                _objectAttaced.transform.position = hit.point;
            }
        }
    }
    private void HandleAddTurretButton()
    {
        if (_resourceCount >= CostPerTurret)
        {
            if (GUI.Button(new Rect(0, 100, 100, 50), "Add Turret"))
            {
                _isObjectAttached = true;
                _objectAttaced = (GameObject) Instantiate(Resources.Load("Turret"));
            }
        }
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
