using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour {
    public Vector3 point;
    public float speed;
    public bool DestroyWhenReached = true;

    Transform thist;

    void Start()
    {
        thist = transform;
    }
    void Update()
    {
        if (thist.position != point)
            thist.position = Vector3.MoveTowards(thist.position, point, speed * Time.deltaTime);
        else
            Destroy(this);
    }
}
