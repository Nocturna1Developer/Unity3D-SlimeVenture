using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only allows you to put one script on it
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    // 0 for not moved, 1 for fully moved gives the editor a slider
    //[Range(0, 1)] [SerializeField] 
    float movementFactor; 

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    // This will move the object by the movement factor in a sinusioudal fashion
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau); 

        // half the ampliytude of the sin wave and moves it up by one
        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
