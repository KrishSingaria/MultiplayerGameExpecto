using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Multiplayer UI")]
    public Button hostButton, clientButton, serverButton;

    [Header("Card System UI")]
    public TMP_Dropdown playerDropdown;
    public Button playCardButton;

    [Header("Card System")]
    public List<Card> availableCards;
    public Player localPlayer;
    private List<Player> allPlayers = new List<Player>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        serverButton.onClick.AddListener(StartServer);
        playCardButton.onClick.AddListener(PlayCard);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Started as HOST");
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Started as CLIENT");
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        Debug.Log("Started as SERVER");
    }

    public void SetLocalPlayer(Player player)
    {
        localPlayer = player;
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        allPlayers.Clear();
        allPlayers.AddRange(FindObjectsByType<Player>(FindObjectsInactive.Include, FindObjectsSortMode.None));

        playerDropdown.ClearOptions();
        List<string> playerNames = new List<string>();

        foreach (var player in allPlayers)
        {
            if (player != localPlayer)
            {
                playerNames.Add($"Player {player.OwnerClientId}");
            }
        }

        playerDropdown.AddOptions(playerNames);
    }

    public void PlayCard()
    {
        if (localPlayer == null || allPlayers.Count <= 1) return;

        int selectedIndex = playerDropdown.value;
        Player targetPlayer = allPlayers[selectedIndex];
        Card selectedCard = availableCards[0];

        Debug.Log($"Playing {selectedCard.Cardname} on Player {targetPlayer.OwnerClientId}");
        selectedCard.ApplyCardEffect(targetPlayer, localPlayer.OwnerClientId);
    }
}
