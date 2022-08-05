/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Thanks!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    /// <summary>
    /// Health System: Damage, Heal, fires several events when data changes.
    /// Use on Units, Buildings, Items; anything you want to have some health
    /// Use HealthSystemComponent if you want to add a HealthSystem directly to a Game Object instead of using the C# constructor
    /// </summary>
    public class HealthSystem {

        public event EventHandler OnHealthChanged;
        public event EventHandler OnHealthMaxChanged;
        public event EventHandler OnDamaged;
        public event EventHandler OnHealed;
        public event EventHandler OnDead;

        private float healthMax;
        private float health;

        /// <summary>
        /// Construct a HealthSystem, receives the health max and sets current health to that value
        /// </summary>
        public HealthSystem(float healthMax) {
            this.healthMax = healthMax;
            health = healthMax;
        }

        /// <summary>
        /// Get the current health
        /// </summary>
        public float GetHealth() {
            return health;
        }

        /// <summary>
        /// Get the current max amount of health
        /// </summary>
        public float GetHealthMax() {
            return healthMax;
        }

        /// <summary>
        /// Get the current Health as a Normalized value (0-1)
        /// </summary>
        public float GetHealthNormalized() {
            return health / healthMax;
        }

        /// <summary>
        /// Deal damage to this HealthSystem
        /// </summary>
        public void Damage(float amount) {
            health -= amount;
            if (health < 0) {
                health = 0;
            }
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
            OnDamaged?.Invoke(this, EventArgs.Empty);

            if (health <= 0) {
                Die();
            }
        }

        /// <summary>
        /// Kill this HealthSystem
        /// </summary>
        public void Die() {
            OnDead?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Test if this Health System is dead
        /// </summary>
        public bool IsDead() {
            return health <= 0;
        }

        /// <summary>
        /// Heal this HealthSystem
        /// </summary>
        public void Heal(float amount) {
            health += amount;
            if (health > healthMax) {
                health = healthMax;
            }
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Heal this HealthSystem to the maximum health amount
        /// </summary>
        public void HealComplete() {
            health = healthMax;
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
            OnHealed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Set the Health Amount Max, optionally also set the Health Amount to the new Max
        /// </summary>
        public void SetHealthMax(float healthMax, bool fullHealth) {
            this.healthMax = healthMax;
            if (fullHealth) health = healthMax;
            OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
            OnHealthChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Set the current Health amount, doesn't set above healthAmountMax or below 0
        /// </summary>
        /// <param name="health"></param>
        public void SetHealth(float health) {
            if (health > healthMax) {
                health = healthMax;
            }
            if (health < 0) {
                health = 0;
            }
            this.health = health;
            OnHealthChanged?.Invoke(this, EventArgs.Empty);

            if (health <= 0) {
                Die();
            }
        }





        /// <summary>
        /// Tries to get a HealthSystem from the GameObject
        /// The GameObject can have either the built in HealthSystemComponent script or any other script that creates
        /// the HealthSystem and implements the IGetHealthSystem interface
        /// </summary>
        /// <param name="getHealthSystemGameObject">GameObject to get the HealthSystem from</param>
        /// <param name="healthSystem">output HealthSystem reference</param>
        /// <param name="logErrors">Trigger a Debug.LogError or not</param>
        /// <returns></returns>
        public static bool TryGetHealthSystem(GameObject getHealthSystemGameObject, out HealthSystem healthSystem, bool logErrors = false) {
            healthSystem = null;

            if (getHealthSystemGameObject != null) {
                if (getHealthSystemGameObject.TryGetComponent(out IGetHealthSystem getHealthSystem)) {
                    healthSystem = getHealthSystem.GetHealthSystem();
                    if (healthSystem != null) {
                        return true;
                    } else {
                        if (logErrors) {
                            Debug.LogError($"Got HealthSystem from object but healthSystem is null! Should it have been created? Maybe you have an issue with the order of operations.");
                        }
                        return false;
                    }
                } else {
                    if (logErrors) {
                        Debug.LogError($"Referenced Game Object '{getHealthSystemGameObject}' does not have a script that implements IGetHealthSystem!");
                    }
                    return false;
                }
            } else {
                // No reference assigned
                if (logErrors) {
                    Debug.LogError($"You need to assign the field 'getHealthSystemGameObject'!");
                }
                return false;
            }
        }

    }

}