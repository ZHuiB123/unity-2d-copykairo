using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capital : MonoBehaviour
{
    // Start is called before the first frame update
    private int capital;
    private Text captext;
    void Start()
    {
        captext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        capital = MapMessage._instance.captical;
        captext.text = capital.ToString();
    }
}
