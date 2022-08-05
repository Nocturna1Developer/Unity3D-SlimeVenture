using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.HealthSystemCM {

    /// <summary>
    /// Health System Demo
    /// Showcases a Unit with a Health System and some UI Buttons to Damage and Heal that Unit
    /// The Unit has a HealthSystemComponent which creates a HealthSystem
    /// The Buttons have a reference for that HealthSystemComponent and deal damage
    /// 
    /// Also demonstrates all the Events fired by the HealthSystem which can be used to update a Health Bar
    /// </summary>
    public class Demo : MonoBehaviour {

        [SerializeField] private GameObject getHealthSystemGameObject;
        [SerializeField] private TextMesh textMesh;

        private List<string> logList = new List<string>();

        private void Awake() {
            textMesh.text = "";

            AddLog("Log...");
        }

        private void Start() {
            HealthSystem.TryGetHealthSystem(getHealthSystemGameObject, out HealthSystem healthSystem, true);

            healthSystem.OnHealthChanged += (object sender, System.EventArgs e) => {
                AddLog("OnHealthChanged");
            };
            healthSystem.OnDamaged += (object sender, System.EventArgs e) => {
                AddLog("OnDamaged");
            };
            healthSystem.OnDead += (object sender, System.EventArgs e) => {
                AddLog("OnDead");
            };
            healthSystem.OnHealed += (object sender, System.EventArgs e) => {
                AddLog("OnHealed");
            };
            healthSystem.OnHealthMaxChanged += (object sender, System.EventArgs e) => {
                AddLog("OnHealthMaxChanged");
            };
        }

        private void AddLog(string logString) {
            logList.Insert(0, logString);

            if (logList.Count > 10) {
                logList.RemoveAt(logList.Count-1);
            }

            string text = "";
            foreach (string str in logList) {
                text += str + "\n";
            }

            textMesh.text = text;
        }

    }

}