using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteTable : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    public GameObject DeleteObj;
    public GameObject Menu;
    public GameObject Builder;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    // Update is called once per frame
    public void ButtonClick()
    {
        Instantiate(DeleteObj);
        Menu.SetActive(false);
        Builder.SetActive(false);
    }
}
