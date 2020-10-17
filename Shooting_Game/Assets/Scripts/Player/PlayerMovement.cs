using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public float speed = 6f;    // The speed that the payer will move at.

    Vector3 movement;   // The vector to store the direction of the player's movememnt
    Animator anim;  // Reference to the animator component.
    Rigidbody playerRigidbody;  // Reference to the Rigidbody component
    int floorMask;  // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 100f;  // The length of the ray from the camera into the scene.


    #endregion

    #region UnityFunctions
    private void Awake()
    {
        // Create a layer ,asl fpr tje floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Store the input axis.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);

        // Turn the player to face the mouse cursor.
        Turning();

        // Animating the player.
        Animating(h, v);
    }
    #endregion

    void Move(float h, float v)
    {
        // Set the movement vector based on teh axis input.
        movement.Set(h, 0, v);

        // Normalizes the movement vector and makle it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {

        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the array.
        RaycastHit floorHit;

        // Perform the raycasthit and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from teh player to the point on the floor the raucast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) vased on looking the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell teh animator wheter or not the plaeyr os walking.
        anim.SetBool("IsWalking", walking);
    }

}// Main Class
