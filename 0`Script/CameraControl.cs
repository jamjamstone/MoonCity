using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;

    [Range(1f, 20f)]
    public float distanceH = 5;
    [Range(1f, 20f)]
    public float distanceV = 5;



    Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos=player.transform.position;
        cameraPos.x += distanceH;
        cameraPos.y += distanceV;
        playerCamera.transform.position = cameraPos;
        playerCamera.transform.LookAt(player.transform);
    }
}
