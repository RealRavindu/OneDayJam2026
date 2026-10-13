using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    public Transform tetrisWaypoint, raccoonWaypoint;
    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0) //left
        {
            //first check if tetriswaypoint is accessible
            //if not do cant go animation
            //if possible:
            transform.position = tetrisWaypoint.position;
        }
    }
}
