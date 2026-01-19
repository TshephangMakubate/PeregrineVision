using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class PeregrineManager : MonoBehaviour
{
    [SerializeField] private string accessKey;
    private string baseUrl = "http://api.aviationstack.com/v1/flights";
    [SerializeField] private bool useMockData = true;
    [SerializeField] private TextAsset mockJsonFile;
    [SerializeField] private float refreshRateSeconds = 28800f;
    [SerializeField] private GameObject planePrefab;
    void Awake()
    {
        LoadSecrets();
    }
    void LoadSecrets()
    {
        string path = Path.Combine(Application.dataPath, "../secrets.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // Use a simple wrapper to parse the key
            SecretData secrets = JsonUtility.FromJson<SecretData>(json);
            accessKey = secrets.access_key;
        }
        else
        {
            Debug.LogError("secrets.json not found! Please create it in the project root.");
        }
    }
    [System.Serializable]
    private class SecretData { public string access_key; } 
    void Start()
    {
        // Use mock data if the flag is set, otherwise fetch live data from the API
        if (useMockData)
        {
            LoadMockData();
        }
        else
        {
            StartCoroutine(LiveUpdateLoop());
        }
    }

    void LoadMockData()
    {
        if (mockJsonFile != null)
        {
            // Parsing of the local file exactly like the API response would be parsed
            FlightResponse responseData = JsonUtility.FromJson<FlightResponse>(mockJsonFile.text);
            
            Debug.Log($"[MOCK] Loaded {responseData.data.Length} flights from local file.");
            ProcessFlightData(responseData);
        }
        else
        {
            Debug.LogError("Mock JSON file is missing in the Inspector!");
        }
    }
    void ProcessFlightData(FlightResponse data)
    {
        float radius = 50.5f;
        foreach (var flight in data.data)
        {
            // Conversion of latitude and longitude to radians for potential use in positioning calculations
            float lat = flight.live.latitude * Mathf.Deg2Rad;
            float lon = flight.live.longitude * Mathf.Deg2Rad;
            // Conversion of Spherical to cartesian coordinates
            float x = radius * Mathf.Cos(lat) * Mathf.Cos(lon);
            float y = radius * Mathf.Sin(lat);
            float z = radius * Mathf.Cos(lat) * Mathf.Sin(lon);

            Vector3 targetPosition = new Vector3(x, y, z);
            // Instantiation of a plane prefab at the calculated position, facing the center of the globe
            GameObject plane = Instantiate(planePrefab, targetPosition, Quaternion.identity);
            plane.transform.LookAt(Vector3.zero);
            Debug.Log($"Processing Flight: {flight.airline.name}");
        }
    }

    IEnumerator LiveUpdateLoop()
    {
        while (true) // Keep running while the app is active
        {
            yield return StartCoroutine(FetchLiveFlights()); // Wait for the fetch to finish
        
            Debug.Log($"Waiting {refreshRateSeconds}s before next update...");
            yield return new WaitForSeconds(refreshRateSeconds); // Pause the coroutine
        }
    }

    IEnumerator FetchLiveFlights()
    {
        // Construction of the full URL with the key and a filter for active flights
        string fullUrl = $"{baseUrl}?access_key={accessKey}&flight_status=active";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
        {
            // Sending of the request and waiting (yield) until it returns
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Parsing of the JSON string into the serializable classes
                string jsonResponse = webRequest.downloadHandler.text;
                FlightResponse responseData = JsonUtility.FromJson<FlightResponse>(jsonResponse);

                Debug.Log($"Successfully fetched {responseData.data.Length} live flights!");

                // Logic for spawning 3D planes 
                foreach (FlightInfo flight in responseData.data)
                {
                    Debug.Log($"Flight: {flight.airline.name} is at Lat: {flight.live.latitude}");
                }
            }
            else
            {
                Debug.LogError($"API Error: {webRequest.error}");
            }
        }
    }
}

