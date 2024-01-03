using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    GameObject box;
    Rigidbody boxRb;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        box = GameObject.Find("Box");
        boxRb = box.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        boxRb.AddRelativeForce(10 * Vector3.forward * forwardInput);
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    box.transform.Translate(Vector3.forward * speed);
        //}
    }
}
