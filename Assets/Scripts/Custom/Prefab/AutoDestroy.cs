using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float timeAuto;

    void Start()
    {
        Destroy(gameObject,timeAuto);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
