using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBox : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(box.transform.position.x, box.transform.position.y + 2.51f, box.transform.position.z - 5.85f);
    }
}
