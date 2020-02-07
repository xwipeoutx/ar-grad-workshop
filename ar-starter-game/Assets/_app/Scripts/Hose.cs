using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hose : MonoBehaviour
{
    [SerializeField] float emissionRate = 1f;
    
    [SerializeField] GameObject waterDrop;
    [SerializeField] float maxLifetime = 1;
    [SerializeField] float hoseSpeed = 5;

    void Update()
    {
        if (waterDrop == null)
            return;
        
        if (Random.value < emissionRate * Time.deltaTime)
        {
            StartCoroutine(SpawnADrop());
        }
    }

    private IEnumerator SpawnADrop()
    {
        var obj = Instantiate(waterDrop, transform.position, Quaternion.identity);
        var rigidBody = obj.GetComponent<Rigidbody>();
        rigidBody.velocity = (transform.up + Random.onUnitSphere*0.1f) * hoseSpeed;
        yield return new WaitForSeconds(maxLifetime);
        
        Destroy(obj);
    }
}
