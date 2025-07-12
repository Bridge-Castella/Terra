using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// dummy class for other platforms
#if !STEAMWORKS_NET
public static class SteamManager
{
    public static void Setup() { }
    public static void Update() { }
    public static void Destroy() { }
    public static void CancelAuthTicket() { }
    public static Task<byte> WaitForSteamPayment(ulong _) { return null; }
    public static Task<string> GetAuthTicket() { return null; }
}
#else
using Steamworks;

public class SteamManager : MonoBehaviour
{
    const int timeout = 30;
    static readonly AppId_t appId = new AppId_t(3650830);

    private static bool steamInit = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void InitSteam()
    {
        if (!Packsize.Test())
        {
            Debug.LogError($"Invalid SteamAPI Packsize detected. " +
                "Wrong version of Steamworks.NET is imported");
        }

        if (!DllCheck.Test())
        {
            Debug.LogError($"SteamAPI DllCheck failed. " +
                "One or more of the Steamworks binaries seems to be the wrong version.");
        }

#if !DEVELOPMENT_BUILD
        try
        {
            if (SteamAPI.RestartAppIfNecessary(appId))
            {
                Debug.LogError("Steam is detected as running. Steam will relaunch the app.");
                Application.Quit();
            }
        }
        catch (DllNotFoundException error)
        {
            Debug.LogException(error);
            Application.Quit();
        }
#endif
    }

    private void Awake()
    {
        steamInit = SteamAPI.Init();
        if (!steamInit)
        {
            Debug.LogError("Steam not initialized.");
            return;
        }

        DontDestroyOnLoad(gameObject);
        SteamClient.SetWarningMessageHook(new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook));
    }

    public void Update()
    {
        if (!steamInit)
        {
            return;
        }

        SteamAPI.RunCallbacks();
    }

    public void Destroy()
    {
        if (!steamInit)
        {
            return;
        }

        SteamAPI.Shutdown();
    }

    [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
    private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText.ToString());
    }
}

#endif // DISABLESTEAMWORKS