using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    public GameObject Menu;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClickMenu);
    }

    // Update is called once per frame
    public void ButtonClickMenu()
    {
        if(Menu.activeSelf==false)
        {
            Menu.SetActive(true);
            CameraMove._instance.enabled = false;
        }
        else
        {
            Menu.SetActive(false);
            CameraMove._instance.enabled = true;
        }
    }
}
