using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // used by checkpoint trigger, slime controller, camera follow
    public Vector3 lastCheckpointPosition; 
    public Vector3 lastCheckpointPositionOfCamera;
    private static GameMaster _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }  
}
