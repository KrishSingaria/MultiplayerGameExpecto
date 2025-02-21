using UnityEngine;
using Unity.Netcode;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class GPSlocation : NetworkBehaviour
{
    private TextMeshProUGUI textboi;
    bool startedGPS = false;
    bool startingGPS = false;
    private string error_str;
    [SerializeField] private Button sendButton;
    void Start()
    {
        textboi = GetComponent<TextMeshProUGUI>();
        sendButton.onClick.AddListener(OnSendButtonClicked);
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
        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Is Client");
        }
        if (NetworkManager.Singleton.IsConnectedClient) { Debug.Log("Is Connected"); }
    }

    IEnumerator StartGPS()
    {
        startingGPS = true;
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
    private void OnSendButtonClicked()
    {
        Debug.Log("Button Pressed");
        if (IsOwner)
        {
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            
            SendLocationServerRpc(latitude, longitude);
        }
        // Send to server

    }

    [ServerRpc]
    private void SendLocationServerRpc(float latitude, float longitude, ServerRpcParams rpcParams = default)
    {
        // This runs on the server
        Debug.Log($"Received location from client {rpcParams.Receive.SenderClientId}:");
        Debug.Log($"Latitude: {latitude}, Longitude: {longitude}");


    }

    void OnDisable()
    {
        // Stop the location service when not needed
        Input.location.Stop();
    }
}
