using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject oner;
    private GameObject bullet;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BulletInstantiate() 
    {
        Vector3 newPos = new Vector3(0.75f, 0.45f, 0);
        bullet = Instantiate(this.gameObject, newPos, Quaternion.identity);
    }

    public void BulletDistroy() 
    {
        Destroy(this.gameObject);
    }
}
