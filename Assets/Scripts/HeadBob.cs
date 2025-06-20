using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobFrequency = 1.5f;
    public float bobAmplitude = 0.05f;

    private CharacterController controller;
    private Vector3 startPos;
    private float timer = 0f;

    void Start()
    {
        startPos = transform.localPosition;

        if (controller == null)
        {
            controller = GetComponentInParent<CharacterController>();
        }
    }

    void Update()
    {
        if (controller != null && IsPlayerMoving() && controller.isGrounded)
        {
            timer += Time.deltaTime * bobFrequency;
            float bobOffset = Mathf.Sin(timer) * bobAmplitude;
            transform.localPosition = startPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * 5f);
        }
    }

    bool IsPlayerMoving()
    {
        // Avoid using raw velocity — check horizontal movement only
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        return horizontalVelocity.magnitude > 0.1f;
    }
}
