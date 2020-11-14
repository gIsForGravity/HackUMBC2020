using LiteNetLib;
using System.Net;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private bool ticking = false;
    public int Tick { get; private set; } = 0;

    private bool host = false;

    NetManager netManager;
    EventBasedNetListener netListener;

    public void StartHost()
    {
        host = true;
    }

    public void StartClient()
    {
        netManager.Start();
        netManager.Connect("localhost", 9050, "key");
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

        netManager = new NetManager(netListener);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ticking) return;
        Tick += 1;

        netManager.PollEvents();
    }
}
