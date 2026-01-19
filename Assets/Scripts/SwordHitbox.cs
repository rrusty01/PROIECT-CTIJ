using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private List<GameObject> inamiciLoviti = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (inamiciLoviti.Contains(other.gameObject)) return;

            inamiciLoviti.Add(other.gameObject);
            Debug.Log("Enemy hit: " + other.name);
            EnemyHealth healthscript = other.GetComponent<EnemyHealth>();
            if (healthscript != null)
            { 
                healthscript.TakeDamage(20);                
            }
        }
    }
    public void ReseteazaListaAtac()
    { 
        inamiciLoviti.Clear();
    }
}
