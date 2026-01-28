using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleSetting : MonoBehaviour
{
    [SerializeField]
    GameObject gameObject;

    Transform child;
    ParticleSystem p_System;
    Transform obj_transform;
    Vector3 keep_rotation;

    float time = 0;
    void Start()
    {
        p_System = gameObject.GetComponent<ParticleSystem>();
        obj_transform = gameObject.GetComponent<Transform>();
        keep_rotation = obj_transform.eulerAngles;

        child = gameObject.transform.Find("BreakCheckBox");

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        obj_transform.eulerAngles = keep_rotation;
        if (!p_System.isPlaying)
            Destroy(gameObject);

        if (child != null)
        {
            child.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 15);
            if (time > 2.5f)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
