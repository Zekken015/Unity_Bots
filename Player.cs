using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform Pointer;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit))
            {
                //Debug.Log("Collision Point: " + rayHit.point);
                navMeshAgent.SetDestination(rayHit.point);
                Pointer.position = rayHit.point;
            }
        }
    }

    private void Shoot()
    {
        var bulletPosition = transform.position + transform.forward * 1f;
        bulletPosition.y = 0.5f;

        var bulletGameObject = Instantiate(BulletPrefab, bulletPosition, Quaternion.identity);

        var bulletRigidBody = bulletGameObject.GetComponent<Rigidbody>();
        bulletRigidBody.velocity = transform.forward * 20f;
    }
}
