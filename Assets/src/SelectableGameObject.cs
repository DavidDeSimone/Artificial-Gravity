using UnityEngine;
using System.Collections;

public class SelectableGameObject : MonoBehaviour {
    // GameObject that the selected object will use as the basis of raycasting for collision
    public GameObject Selector;

    // Distance we will consider for object selection
    public float Distance;

    // If we want useful debug drawing to be done in scene
    public bool DebugDraw;

    private SelectableObjectCallback[] callbackObject;
    private bool outlineDirty = false;
    private bool objectSelected = false;

    // Use this for initialization
    void Start () {
        SelectableObjectCallback[] callbackComponent = gameObject.GetComponents<SelectableObjectCallback>();
        callbackObject = callbackComponent;
        
        Debug.Assert(Selector != null, "Please assign a selector object");
        Debug.Assert(callbackComponent != null, "Please assign a callback to this object, if you are going to make it selectable");
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forwardRay = Selector.transform.forward.normalized * Distance;
        RaycastHit outInfo;

        if (DebugDraw)
        {
            Debug.DrawRay(Selector.transform.position, forwardRay, Color.cyan);
        }

        if (Physics.Raycast(Selector.transform.position, forwardRay, out outInfo, 10.0f))
        {
            setThickness(1.1f);
            outlineDirty = true;
        }
        else
        {
            if (outlineDirty && !objectSelected)
            {
                setThickness(1.0f);
                outlineDirty = false;
            }
        }


        if (Input.GetButtonUp(InputService.ACTION_BUTTON))
        {
            if (objectSelected)
            {
                notifyDeselection();
            }
            else if (outlineDirty)
            {
                notifiySelection();
            }
        }
    }

    private void notifiySelection()
    {
        foreach (SelectableObjectCallback callback in callbackObject)
        {
            if (callback.ObjectSelected())
            {
                objectSelected = true;
                break;
            }
        }
    }

    private void notifyDeselection()
    {
        foreach (SelectableObjectCallback callback in callbackObject)
        {
            if (callback.ObjectDeselected())
            {
                objectSelected = false;
                break;
            }
        }
    }

    private void setThickness(float thickness)
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_Thickness", thickness);
    }
}
