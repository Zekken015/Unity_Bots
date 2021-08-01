using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereinSphere : MonoBehaviour
{
    //public NPC NPCData;
    public Transform TestSphere;
    public Material CollisionMaterial;
    Material defaultMaterial;
    Renderer TestSphereRenderer;
    SphereCollider collider;
    SphereCollider testSphereCollider;
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        TestSphereRenderer = TestSphere.GetComponent<Renderer>();
        //defaultMaterial = TestSphereRenderer.material;
        testSphereCollider = TestSphere.GetComponent<SphereCollider>();
        collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        var xDistance = TestSphere.position.x - transform.position.x;
        var yDistance = TestSphere.position.y - transform.position.y;
        var zDistance = TestSphere.position.z - transform.position.z;

        var distanceSquared = (xDistance * xDistance) + (yDistance * yDistance) + (zDistance * zDistance);
        var distance = Mathf.Sqrt(distanceSquared);

        var sumOfRadiuses = collider.radius + testSphereCollider.radius;
        if (distance < sumOfRadiuses)
        {
            Debug.Log("Collision!");
            TestSphereRenderer.material = CollisionMaterial;
            var damage = Random.Range(20, 35);

            //NPCData.TakeDamage();
        }
        else
        {
            TestSphereRenderer.material = defaultMaterial;
        }
    }

    
}
