using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class ServerShit : MonoBehaviour
{
    private NetworkManager m_NetworkManager;
    [SerializeField] private GameObject location_prefab;
    [SerializeField] private Transform canvas_transform;

    void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!m_NetworkManager.IsClient && !m_NetworkManager.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host")) {
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetConnectionData("0.0.0.0", 7777); NetworkManager.Singleton.StartHost(); }
        if (GUILayout.Button("Client")) {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("192.168.251.11",7777); 
                NetworkManager.Singleton.StartClient(); 
                //m_NetworkManager.StartClient();
                //Instantiate(location_prefab, canvas_transform); 
        }
        if (GUILayout.Button("Server")) m_NetworkManager.StartServer();
    }

    void StatusLabels()
    {
        var mode = m_NetworkManager.IsHost ?
            "Host" : m_NetworkManager.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            m_NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

}

