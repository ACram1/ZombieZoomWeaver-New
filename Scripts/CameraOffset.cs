using UnityEngine;
using Unity.Cinemachine;

public class CameraOffset : MonoBehaviour
{
    public CinemachineCamera vcam;
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0, 1, 0);

    //Sends Camera to where Player is when a trigger is walked through, counteracts slow vertical follow speed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 pos = playerTransform.position - vcam.transform.position;
        pos.z = 0;
        vcam.OnTargetObjectWarped(playerTransform, pos + offset);
    }
}