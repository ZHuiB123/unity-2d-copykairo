using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject builder;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }
    public void ButtonClick()
    {
        builder.SetActive(true);
    }
    // Update is called once per frame
}
