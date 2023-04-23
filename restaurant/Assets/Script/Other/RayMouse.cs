using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMouse : MonoBehaviour
{
    // Start is called before the first frame update
    private Ray ray;
    private RaycastHit2D hit;
    private LayerMask mask;
    void Start()
    {
        mask = 1 << 6;
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -10;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
            hit = Physics2D.Raycast(screenPos, Vector2.zero,1000.0f,mask);
            if (hit)
            {
                print(hit.collider.name);
            }
        }
    }
}
