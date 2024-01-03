using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Forage : StateMachineBehaviour
{
    NavMeshAgent agent;
    GameObject closestFood = null;
    Spider spider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spider = animator.GetComponent<Spider>();
        agent = animator.GetComponent<NavMeshAgent>();
        FindClosestFood(animator);

        if (closestFood != null)
        {
            agent.SetDestination(closestFood.transform.position);
        }
    }

    void FindClosestFood(Animator animator)
    {
        GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");

        float closestDist = Mathf.Infinity;

        foreach (GameObject food in allFood)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(animator.transform.position, food.transform.position, NavMesh.AllAreas, path))
            {
                float dist = CalculatePathLength(path);

                if (dist < closestDist)
                {
                    closestFood = food;
                    closestDist = dist;
                }
            }  
        }
    }

    float CalculatePathLength(NavMeshPath path)
    {
        Vector3[] corners = path.corners;

        float dist = 0;

        for (int i = 1; i < corners.Length; i++)
        {
            dist += Vector3.Distance(corners[i], corners[i - 1]);
        }
        return dist;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, closestFood.transform.position) < 0.5)
        {
            animator.SetTrigger("Stash");
            spider.carriedFood = closestFood;
            closestFood.transform.SetParent(animator.transform);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
