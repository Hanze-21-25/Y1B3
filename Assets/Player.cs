using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 MovementInput { get; set; }
    private Vector2 MouseInput { get; set; }
    private float xRotation { get; set; }
    private float yRotation { get; set; }
    private RaycastHit downRayHit;
    private Ray downRay { get; set; }
    private Ray lookDirection { get; set; }
    private RaycastHit looksAt { get; set; }

    [SerializeField] private float reachLength; 
    
    [SerializeField] private Rigidbody body;
    
    [SerializeField] private new Transform camera;
    
    [SerializeField] private float speed;
    
    [SerializeField] private float jumpForce;
    
    [SerializeField] private float sensitivity;
    
    /** Damping of a vertical force used for hovering */
    [SerializeField] private float vForceDamping;
    
    [SerializeField] private float hoverForce;
    
    [SerializeField] private float hoverHeight;


    private void Start()
    {
        SetValues();
        UpdateInputs();
    }

    private void Update()
    {
        UpdateFrameDependentPhysics();
        UpdateInputs();
        Interact();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }
    
    /**
     * <summary> Used for setting values which are necessary for a player, in order to work</summary>>
     */
    private void SetValues()
    {
        xRotation = 0;
        yRotation = 0;
        camera = camera.transform;
    }

    /**
     * <summary> Sets default settings for a player's controls</summary>>
     */
    private void DefaultSettings()
    {
        hoverForce = 30f;
        hoverHeight = 2f;
        vForceDamping = 6f;
        speed = 9;
        sensitivity = 200;
        jumpForce = 180;
        reachLength = 2;

    }

    /**
     * <summary>Used for physics used in the FixedUpdate() method, which is not dependent on a framerate</summary>
     */
    private void UpdatePhysics()
    {
        Jump();
        Hover();
    }

    /**
     * <summary> Used for physics used in the Update() method, which is dependent on a framerate</summary>
     */
    private void UpdateFrameDependentPhysics()
    {
        downRay = new Ray(body.position ,Vector3.down);
        Physics.Raycast(downRay, out downRayHit);
    }

    /**
     * <summary> Calls all "input-update" functions. </summary>
     */
    private void UpdateInputs()
    {
        UpdateKeyboard();
        UpdateMouse();
        
    }

    /**
     * <summary> Updates keyboard inputs as well as applies velocity to player's body. </summary>"/>
     */
    private void UpdateKeyboard()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var MovementVector = transform.TransformDirection(MovementInput) * speed;
        body.velocity = new Vector3(MovementVector.x, body.velocity.y, MovementVector.z);
    }

    /**
     * <summary> Adds an upward-directed force on "Space" button press. </summary>
     */
    private void Jump()
    {
        if (!Airborn() && Input.GetKeyDown(KeyCode.Space)) {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    /**
     * <summary> Updates mouse inputs as well as applies camera rotation. </summary>
     */
    private void UpdateMouse()
    {
        MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        var dx = -MouseInput.y * sensitivity * Time.deltaTime;
        var dy = MouseInput.x * sensitivity * Time.deltaTime;
        if (xRotation + dx is < 45 and > -85)
        {
            xRotation += dx;
        }

        yRotation += dy;
        
        transform.rotation = Quaternion.Euler(0,yRotation,0);
        
        var limit = camera;
        limit.rotation = Quaternion.Euler(xRotation,yRotation,0);
        
        lookDirection = new Ray(camera.transform.position, camera.forward);
        Physics.Raycast(lookDirection, out var tmpLooksAt, hoverHeight);
        looksAt = tmpLooksAt;
    }

    /// <summary>
    /// Casts a ray down from a player's body centre, which length does not exceeds hoverHeight.
    /// </summary>
    /// <returns>
    /// Whether a player is standing on the ground or if they are in air.
    /// </returns>
    private bool Airborn()
    {
        return !Physics.Raycast(body.transform.position, Vector3.down, hoverHeight);
    }
    
    /**
     * <summary>
     * Keeps players floating to prevent them from continuously stumbling and getting stuck;
     * Adds a force, directed at the sky sufficient to keep player hovering, if player higher than
     * a hover height the force gets removed.
     * </summary>
     */
    private void Hover()
    {

        if (Airborn()) { return; }

        body.AddForce(Vector3.up * ((hoverHeight - downRayHit.distance) * hoverForce - body.velocity.y * vForceDamping),ForceMode.Acceleration);
    }

    /**
     * <summary>
     * Instantly changes a player rotation, by the set angle in degrees.
     * </summary>
     * <remarks>
     * Not Smoothly;
     * </remarks>
     */
    public void Rotate(float angle)
    {
        if (camera == null) return;
        var dy = MouseInput.x * sensitivity + angle;
        yRotation += dy;
        transform.rotation = Quaternion.Euler(0,yRotation,0);
        camera.rotation = Quaternion.Euler(xRotation,yRotation,0);
        Debug.Log("Rotated  player by " + angle + " degrees");
    }
    
    /**
     * <summary>
     * Launches a forward-directed ray, from the camera's center.
     * </summary>
     * <returns>
     *  Whether an object that a player is looking at is in a reach distance;
     * </returns>
     */
    private bool InReach()
    {
        return Physics.Raycast(lookDirection, reachLength);
    }

    /**
     * <summary> On "F" key press - Switch() method of an interactable object called. </summary>
     * <remarks>
     * Only called if an object that a player is looking at in the reach distance of the the player and
     * it inherits Interactable class
     * </remarks>
     */
    private void Interact()
    {
        if (!InReach()) return;

        var interactableSubject = looksAt.collider.GetComponent<Interactable>();

        if (interactableSubject == null) return;

        if (Input.GetKeyDown(KeyCode.F)){
            interactableSubject.Switch();
        }
    }

}