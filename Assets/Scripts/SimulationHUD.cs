using UnityEngine;
using TMPro;
public class SimulationHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockText;
    private float simStartTime;
    private float timeScale = 720f; 

    void Start()
    {
        simStartTime = Time.time;
    }

    void Update()
    {
        // 1. Calculate how many 'Simulated Seconds' have passed
        float elapsedRealSeconds = Time.time - simStartTime;
        float elapsedSimSeconds = elapsedRealSeconds * timeScale;

        // 2. Convert to a 24-hour clock format
        System.TimeSpan time = System.TimeSpan.FromSeconds(elapsedSimSeconds % 86400);
        
        // 3. Update the UI Text
        clockText.text = string.Format("{0:D2}:{1:D2}", time.Hours, time.Minutes);
    }
}
