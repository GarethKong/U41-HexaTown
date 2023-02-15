using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDestroy : MonoBehaviour
{

    public static HandDestroy Instance;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }
    
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void DestroyHand(bool check)
    {
        if (check)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyHand(GameManager.Instance.checkTouchEnd);
    }
}
