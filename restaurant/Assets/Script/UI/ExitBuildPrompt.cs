using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBuildPrompt : MonoBehaviour
{
    public static ExitBuildPrompt _instance;

    void Awake()
    {
        _instance = this;
    }

    public void OpenPrompt()
    {
        gameObject.SetActive(true);
    }

    public void ClosePrompt()
    {
        gameObject.SetActive(false);
    }
}
