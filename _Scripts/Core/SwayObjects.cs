// using System;
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

// public class SwayObjects : MonoBehaviour
// {
//     private Vector3 _startAngle;          // Reference to the object's original angle values
//     private double _rotationSpeed  = 1f;   // Speed variable used to control the animation
//     private double _rotationOffset = 50f;  // Rotate by 50 units
//     private double _finalAngle;            // Keeping track of final angle to keep code cleaner

//     private void Start()
//     {
//         _startAngle = transform.eulerAngles;  // Get the start position
//     }

//     private void Update()
//     {
//         Sway();
//     }

//     private void Sway()
//     {
//         _finalAngle = _startAngle.y + Math.Sin(Environment.TickCount*_rotationSpeed)*_rotationOffset;  //Calculate animation angle
//         transform.eulerAngles = new Vector3(_startAngle.x, _finalAngle , _startAngle.z); //Apply new angle to object
//     }
// }