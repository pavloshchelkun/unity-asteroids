using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public float tumble;

    public GameObject explosion;
    public int scoreValue;
    public int piecesOnDestroy;

    private int size;

    public void SetSize(int asteroidSize)
    {
        size = asteroidSize;
        // Scale asteroid by the size
        transform.localScale = Vector3.one * asteroidSize;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        rigidbody.velocity = transform.forward * speed;
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}

    private void OnTriggerEnter(Collider other)
    {
        // Asteroid can only hit projectiles or the player
        if (other.tag != Tags.Projectile && other.tag != Tags.Player)
        {
            return;
        }

        // Show explosion effect if exists
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        // Kill player if it is him
        if (other.tag == Tags.Player)
        {
            var player = other.GetComponent<PlayerController>();
            player.Kill();
        }
        // Kill projectile if it is it
        else
        {
            GameController.Instance.AddScore(scoreValue * size);
            Destroy(other.gameObject);
        }

        // Spawn smaller asteroids if needed
        if (size > 1)
        {
            for (int i = 0; i < piecesOnDestroy; i++)
            {
                GameController.Instance.CreateAsteroid(transform.position, size - 1);
            }
        }

        // Notify about destroyed asteroid
        GameController.Instance.OnAsteroidDestroyed();

        // Destroy this asteroid
        Destroy(gameObject);
    }
}
