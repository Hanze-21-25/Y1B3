using System;
using System.ComponentModel.Design;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 MovementInput { get; set; }
    private Vector2 MouseInput { get; set; }
    private float xRotation { get; set; }
    private float yRotation { get; set; }
    private RaycastHit downRayHit;
    private Ray downRay { get; set; }
    private bool doJump { get; set; }


    [SerializeField] private Rigidbody body;
    [SerializeField] private new Transform camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sensitivity;
    [SerializeField] private float vForceDamping;
    [SerializeField] private float hoverForce;
    [SerializeField] private float hoverHeight;


    void Update()
    {
        downRay = new Ray(body.position ,Vector3.down);
        Physics.Raycast(downRay, out downRayHit);
        
        if (Airborn()) { doJump = false; } else { doJump = true; }
        UpdateInputs();
        Move();
        Rotate();
    }
    void FixedUpdate()
    {
        Jump();
        Hover();
    }

    private void UpdateInputs()
    {
        MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MouseInput = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
    }

    private void Move()
    {
        Vector3 MovementVector = transform.TransformDirection(MovementInput) * speed;
        body.velocity = new Vector3(MovementVector.x, body.velocity.y, MovementVector.z);
    }

    private void Jump()
    {
        if (doJump && Input.GetKeyDown(KeyCode.Space)) {
            body.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void Rotate()
    {
        xRotation += -MouseInput.y * sensitivity * Time.deltaTime;
        yRotation += MouseInput.x * sensitivity * Time.deltaTime;

        transform.Rotate(0,MouseInput.x * sensitivity * Time.deltaTime,0);
        camera.transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
    }

    private bool Airborn()
    {
        if (downRayHit.distance > hoverHeight) { return true; }

        return false;
    }

    private void Hover()
    {

        if (Airborn()) { return; }

        body.AddForce(Vector3.up * ((hoverHeight - downRayHit.distance) * hoverForce - body.velocity.y * vForceDamping),ForceMode.Impulse);
        
    }

}
