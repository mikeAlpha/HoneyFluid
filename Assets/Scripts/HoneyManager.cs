using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyManager : MonoBehaviour
{
    public static HoneyManager hInstance;
    public Transform Solver;
    public Transform Spoon;

    public Transform JarObj;

    public GameObject CurrentEmitter;
    Vector3 pos = Vector3.zero;

    private void OnEnable()
    {
        hInstance = this;
    }

    void Update()
    {
        UpdateSpoonMovement();
        UpdateHoneyEmitter();
    }

    void UpdateSpoonMovement()
    {
        Vector3 sPos = new Vector3(Spoon.transform.position.x, pos.y, pos.z);
        Spoon.transform.position = Vector3.Lerp(Spoon.transform.position, sPos, 15f * Time.deltaTime); ;
    }

    void UpdateHoneyEmitter()
    {
#if UNITY_EDITOR
        if (Input.GetAxis("Fire1") > 0)
        {
            Vector3 mousePos = Input.mousePosition;
            UpdateInputPosition(mousePos);
        }

        else
        {
            CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = false;
        }


#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector3 tPos = t.position;
                UpdateInputPosition(tPos);
            }

            else if (t.phase == TouchPhase.Ended)
            {
                CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = false;
            }
        }
#endif
    }

    void UpdateInputPosition(Vector3 inputpos)
    {
        RaycastHit hitInfo;

        Ray mRay = Camera.main.ScreenPointToRay(inputpos);

        if (Physics.Raycast(mRay, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Honey"))
            {
                pos = hitInfo.collider.transform.GetChild(0).position;
            }
        }

        if (pos != Vector3.zero)
        {
            CurrentEmitter.transform.position = Vector3.Lerp(CurrentEmitter.transform.position, pos, 15f * Time.deltaTime);
            CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = true;
        }
    }
}
    

