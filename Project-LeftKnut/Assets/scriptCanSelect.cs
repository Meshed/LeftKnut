using UnityEngine;

public class scriptCanSelect : MonoBehaviour {
	public bool Selected;
    public Color BaseColor;
	
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
        renderer.material.color = Color.green;
    }
    void SetToStandardColor()
    {
        renderer.material.color = BaseColor;
    }
}