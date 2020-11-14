using LiteNetLib;
using UnityEngine;

public class Client : MonoBehaviour
{
    private bool ticking = false;
    public int Tick { get; private set; } = 0;

    NetManager netManager;
    EventBasedNetListener netListener;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }

    public void StartClient()
    {
        Debug.LogError("starting client");

        netManager = new NetManager(netListener);
        netManager.Connect("localhost", 12345, "");

        ticking = true;
    }

    private void Start()
    {
        netListener = new EventBasedNetListener();
        netListener.PeerConnectedEvent += (server) =>
        {
            Debug.LogError($"Connected to server: {server}");
        };

        netListener.ConnectionRequestEvent += (request) =>
        {
            request.Accept();
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ticking) return;
        Tick += 1;

        netManager.PollEvents();
    }
}
