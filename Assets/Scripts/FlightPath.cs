using UnityEngine;

public class FlightPath : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 controlPoint;
    private float journeyTime = 60f; // Seconds for the demo trip
    private float startTime;
    
    public void Initialize(Vector3 start, Vector3 end, float duration)
    {
        startPoint = start;
        endPoint = end;
        journeyTime = duration;
        Vector3 midPoint = Vector3.Lerp(start, end, 0.5f);
        controlPoint = midPoint + (midPoint.normalized * 20f);
        journeyTime = duration;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.time - startTime) / journeyTime;

        if (t <= 1.0f)
        {
            // The Bezier Formula
            Vector3 m1 = Vector3.Lerp(startPoint, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, endPoint, t);
            transform.position = Vector3.Lerp(m1, m2, t);

            // Make the plane point toward its next position
            transform.LookAt(Vector3.Lerp(m1, m2, t + 0.01f)); 
        }
        else
        {
            OnDestinationReached();
        }
    }
    void OnDestinationReached()
    {
       Destroy(gameObject);
    }
}
