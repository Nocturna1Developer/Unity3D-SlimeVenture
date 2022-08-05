using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestoryGameManager : MonoBehaviour
{    
    private void Awake()
    {
       Destroy(GameObject.Find("GameManager1")); 
    }  
}