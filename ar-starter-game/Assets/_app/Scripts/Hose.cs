using UnityEngine;
using Random = UnityEngine.Random;

public class Hose : MonoBehaviour
{
    [SerializeField] float emissionRate = 1f;
    
    [SerializeField] GameObject waterDrop;
    [SerializeField] float hoseSpeed = 5;

    void Update()
    {
        if (waterDrop == null)
            return;
        
        if (Random.value < emissionRate * Time.deltaTime)
        {
            SpawnADrop();
        }
    }

    public void ToggleHose()
    {
        enabled = !enabled;
    }

    private void SpawnADrop()
    {
        var obj = Instantiate(waterDrop, transform.position, Quaternion.identity);
        var rigidBody = obj.GetComponent<Rigidbody>();
        rigidBody.velocity = (transform.up + Random.onUnitSphere*0.1f) * hoseSpeed;
    }
}
