using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCoordinate : MonoBehaviour
{
    // Start is called before the first frame update
    public static TransformCoordinate _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this) { Destroy(gameObject); } //保证这个实例的唯一性
        //DontDestroyOnLoad(gameObject);
    }
    public Vector3 CoordinateToTransform(int x,int y)
    {
        float vec_X = (float)(x * 1 + y * 1);
        float vec_Y = (float)(-x * 0.5 + y * 0.5);
        Vector3 vec = new Vector3(vec_X, vec_Y, 0);
        return vec;
    }

    public int[] TransformToCoordinate(Vector3 tran)
    {
        int[] tra = new int[2];
        tra[0] = (int)(0.5 * (tran.x + 1) - tran.y);
        tra[1] = (int)(0.5 * (tran.x + 1) + tran.y);
        return tra;
    }
}
