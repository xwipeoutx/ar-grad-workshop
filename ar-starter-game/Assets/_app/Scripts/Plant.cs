using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] LayerMask growthSource;
    [SerializeField] float growthRate = 0.01f;
    [SerializeField] float maxScale = 2f;

    private Vector3 initialScale;
    private float scaleMultiplier;

    void Start()
    {
        initialScale = transform.localScale;
        scaleMultiplier = 1;
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.layer & growthSource) == growthSource)
            return;

        scaleMultiplier += growthRate;

        if (scaleMultiplier > maxScale)
        {
            enabled = false;
            scaleMultiplier = maxScale;
        }

        transform.localScale = initialScale * scaleMultiplier;
    }
}
