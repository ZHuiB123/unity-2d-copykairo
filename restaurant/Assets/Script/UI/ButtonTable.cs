using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTable : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Building;
    public GameObject table;
    public GameObject Exitbuildpromet;
    public GameObject Menu;
    public GameObject Builder;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        GameObject obj = Instantiate(Building);
        obj.GetComponent<BuildObj>().SetObj(table);
        obj.GetComponent<SpriteRenderer>().sprite = table.GetComponent<SpriteRenderer>().sprite;
        Builder.SetActive(false);
        Menu.SetActive(false);
        Exitbuildpromet.SetActive(true);
    }
}
