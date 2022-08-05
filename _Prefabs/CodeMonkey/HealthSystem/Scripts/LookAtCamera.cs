using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    /// <summary>
    /// Utility script to make a Transform look straight at the main camera
    /// Useful for HealthBar World Objects, always face camera
    /// </summary>
    public class LookAtCamera : MonoBehaviour {

        [SerializeField] private bool invert;

        private Transform mainCameraTransform;

        private void Awake() {
            mainCameraTransform = Camera.main.transform;
        }

        private void Update() {
            LookAt();
        }

        private void OnEnable() {
            LookAt();
        }

        private void LookAt() {
            if (invert) {
                Vector3 dir = (transform.position - mainCameraTransform.position).normalized;
                transform.LookAt(transform.position + dir);
            } else {
                transform.LookAt(mainCameraTransform.position);
            }
        }

    }

}