using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour {
    public static UiManager i;

    [SerializeField] private List<UiWindow> windowsList; // Only used to copy windows into windows dictionary;
    private Dictionary<UiWindow.ID, UiWindow> windows = new Dictionary<UiWindow.ID, UiWindow>();
    private UiWindow activeWindow;
    [SerializeField] private UiHud hud;

    private void Awake() {
        i = this;

        // Gather all windows to the windows dictionary
        for (int i = 0; i < windowsList.Count; i++) {
            windows.Add(windowsList[i].id, windowsList[i]);
        }

        //hud.Initialize();

        ClientManagerL.i.OnConnected += ConnectionManager_OnConnected;

        DontDestroyOnLoad(this);
    }

    private void ConnectionManager_OnConnected() {
        OpenWindow(UiWindow.ID.login);
    }

    public UiWindow GetActiveWindow() {
        return activeWindow;
    }

    public UiWindow GetWindow(UiWindow.ID windowId) {
        return windows[windowId];
    }

    public void OpenWindow(UiWindow.ID windowId) {
        if (windows.ContainsKey(windowId)) {
            if (activeWindow != null)
                activeWindow.Close();

            activeWindow = windows[windowId];
            activeWindow.Open();
        }
        else {
            Debug.LogError("This UI manager does not contain a window with the ID: " + windowId);
        }
    }

    public void CloseActiveWindow() {
        if (activeWindow != null)
            activeWindow.Close();
    }
}
