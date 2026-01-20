using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSetting : MonoBehaviour
{
    [SerializeField]
    GameObject gameObject;

    ParticleSystem p_System;
    Transform obj_transform;
    Vector3 keep_rotation;
    void Start()
    {p_System=gameObject.GetComponent<ParticleSystem>();
        obj_transform =gameObject.GetComponent<Transform>();
        keep_rotation=obj_transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        obj_transform.eulerAngles=keep_rotation;
        if(!p_System.isPlaying)
            Destroy(gameObject);
    }
}
