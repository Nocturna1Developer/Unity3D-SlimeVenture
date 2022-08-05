using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    public class Demo_Zombie : MonoBehaviour, IGetHealthSystem {

        private HealthSystem healthSystem;

        private void Awake() {
            healthSystem = new HealthSystem(100);
            healthSystem.OnDead += HealthSystem_OnDead;
        }

        private void Update() {
            Vector3 moveDir = new Vector3(-1, 0, 0);
            float speed = 10f;
            transform.position += moveDir * speed * Time.deltaTime;
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e) {
            Destroy(gameObject);
        }

        public void Damage() {
            healthSystem.Damage(60);
        }

        public HealthSystem GetHealthSystem() {
            return healthSystem;
        }

    }

}