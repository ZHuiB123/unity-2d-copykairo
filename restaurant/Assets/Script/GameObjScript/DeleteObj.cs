using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DeleteObj : MonoBehaviour
{
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
            hit = Physics2D.Raycast(screenPos, Vector2.zero, 1000.0f, mask);
            if (hit)
            {
                int[] pos_C = TransformCoordinate._instance.TransformToCoordinate(hit.collider.transform.position);
                Vector3 pos_T = TransformCoordinate._instance.CoordinateToTransform(pos_C[0], pos_C[1]);
                DeleteHitObj(pos_C, pos_T);
                Destroy(gameObject);
                CameraMove._instance.enabled = true;
            }
        }
    }

    void DeleteHitObj(int[] pos_C, Vector3 pos_T)
    {
        MapDate InitialMapDate = new MapDate();
        foreach (Build obj in MapMessage._instance.BuildData)
        {
            if (obj.x == pos_T.x && obj.y == pos_T.y)
            {
                //ArcNumAdd(obj.id,1);
                if (obj.size >= 1)
                {
                    MapMessage._instance.MapCategory[pos_C[0], pos_C[1]] = InitialMapDate.MapCategory[pos_C[0] * 11 + pos_C[1]];
                }
                if (obj.size >= 2)
                {
                    MapMessage._instance.MapCategory[pos_C[0], pos_C[1] + 1] = InitialMapDate.MapCategory[pos_C[0] * 11 + pos_C[1] + 1];
                }
                if (obj.size >= 4)
                {
                    MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1]] = InitialMapDate.MapCategory[(pos_C[0] + 1) * 11 + pos_C[1]];
                    MapMessage._instance.MapCategory[pos_C[0] + 1, pos_C[1] + 1] = InitialMapDate.MapCategory[(pos_C[0] + 1) * 11 + pos_C[1] + 1];
                }
                MapMessage._instance.BuildData.Remove(obj);
                MapMessage._instance.BuildNum[obj.id]++;
                Destroy(hit.collider.gameObject);
                Debug.Log("²ð³ý³É¹¦");
                Destroy(gameObject);
                break;
            }
        }
    }

}
