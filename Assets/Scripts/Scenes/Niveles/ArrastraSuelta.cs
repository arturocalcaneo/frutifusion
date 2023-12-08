using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastraSuelta : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayPoint = ray.GetPoint(distance);
            transform.position = new Vector2(rayPoint.x, rayPoint.y);
        }
    }
}
