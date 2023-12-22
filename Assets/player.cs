using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private CharacterController CharacterController;
    [SerializeField] private float playerSpeed =5f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float rotationSpeed = 10f;
   
    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;

    public static player instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CheckWinner.instance.isWinner)
        {
            case true:
                animator.SetBool("winner", CheckWinner.instance.isWinner);
                break;
            case false:
                movement();
                break;
        }
        
    }
    void movement()
    {
        groundedPlayer = CharacterController.isGrounded;
        if(CharacterController.isGrounded && playerVelocity.y < -2f)
        {
            playerVelocity.y = -1f;
        }

        groundedPlayer = CharacterController.isGrounded;
        float HorizontalInput = Input.GetAxis("Horizontal");
         float VerticalInput = Input.GetAxis("Vertical");

         Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)*
           new Vector3(HorizontalInput, 0,VerticalInput);
         Vector3 movementDirection = movementInput.normalized;
        CharacterController.Move(movementDirection * playerSpeed * Time.deltaTime);
       
        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if(Input.GetButton("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
        }
       
        playerVelocity.y += gravityValue * Time.deltaTime;
        CharacterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("ground", CharacterController.isGrounded);
    }
}
