using UnityEngine;

public class CursorSprite : MonoBehaviour
{
    private Transform parentTransform;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        parentTransform = transform.parent;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, parentTransform.position, moveSpeed * Time.deltaTime);
    }
}
