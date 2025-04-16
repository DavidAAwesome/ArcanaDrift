using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float grabRange = 3f;
    public float moveForce = 150f;
    public float holdDistance = 2f; // Distance from camera
    public float verticalHoldDistance = 2f;
    public Transform holdPoint;
    public Transform playerObject;

    private Rigidbody heldObject;

    void Update()
    {
        // Make holdPoint follow the camera
        UpdateHoldPointPosition();

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

            // Optional: align held object rotation
            heldObject.MoveRotation(Quaternion.Lerp(heldObject.rotation, holdPoint.rotation, Time.fixedDeltaTime * 10f));
        }
    }

    void UpdateHoldPointPosition()
    {
        Transform cam = Camera.main.transform;
        Vector3 newPosition = playerObject.position + playerObject.forward * holdDistance + Vector3.down * verticalHoldDistance;
        holdPoint.position = newPosition;
        holdPoint.rotation = playerObject.rotation;
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