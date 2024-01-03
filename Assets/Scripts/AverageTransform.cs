using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageTransform : MonoBehaviour
{
    [SerializeField] Transform[] transforms;
    [SerializeField] float heightOffset = .1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 averagePos = Vector3.zero;
        Vector3 averageRot = Vector3.zero;
        foreach (Transform transform in transforms)
        {
            averagePos += transform.position;
            averageRot += transform.forward;
        }
        averagePos /= transforms.Length;
        averageRot /= transforms.Length;

        this.transform.position = averagePos + Vector3.up * heightOffset;
        this.transform.forward = averageRot;
    }
}
