using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoneyManager : MonoBehaviour
{
    public static HoneyManager hInstance;
    public Transform Spoon;

    public Transform JarObj;
    public GameObject MsgHighlight;

    public GameObject HoneyPrefab;
    Vector3 pos = Vector3.zero;
    public float HoneyReleaseTime = 0.5f;
    float timer = 0;
    bool Ispressed = false;

    public bool SpLimit = false, JrLimit = false;

    private void OnEnable()
    {
        EventHandler.RegisterEvent<Vector3>("UpdateInputPosition", UpdateInputPosition);
        EventHandler.RegisterEvent("JarLimit", JarLimit);
        EventHandler.RegisterEvent("SpoonLimit", SpoonLimit);
        EventHandler.RegisterEvent("CompleteSpoonPouring", CompleteSpoonPouring);

    }

    private void OnDisable()
    {
        EventHandler.UnregisterEvent<Vector3>("UpdateInputPosition", UpdateInputPosition);
        EventHandler.UnregisterEvent("JarLimit", JarLimit);
        EventHandler.UnregisterEvent("SpoonLimit", SpoonLimit);
        EventHandler.UnregisterEvent("CompleteSpoonPouring", CompleteSpoonPouring);
    }

    void Awake()
    {
        hInstance = this;
    }

    void JarLimit()
    {
        MsgHighlight.GetComponent<Text>().text = "JAR LIMIT";
        MsgHighlight.SetActive(true);
        JrLimit = true;
        CompleteJarPouring();
    }

    void SpoonLimit()
    {
        MsgHighlight.GetComponent<Text>().text = "SPOON LIMIT";
        MsgHighlight.SetActive(true);
        SpLimit = true;
    }

    void Update()
    {
        UpdateSpoonMovement();
        UpdateHoneySpawn();
    }

    void UpdateHoneySpawn()
    {
        if (JrLimit)
            return;

        if (InputManager.Ispressed && !SpLimit)
        {
            timer += Time.deltaTime;
            if(timer > HoneyReleaseTime)
            {
                Vector3 p = new Vector3(HoneyPrefab.transform.position.x, pos.y, pos.z);
                GameObject h = Instantiate(HoneyPrefab, p, HoneyPrefab.transform.rotation);
                timer = 0;
            }
        }
    }

    void UpdateSpoonMovement()
    {
        if (SpLimit)
        {
            Spoon.transform.rotation = Quaternion.Slerp(Spoon.transform.rotation, Quaternion.Euler(0, 0, -51), 15f * Time.deltaTime);
            return;
        }

        if (JrLimit)
            return;

        Vector3 sPos = new Vector3(Spoon.transform.position.x, pos.y, pos.z);
        Spoon.transform.position = Vector3.Lerp(Spoon.transform.position, sPos, 15f * Time.deltaTime); ;
    }

    void CompleteSpoonPouring()
    {
        SpLimit = false;
        MsgHighlight.SetActive(false);
        Spoon.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void CompleteJarPouring()
    {
        JrLimit = false;
        Reset();
    }

    public void Reset()
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
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
            //CurrentEmitter.transform.position = Vector3.Lerp(CurrentEmitter.transform.position, pos, 15f * Time.deltaTime);
            //CurrentEmitter.GetComponent<Obi.ObiEmitter>().enabled = true;
            InputManager.Ispressed = true;
        }
    }
}
    

