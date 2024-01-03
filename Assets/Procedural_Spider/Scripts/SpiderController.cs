using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] LegStepper backLeft;
    [SerializeField] LegStepper backRight;
    [SerializeField] LegStepper frontLeft;
    [SerializeField] LegStepper frontRight;

    public float stepDistance = .1f;
    public float stepTime = .15f;
    public float stepHeight = .2f;
    public float overshootDistance = .1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LegUpdateCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * .5f);
        //transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }

    IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            // Try moving one diagonal pair of legs
            do
            {
                backLeft.TryMove();
                frontRight.TryMove();

                // Wait a frame
                yield return null;
            } while (backLeft.moving || frontRight.moving);

            // Do the same thing for the other diagon pair
            do
            {
                backRight.TryMove();
                frontLeft.TryMove();

                // Wait a frame
                yield return null;
            } while (backRight.moving || frontLeft.moving);
        }
    }
}
