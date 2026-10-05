using UnityEngine;
using Unity.Netcode;

public class Cursor : NetworkBehaviour
{
    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        mousePosition.x = Mathf.Clamp(mousePosition.x, -9, 9); mousePosition.y = Mathf.Clamp(mousePosition.y, -9, 9);
        transform.position = mousePosition;
    }
}
