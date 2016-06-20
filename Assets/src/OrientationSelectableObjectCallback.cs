using UnityEngine;
using System.Collections;

public class OrientationSelectableObjectCallback : SelectableObjectCallback {

    public GameObject OrentationReference;
    public bool DebugDraw;
    private bool rotatingTowards = false;
    private bool movingTowards = false;
    private Quaternion rotateTowards;
    private Rigidbody rigidBody;
    private Vector3 lastForce;

    private const float SELECTION_DISTANCE = 2.0f;
    private const float SPEED = 1000.0f;
    private const float MINIMUM_DISTANCE = 0.05f;

	// Use this for initialization
	void Start () {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        lastForce = Vector3.zero;
        Debug.Assert(rigidBody != null, "Please attach a rigidBody to this object");
        Debug.Assert(OrentationReference != null, "Please set a rotation reference object");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (DebugDraw)
        {
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 7.0f, Color.magenta);
        }

        if (rotatingTowards)
        {
            rotateTowards = Quaternion.LookRotation(OrentationReference.transform.position - gameObject.transform.position, OrentationReference.transform.up);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rotateTowards, Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (movingTowards)
        {
           
            Vector3 diff = OrentationReference.transform.position + (OrentationReference.transform.forward.normalized * SELECTION_DISTANCE) - gameObject.transform.position;
            Vector3 frc = diff * SPEED;
            rigidBody.AddForce(-lastForce);
            rigidBody.AddForce(frc);
            lastForce = frc;

            if (DebugDraw)
            {
                Debug.DrawRay(gameObject.transform.position, diff, Color.grey);
            }
            
            if (Vector3.Distance(gameObject.transform.position, OrentationReference.transform.position + (OrentationReference.transform.forward.normalized * SELECTION_DISTANCE)) <= MINIMUM_DISTANCE)
            {
                rigidBody.AddForce(-lastForce);
                
            }

            // I'm going to leave this in commented form
            // This code will handle translating to infront of the orientation reference vis a moveTowards call mutating the position directly.
            // Since we are using this for a physics based game, the consequence of using this will be that it can have strange interations with
            // the physics of target object. 
            //Vector3 relative = OrentationReference.transform.position + (OrentationReference.transform.forward.normalized * SELECTION_DISTANCE);
            //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, relative, SPEED * Time.deltaTime);
        }
    }

    override public bool ObjectSelected()
    {
        rotatingTowards = true;
        movingTowards = true;
        Debug.Log("Second Selected...");
        return true;
    }

    public override bool ObjectDeselected()
    {
        rotatingTowards = false;
        movingTowards = false;
        Debug.Log("Second Deselected....");
        return true;
    }
}
