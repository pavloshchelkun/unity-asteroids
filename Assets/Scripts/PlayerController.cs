using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 5f;
    public float dragForce = 0.5f;
    public float rotateSpeed = 180f;
    public float maxVelocity = 5f;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    public GameObject explosion;

    private float nextFire;

    private bool isMoving;

    public void Kill()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);

        GameController.Instance.OnPlayerKilled();
    }

    private void Start()
    {
        EasyButton.On_ButtonPress += OnButtonPress;
        EasyButton.On_ButtonUp += OnButtonUp;
        EasyJoystick.On_JoystickMove += OnJoystickMove;
    }

    private void OnDestroy()
    {
        EasyButton.On_ButtonPress -= OnButtonPress;
        EasyButton.On_ButtonUp -= OnButtonUp;
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
    }

    private void OnButtonPress(string buttonName)
    {
        switch (buttonName)
        {
            case "Shoot":
                Shoot();
                break;
            case "Move":
                isMoving = true;
                break;
        }
    }

    private void OnButtonUp(string buttonName)
    {
        switch (buttonName)
        {
            case "Move":
                isMoving = false;
                break;
        }
    }

    private void OnJoystickMove(MovingJoystick movingJoystick)
    {
        transform.Rotate(Vector3.up * movingJoystick.joystickAxis.x * rotateSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0f, movingJoystick.Axis2Angle(), 0f);
    }

    private void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audio.Play();
        }
    }
	
    private void Update() 
    {
        transform.Rotate(Vector3.up * (Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0 || isMoving)
        {
            rigidbody.AddForce(transform.forward * thrustForce * Time.deltaTime);
            rigidbody.drag = 0f;
        }
        else
        {
            rigidbody.drag = dragForce;
        }

        if (rigidbody.velocity.magnitude > maxVelocity)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
        }
    }
}
