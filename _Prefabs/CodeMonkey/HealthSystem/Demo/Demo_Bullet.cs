using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    public class Demo_Bullet : MonoBehaviour {


        private void Start() {
            float bulletSpeed = 200f;
            GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collider2D) {
            if (collider2D.TryGetComponent(out Demo_Zombie zombie)) {
                zombie.Damage();
                Destroy(gameObject);
            }
        }

    }

}