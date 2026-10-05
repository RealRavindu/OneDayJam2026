using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
