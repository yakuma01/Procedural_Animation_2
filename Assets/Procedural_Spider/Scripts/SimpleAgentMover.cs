using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgentMover : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;

    Vector3[] pathPoints;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 target = hit.point;
                agent.SetDestination(target);

                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(this.transform.position, target, NavMesh.AllAreas, path))
                {
                    pathPoints = path.corners;
                }
            }
        }

        if (pathPoints != null)
        {
            if (pathPoints.Length > 1)
            {
                for (int i = 1; i < pathPoints.Length; i++)
                {
                    Debug.DrawLine(pathPoints[i - 1], pathPoints[i]);
                }
            }
        }
    }
}
