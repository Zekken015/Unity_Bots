using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointinAABB : MonoBehaviour
{
    public Transform TestPoint;
    public Material CollisionMaterial;
    Renderer renderer;
    Material defaultMaterial;
    BoxCollider boxCollider;
    public int health = 100;
    public TextMesh healthText;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // X Axis
        var xCheckMin = TestPoint.position.x > boxCollider.bounds.min.x;
        var xCheckMax = TestPoint.position.x < boxCollider.bounds.max.x;

        // Z Axis
        var zCheckMin = TestPoint.position.z > boxCollider.bounds.min.z;
        var zCheckMax = TestPoint.position.z < boxCollider.bounds.max.z;


        // Y Axis
        var yCheckMin = TestPoint.position.y > boxCollider.bounds.min.y;
        var yCheckMax = TestPoint.position.y < boxCollider.bounds.max.y;

        Debug.Log("Test1");

        if (xCheckMin && xCheckMax && zCheckMin && zCheckMax && yCheckMin && yCheckMax)
        {
            Debug.Log("Collision!");
            renderer.material = CollisionMaterial;
            var damage = Random.Range(20, 35);

            TakeDamage(damage);

            Destroy(TestPoint);
        }
        else
        {
            Debug.Log("Test2");
            renderer.material = defaultMaterial;
        }
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
