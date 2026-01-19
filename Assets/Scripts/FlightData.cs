using UnityEngine;
using System;

[System.Serializable]
public class FlightResponse {
    public FlightInfo[] data;
}

[System.Serializable]
public class FlightInfo {
    public string flight_status;
    public LiveInfo live; // This contains the lat/long we need
    public AirlineInfo airline;
}

[System.Serializable]
public class LiveInfo {
    public float latitude;
    public float longitude;
    public float altitude;
    public float direction;
}

[System.Serializable]
public class AirlineInfo {
    public string name;
}