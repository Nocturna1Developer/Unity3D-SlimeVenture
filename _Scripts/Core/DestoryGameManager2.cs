using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestoryGameManager2 : MonoBehaviour
{    
    private void Awake()
    {
       Destroy(GameObject.Find("GameManager2")); 
    }  
}