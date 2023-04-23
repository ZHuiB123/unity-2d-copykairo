using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;

    private float input_x;
    private float input_y;
    public float speed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        Vector2 move_dir = new Vector2(input_x, input_y).normalized;
        rig.velocity = move_dir * speed;
    }
}
