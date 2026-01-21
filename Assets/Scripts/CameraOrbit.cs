using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10f; // Speed of the 5-minute orbit
    [SerializeField] private float distance = 70f; // Distance from the globe
    
    void Start()
    {
        if (target == null)
        {
            // Fallback to finding the Earth if not assigned
            GameObject earth = GameObject.Find("Earth");
            if (earth != null) target = earth.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            // 1. Rotate the camera around the Earth
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
            
            // 2. Ensure the camera is always looking at the center
            transform.LookAt(target.position);
        }
    }
}