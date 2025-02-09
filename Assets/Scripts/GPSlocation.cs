using UnityEngine;
using System.Collections;
using TMPro;

public class GPSlocation : MonoBehaviour
{
    private TextMeshProUGUI textboi;
    bool startedGPS = false;
    private string error_str;
    void Start()
    {
        textboi = GetComponent<TextMeshProUGUI>();
        StartCoroutine(StartGPS());
    }
    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running) { 
            textboi.text = Input.location.lastData.latitude.ToString() +" "+ Input.location.lastData.longitude.ToString();
        }
        else
        {
            textboi.text = error_str;
        }
    }

    IEnumerator StartGPS()
    {
        // Check if location service is enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled by the user.");
            error_str = "Location services are not enabled by the user.";
            yield break;
        }

        // Start the location service
        Input.location.Start();

        // Wait until location service initializes
        int maxWait = 20; // seconds
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            error_str = "Waiting";
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the location service didn't initialize
        if (maxWait <= 0)
        {
            Debug.LogError("Timed out while initializing location service.");
            error_str = "Timeout";
            yield break;
        }

        // If the connection to the service failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location.");
            error_str = "Failed";
            yield break;
        }

        // If location is successfully retrieved
        Debug.Log($"Location: Latitude {Input.location.lastData.latitude}, Longitude {Input.location.lastData.longitude}");
        startedGPS = true;
    }

    void OnDisable()
    {
        // Stop the location service when not needed
        Input.location.Stop();
    }
}
