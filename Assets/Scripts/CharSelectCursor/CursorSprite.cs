using UnityEngine;

public class CursorSprite : MonoBehaviour
{
    private Transform parentTransform;
    [SerializeField] private float moveSpeed, closeEnoughThreshold;
    private Vector3 currentPos;

    private void Start()
    {
        parentTransform = transform.parent;
        currentPos = transform.position;
    }
    private void Update()
    {
        if((parentTransform.position - currentPos).magnitude > closeEnoughThreshold)
        {
            currentPos = Vector3.Lerp(currentPos, parentTransform.position, moveSpeed * Time.deltaTime);
            transform.position = currentPos;
        }
    }

    public void DoCantSelectAnimation()
    {

    }
}
