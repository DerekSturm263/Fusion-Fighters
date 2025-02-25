using Quantum.Demo;
using UnityEngine;
using UnityEngine.Events;

public class HostClientEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHost;
    [SerializeField] private UnityEvent _onClient;

    public static int DeviceIndex
    {
        get
        {
            if (UIMain.Client is not null)
                return UIMain.Client.LocalPlayer.ActorNumber;
            else
                return 0;
        }
    }

    private void Awake()
    {
        if (UIMain.Client is not null)
        {
            if (!UIMain.Client.InRoom || UIMain.Client.LocalPlayer.IsMasterClient)
                _onHost.Invoke();
            else
                _onClient.Invoke();
        }
        else
        {
            _onHost.Invoke();
        }
    }

    public void Invoke()
    {
        _onHost.Invoke();
    }
}
