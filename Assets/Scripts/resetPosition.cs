using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPosition : MonoBehaviour
{

    public void reset()
    {
        Vector3 temp = transform.position;
        temp = new Vector3(6f, 0, 0);
        transform.position = temp;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
