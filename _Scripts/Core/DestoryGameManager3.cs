using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestoryGameManager3 : MonoBehaviour
{    
    private void Awake()
    {
       Destroy(GameObject.Find("GameManager3")); 
    }  
}