using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField,Header("Main Camera")]
    Camera cam;
    [SerializeField,Header("Player")]
    GameObject player;

    Transform player_T;

    // Start is called before the first frame update
    void Start()
    {
        player_T = player.transform;
                cam.transform.position = new Vector3(player_T.position.x, player_T.position.y +2, player_T.position.z -4);
   cam.transform.Rotate(20,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(player_T.position.x, cam.transform.position.y, player_T.position.z - 4);

    }
}
