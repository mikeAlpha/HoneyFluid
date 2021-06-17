using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static float timer = 0;
    public static bool Ispressed = false;

    void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {

        if (HoneyManager.hInstance.JrLimit)
            return;

        if (HoneyManager.hInstance.SpLimit)
            return;

#if UNITY_EDITOR
        if (Input.GetAxis("Fire1") > 0)
        {
            Vector3 mousePos = Input.mousePosition;
            EventHandler.ExecuteEvent<Vector3>("UpdateInputPosition", mousePos);
        }

        else
        {
            Ispressed = false;
            timer = 0;
            //CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = false;
        }


#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector3 tPos = t.position;
                EventHandler.ExecuteEvent<Vector3>("UpdateInputPosition", tPos);
            }

            else if (t.phase == TouchPhase.Ended)
            {
                //CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = false;
                timer = 0;
                Ispressed = false;
            }
        }
#endif
    }
}
