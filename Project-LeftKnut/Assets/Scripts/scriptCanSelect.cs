using UnityEngine;

public class scriptCanSelect : MonoBehaviour {
	public bool Selected;
	public Material SelectedMaterial;
	public Material StandardMaterial;
	
	public void Select()
	{
		Selected = true;
        SetToSelectedColor();
	}
	public void Deselect()
	{
		Selected = false;
        SetToStandardColor();
	}

    void SetToSelectedColor()
    {
		
		var body = transform.Find("Body");
		if(body != null)
		{
        	body.renderer.material = SelectedMaterial;
		}
    }
    void SetToStandardColor()
    {
		var body = transform.Find("Body");
		if(body != null)
		{
        	body.renderer.material = StandardMaterial;
		}
    }
}