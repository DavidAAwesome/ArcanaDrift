using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float grabRange = 3f;
    public float moveForce = 150f;
    public Transform holdPoint; // empty GameObject in front of player

    private Rigidbody heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryGrab();
            else
                DropObject();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null)
        {
            Vector3 directionToHoldPoint = holdPoint.position - heldObject.position;
            heldObject.linearVelocity = directionToHoldPoint * (moveForce * Time.fixedDeltaTime);
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grabRange))
        {
            if (hit.rigidbody != null)
            {
                heldObject = hit.rigidbody;
                heldObject.useGravity = false;
                heldObject.linearDamping = 10;
            }
        }
    }

    void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 1;
        heldObject = null;
    }
}