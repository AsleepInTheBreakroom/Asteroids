using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Input
    private Vector2 input_vec = Vector2.zero;

    //Rotation
    private float rotate_speed = 200f;

    //Movement
    private float max_speed = 5.5f;
    private float acceleration = 13f;
    private Vector3 velocity = Vector3.zero;

    //Screen Wrapping
    public Camera cam;
    private Vector2 cam_bottom_left = Vector2.zero;
    private Vector2 cam_top_right = Vector2.zero;
    private float wrap_radius = 0.16f;

    //Ship Visuals
    public GameObject ship_sprite;
    public GameObject fire_sprite;
    private float jitter_amount = 0.04f;



    // Start is called before the first frame update
    void Start()
    {
        //Set camera vector to screen space. (In pixels)
        cam_top_right = new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0);

        //Convert from pixels to world space.
        cam_bottom_left = cam.ScreenToWorldPoint(cam_bottom_left);
        cam_top_right = cam.ScreenToWorldPoint(cam_top_right);

        //Turn of fire.
        fire_sprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If player is holding left/right button.
        if(input_vec.x != 0)
        {
            //Rotate the player.
            transform.Rotate(Vector3.forward, -Mathf.Sign(input_vec.x) * rotate_speed * Time.deltaTime);
        }

        //If w key is down...
        if(input_vec.y > 0)
        {
            //Add acceleration to velocity.
            velocity += transform.up * acceleration * Time.deltaTime;

            //Limit velocity.
            velocity = Vector3.ClampMagnitude(velocity, max_speed);

            //Jitter
            ship_sprite.transform.localPosition = new Vector3(Random.Range(-jitter_amount, jitter_amount),
                                                              Random.Range(-jitter_amount, jitter_amount), 0);

            //Enable and jitter fire.
            fire_sprite.SetActive(true);
            fire_sprite.transform.localPosition = new Vector3(Random.Range(-jitter_amount, jitter_amount) + 0.01f,
                                                              Random.Range(-jitter_amount, jitter_amount) - 0.25f, 0);
        }
        else //Player not holding the w key.
        {
            //Turn off fire.
            fire_sprite.SetActive(false);
        }

        //Move player.
        transform.position += velocity * Time.deltaTime;

        //Screen Wrap
        //Right Side
        if (transform.position.x - wrap_radius > cam_top_right.x)
            transform.position = new Vector3(cam_bottom_left.x - wrap_radius + 0.01f, transform.position.y, 0);
        
        //Left Side
        if (transform.position.x + wrap_radius < cam_bottom_left.x)
            transform.position = new Vector3(cam_top_right.x + wrap_radius - 0.01f, transform.position.y, 0);

        //Top Side
        if (transform.position.y - wrap_radius > cam_top_right.y)
            transform.position = new Vector3(transform.position.x, cam_bottom_left.y - wrap_radius + 0.01f, 0);

        //Bottom Side
        if (transform.position.y + wrap_radius < cam_bottom_left.y)
            transform.position = new Vector3(transform.position.x, cam_top_right.y + wrap_radius - 0.01f, 0);

    }

    public void CaptureMoveInput(InputAction.CallbackContext context)
    {
        input_vec = context.ReadValue<Vector2>();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, wrap_radius);
    }
}
