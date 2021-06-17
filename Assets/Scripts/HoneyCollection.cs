using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCollection : MonoBehaviour
{
    public int TotalCollection = 15;

    int counter = 0;

    List<GameObject> liquidParticles = new List<GameObject>();

    public bool IsJar = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Fluid"))
        {
            Debug.Log(other.gameObject.name);
            if (liquidParticles.Count > 0)
            {
                if (liquidParticles.Contains(other.gameObject))
                    return;
                
            }

            if (!CheckCollectionLimit(other.gameObject))
            {
                Debug.Log(gameObject.name + " Limit");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fluid"))
        {
            Debug.Log(other.gameObject.name);
            if (liquidParticles.Count > 0)
            {
                if (liquidParticles.Contains(other.gameObject))
                {
                    counter--;
                    RemoveParticles(other.gameObject);
                }
                    
            }

            if(counter == 0 || liquidParticles.Count == 0)
            {
                if(!IsJar)
                    EventHandler.ExecuteEvent("CompleteSpoonPouring");
            }

        }
    }

    void RemoveParticles(GameObject particle)
    {
        liquidParticles.Remove(particle);
    }

    bool CheckCollectionLimit(GameObject particle)
    {
        if (liquidParticles.Count == TotalCollection)
        {
            if(!IsJar)
             EventHandler.ExecuteEvent("SpoonLimit");
            else
             EventHandler.ExecuteEvent("JarLimit");

            return false;
        }

        liquidParticles.Add(particle);
        counter++;
        return true;
    }
}
