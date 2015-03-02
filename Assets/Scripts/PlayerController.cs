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

    public void Kill()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);

        GameController.Instance.OnPlayerKilled();
    }
	
    private void Update() 
    {
        transform.Rotate(Vector3.up * (Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime));

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audio.Play();
        }
	}

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0)
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
