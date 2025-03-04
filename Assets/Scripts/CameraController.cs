using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _focus;
    [SerializeField] private float _minRadius, _maxRadius;

    private float _radius = 5;
    
    private void Update()
    {
        transform.LookAt(_focus);
        if (Input.GetMouseButton(1))
        {
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            transform.eulerAngles += new Vector3(-mouseY * 5, mouseX * 5, 0);
        }


        _radius -= Input.mouseScrollDelta.y;
        _radius = Mathf.Clamp(_radius, _minRadius, _maxRadius);
        
        transform.position = _focus.position -
                             transform.forward * _radius;
    }
}
