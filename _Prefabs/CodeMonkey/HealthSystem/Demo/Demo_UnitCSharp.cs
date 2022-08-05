using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    /// <summary>
    /// Demo Unit
    /// This one creates the HealthSystem through the C# constructor instead of using the Component
    /// </summary>
    public class Demo_UnitCSharp : MonoBehaviour, IGetHealthSystem {

        [SerializeField] private ParticleSystem damageParticleSystem;
        [SerializeField] private ParticleSystem healParticleSystem;

        private HealthSystem healthSystem;


        private void Awake() {
            healthSystem = new HealthSystem(100);

            healthSystem.OnDead += HealthSystem_OnDead;
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnHealed += HealthSystem_OnHealed;
        }

        private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
            healParticleSystem.Play();
        }

        private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
            damageParticleSystem.Play();
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e) {
            transform.eulerAngles = new Vector3(0, 0, -90);
            damageParticleSystem.Play();
        }

        private void OnMouseDown() {
            healthSystem.Damage(10);
        }

        public HealthSystem GetHealthSystem() {
            return healthSystem;
        }

    }

}