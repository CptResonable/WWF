using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiWindow_login : UiWindow {
    [SerializeField] private TMP_InputField ifUsername;

    public delegate void LoginBtnDelegate(string username);
    public static event LoginBtnDelegate OnClickEvent_btnLogin;

    public void OnBtnLogin() {
        OnClickEvent_btnLogin?.Invoke(ifUsername.text);
    }
}
