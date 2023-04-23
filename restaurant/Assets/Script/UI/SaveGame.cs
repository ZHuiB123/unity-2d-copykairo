using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    // Update is called once per frame
    public void ButtonClick()
    {
        MapManager.SaveThisGame();
    }
}
