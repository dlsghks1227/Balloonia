using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(
           gameObject.transform.position,
           player.gameObject.transform.Find("CamPosition").position,
           Time.deltaTime * 6.0f);
        gameObject.transform.LookAt(
            player.gameObject.transform.Find("CamLookAtTarget").position
            );
    }
}
