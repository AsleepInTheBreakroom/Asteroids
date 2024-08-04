using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Movement
    private Vector3 direction = Vector3.zero;
    private float max_speed = 0f;
    private Vector3 velocity = Vector3.zero;

    //Graphics
    public GameObject asteroid_sprite;
    public Sprite[] array_of_sprites;
    private float spin_speed = 0f;

    //Screen Wrapping
    public Camera cam;
    private Vector2 cam_bottom_left = Vector2.zero;
    private Vector2 cam_top_right = Vector2.zero;
    private float wrap_radius = 0.64f;

    // Start is called before the first frame update
    void Start()
    {
        //Set max speed;
        max_speed = Random.Range(0.65f, 1.15f);

        //Set diection.
        float start_angle = Random.Range(0, 360);
        start_angle *= Mathf.Deg2Rad;
        direction.x = Mathf.Cos(start_angle);
        direction.y = Mathf.Sin(start_angle);

        //Set Velocity.
        velocity = max_speed * direction;

        //Sety sprite
        asteroid_sprite.GetComponent<SpriteRenderer>().sprite = array_of_sprites[Random.Range(0, array_of_sprites.Length)];

        //Set Spin speed.
        spin_speed = Random.Range(-20f, 20f);

        //Find camera
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        //Set camera vector to screen space. (In pixels)
        cam_top_right = new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0);

        //Convert from pixels to world space.
        cam_bottom_left = cam.ScreenToWorldPoint(cam_bottom_left);
        cam_top_right = cam.ScreenToWorldPoint(cam_top_right);
    }

    // Update is called once per frame
    void Update()
    {
        //Move asteroid.
        transform.position += velocity * Time.deltaTime;

        //Rotate
        asteroid_sprite.transform.Rotate(Vector3.forward, spin_speed * Time.deltaTime);

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
}
