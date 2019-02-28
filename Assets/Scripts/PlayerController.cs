using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float fuelBurnSpeed = 1f;

    public float GetFuelAmount()
    {
        return fuelAmount;
    }

    [SerializeField]
    private float fuelRegenSpeed = 0.3f;

    private float fuelAmount = 1f;
    

    [Header("Spring settings: ")]
   
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    [SerializeField]
    private LayerMask enviromentMask;

    private ConfigurableJoint joint;
    private PlayerMotor motor;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);


    }
    void Update()
    {

        if (PauseMenu.IsOn)
        {
            return;
        }

        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, enviromentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);

        }


        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical)* speed;
        animator.SetFloat("ForwardVelocity", _zMov);

        //apply movement
        motor.Move(_velocity);


        // turning around
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //apply rotation
        motor.Rotate(_rotation);

        //Camera Rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //apply rotation
        motor.RotateCamera(_cameraRotationX);


        //calcule thurterforce
        Vector3 _thrusterForce = Vector3.zero;
        


        if (Input.GetButton("Jump") && fuelAmount >0f)
        {
            fuelAmount -= fuelBurnSpeed * Time.deltaTime;
            if( fuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            fuelAmount += fuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }
        fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);

        // apply thrusterfirce
        motor.ApplyThruster(_thrusterForce);
    }

    

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {
        positionSpring = _jointSpring,
        maximumForce = jointMaxForce};
    }
}
