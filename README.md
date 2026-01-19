# PeregrineVision
## Overview
### A Real-Time 3D Global Flight Visualization Engine
**The Goal**: To bridge the gap between abstract API data and immersive 3D visualization.

**The Tech**: Built in Unity using C#, consuming real-time JSON data from the Aviationstack API.
## Key Features
* **Live Data Integration**: Fetches real-time flight coordinates, altitudes, and headings.

* **Geospatial Mapping**: Converts Latitude/Longitude/Altitude into Unity Cartesian coordinates.

* **Interactive UI**: Clickable flight entities displaying metadata (Flight ID, Airline, Origin/Destination).

* **Performance Optimized**: Uses object pooling to handle hundreds of active flight entities smoothly.

## Tech Stack
- **Unity** (Universal Render Pipeline)
- **C#** (Asynchronous programming & JSON Parsing)
- **Aviationstack API**