using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;

    [Header("Ground Check")]
    public Transform groundCheckPoint; // ตัวแปรสำหรับ CheckSphere
    public float groundCheckRadius = 0.2f; // ตัวแปรสำหรับ CheckSphere
    public LayerMask groundLayer; // <<< เราต้องเพิ่มตัวนี้เข้ามา!

    public Animator anim;

    private Rigidbody rb;
    public bool isGrounded; // (ตั้งเป็น public ไว้เช็คใน Inspector ได้)
    private bool facingRight = true;

    private Vector2 moveInput;

    private int animIDMoveSpeed;
    private int animIDJump;
    private int animIDGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        // เพิ่มการตรวจสอบเผื่อลืมลาก anim มาใส่
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        animIDMoveSpeed = Animator.StringToHash("MoveSpeed");
        animIDJump = Animator.StringToHash("Jump");
        animIDGrounded = Animator.StringToHash("Grounded");
    }


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger(animIDJump);
        }
    }

    void Update()
    {
        CheckGrounded(); // เรียกใช้ฟังก์ชัน CheckGrounded (ที่แก้ไขแล้ว)
        anim.SetBool(animIDGrounded, isGrounded);
    }

    void FixedUpdate()
    {
        float horizontalInput = moveInput.x;

        // (แก้ไข) ใช้ rb.velocity (ตัว v เล็ก) ครับ
        rb.linearVelocity = new Vector3(horizontalInput * moveSpeed, rb.linearVelocity.y, 0f);

        float currentSpeed = Mathf.Abs(horizontalInput * moveSpeed);
        anim.SetFloat(animIDMoveSpeed, currentSpeed);

        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // --- นี่คือฟังก์ชันที่แก้ไขให้ถูกต้อง ---
    void CheckGrounded()
    {
        if (groundCheckPoint == null)
        {
            Debug.LogWarning("GroundCheckPoint ยังไม่ได้ถูกตั้งค่าใน Inspector!");
            isGrounded = false;
            return;
        }

        // ใช้ CheckSphere ตามที่ตกลงกันไว้
        isGrounded = Physics.CheckSphere(
            groundCheckPoint.position,
            groundCheckRadius,
            groundLayer // ใช้ LayerMask ที่เราเพิ่มไว้
        );
    }
}