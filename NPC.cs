using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public enum NPCStates
    {
        Patrol,
        Chase,
        Attack
    }

    public GameObject BulletPrefab;
    public Vector3[] PatrolPoints;
    public Transform Player;
    public float ChaseRange = 7f;
    public float AttackRange = 3f;
    public int health = 100;
    public Material PatrolMaterial;
    public Material ChaseMaterial;
    public Material AttackMaterial;
    public TextMesh healthText;
    public TextMesh statusText;
    public TextMesh idText;

    NPCStates currentState = NPCStates.Patrol;
    NavMeshAgent navMeshAgent;
    MeshRenderer meshRenderer;
    int nextPatrolPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
        navMeshAgent.SetDestination(PatrolPoints[0]);

        var randomnum = UnityEngine.Random.Range(100, 999);

        idText.text = "ID: NPC" + randomnum.ToString();
        healthText.transform.LookAt(Camera.main.transform);
        statusText.transform.LookAt(Camera.main.transform);
        idText.transform.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == NPCStates.Patrol)
        {
            statusText.text = "Status: Patrol";
            Patrol();
        }
        else if (currentState == NPCStates.Chase)
        {
            statusText.text = "Status: Chase";
            Chase();
        }
        else if (currentState == NPCStates.Attack)
        {
            statusText.text = "Status: Attack";
            Attack();
        }
    }

    private void Patrol()
    {
        meshRenderer.material = PatrolMaterial;
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            nextPatrolPoint++;
            if (nextPatrolPoint >= PatrolPoints.Length)
            {
                nextPatrolPoint = 0;
            }

            //nextPatrolPoint = (nextPatrolPoint + 1) % PatrolPoints.Length; Code that can be used instead of above 3 lines.

            navMeshAgent.SetDestination(PatrolPoints[nextPatrolPoint]);
        }

        if (Vector3.Distance(transform.position, Player.position) < ChaseRange)
        {
            currentState = NPCStates.Chase;
        }
    }

    private void Attack()
    {
        meshRenderer.material = AttackMaterial;
        navMeshAgent.ResetPath();
        transform.LookAt(Player.position);

        if (Vector3.Distance(transform.position, Player.position) > AttackRange)
        {
            currentState = NPCStates.Chase;
        }

        AttackDelay();

    }

    private void Chase()
    {
        Debug.Log("player position:" + Player.position);
        meshRenderer.material = ChaseMaterial;
        navMeshAgent.SetDestination(Player.position);

        if (Vector3.Distance(transform.position, Player.position) < AttackRange)
        {
            currentState = NPCStates.Attack;
        }

        if (Vector3.Distance(transform.position, Player.position) > ChaseRange)
        {
            navMeshAgent.ResetPath();
            currentState = NPCStates.Patrol;
        }

    }

    public void AttackDelay()
    {
        var bulletPosition = transform.position + transform.forward * 1f;
        bulletPosition.y = 1f;

        var bulletGameObject = Instantiate(BulletPrefab, bulletPosition, Quaternion.identity);

        var bulletRigidBody = bulletGameObject.GetComponent<Rigidbody>();
        bulletRigidBody.velocity = transform.forward * 20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damage = Random.Range(20, 35);

        TakeDamage(damage);
    }

    private void TakeDamage(int damage)
    {
        Debug.Log("Damage: " + damage);

        health -= damage;

        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }

        healthText.text = health.ToString();
    }

}
