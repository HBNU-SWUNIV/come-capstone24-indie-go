using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public Vector3 _offset = new Vector3(0, 1f, -10f);
    public float smooth = 5f;
    Vector3 target;

    public void SetPlayer(GameObject player) { this.player = player; }

    private void LateUpdate()
    {
        if (player != null)
        {
            
            target = player.transform.position + _offset;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smooth);
        }
    }
}
