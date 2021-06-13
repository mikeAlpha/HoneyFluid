using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarEvents : MonoBehaviour
{
    public bool IsLeft = false;
    private bool Moving = false;

    void Update()
    {
        if (Moving)
        {
            float dir = IsLeft ? -1 : 1;
            Transform t = HoneyManager.hInstance.JarObj;

            Vector3 d = dir * Vector3.forward * Time.deltaTime * 5f;

            if (t.position.z > 2f)
            {
                t.position = new Vector3(t.position.x, t.position.y, 2f);
                return;
            }
            
            else if (t.position.z < -2f)
            {
                t.position = new Vector3(t.position.x, t.position.y, -2f);
                return;
            }


            t.position += d;
        }
    }

    public void OnPointerDown()
    {
        Moving = true;
    }

    public void OnPointerUp()
    {
        Moving = false;
    }
}
