using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStepper : MonoBehaviour
{
    [SerializeField] SpiderController controller;
    [SerializeField] Transform homeTransform;
    [SerializeField] float wantStepAtDistance = .1f;
    [SerializeField] float stepTime = .15f;
    [SerializeField] float stepHeight = .1f;
    [SerializeField] float overshootDistance = .1f;

    public bool moving = false;

    // Keeping track of velocity
    Vector3 lastPos;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = homeTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Velocity calculation
        velocity = (homeTransform.position - lastPos) / Time.deltaTime;
        lastPos = homeTransform.position;

        SetParameters(); // set variables according to central controller
    }

    IEnumerator MoveToHome()
    {
        // We don't want to try to move if we're already moving
        moving = true;

        // Get the start position and rotation so we can interpolate from here to the end position
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Quaternion endRot = homeTransform.rotation;
        // Displacement = velocity * time - we add our displacement to our position
        Vector3 futurePos = velocity * stepTime + homeTransform.position;

        // Get pure direction of foot movement
        Vector3 direction = (futurePos - startPos).normalized;

        // We may want to overshoot our future target, so we add a little displacement in the direction of travel
        Vector3 endPos = futurePos + direction * overshootDistance;

        Vector3 centerPos = (startPos + endPos) / 2; // Center point is the average position
        centerPos.y += stepHeight * 2; // If we want the leg to reach this height, we'll need to double it

        float timeElapsed = 0;

        do
        {
            // We need to wrangle our current time in the loop to a value between 0 - 1,
            // so we can use it in our Lerp
            timeElapsed += Time.deltaTime;
            float normalizedTime = timeElapsed / stepTime;

            // Nested Lerps to give us a Quadratic curve
            transform.position = Vector3.Lerp(Vector3.Lerp(startPos, centerPos, normalizedTime),
                Vector3.Lerp(centerPos, endPos, normalizedTime), normalizedTime);

            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);
            yield return null;

        } while (timeElapsed < stepTime);

        // Reset flag when we're done so other legs can move
        moving = false;
    }

    public void TryMove()
    {
        float distance = Vector3.Distance(transform.position, homeTransform.position);

        if (moving) return;

        // We want to take a step if we're outside our comfort zone
        if (distance > wantStepAtDistance)
        {
            StartCoroutine("MoveToHome");
        }
    }

    void SetParameters()
    {
        wantStepAtDistance = controller.stepDistance;
        stepTime = controller.stepTime;
        stepHeight = controller.stepHeight;
        overshootDistance = controller.overshootDistance;
    }
}
