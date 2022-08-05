using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowMousePosition : MonoBehaviour 
{
    [Header("Core Properties")] 
    [SerializeField] private RectTransform _myCanvas;

    private void Start()
    {
        _myCanvas = GetComponent<RectTransform>();
    }
    private void Update()
    {
        CanvasScaler scaler = GetComponentInParent<CanvasScaler>();
        _myCanvas.anchoredPosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);
    }
}