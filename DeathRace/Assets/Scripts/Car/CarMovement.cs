using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

  [Header("General")]
  public int m_PlayerNumber = 1;

  // Car Components
  public Transform m_Chassi;
  public Transform m_LeftWheel;
  public Transform m_RightWheel;

  private string m_movementAxisName;      //To give player correct key controls (depending on player number)
  private string m_turnAxisName;          // ^  ^  ^
  private Rigidbody m_rigidbody;
  private float m_movementInputValue;
  private float m_turnInputValue;
  [Space]


  /*[Header("Audio")]
  public AudioSource m_MovementAudio;
  public AudioClip m_EngineIdle;
  public AudioClip m_EngineDriving;
  public float m_Pitchrange = 0.2f;
  
  private float m_originalPitch;
  [Space]*/


  //experimental, better turning
  [Header("Turning")]
  public float m_VCrit;
  public float m_TurnSpeed = 180f;
  public float m_turnGradient;
  public float m_turnSmoothing = 0.05f;

  private float m_turnValue;
  private Vector3 m_movement;             //The Vector by which the car is being moved forwards
  [Space]


  //experimental, acceleration;
  [Header("Acceleration")]
  public float m_ATop;                    //Maximum acceleration (at speed = 0, then decreses linear)
  public float m_BrakeStrength;
  public float m_RollDeceleration;        //How fast the car slows down if you let it roll out
  public float m_TopSpeed;
  public float m_ReverseTopSpeed;
  public float m_DeadZone;                //The range of speed of the car in which it gets stopped (See Coast())

  private float m_CurrentSpeed;
  private float m_CurrentAcceleration;      //Current acceleration
  private float m_AccelerationGradient;     //Gradient(Steigung) of acceleration
  [Space]

  [Header("Suspension")]
  public float m_SpeedDamping;
  public float m_SpeedTravel = -9;
  public float m_TurnDamping;
  public float m_TurnTravel = 1;

  private float lastSpeed = 0;              //Cars' CurrentSpeed at the last Update()
  private float speedMoveVelocity = 0.2f;          //reference for the Damp function
  private float lastTurnValue = 0;          //Cars' turnValue at the last Update()
  private float turnMoveVelocity = 0.05f;           //reference for the Damp function

  public float Velocity
  {
    get { return m_CurrentSpeed; }
  }
  private void Awake()
  {
    m_rigidbody = GetComponent<Rigidbody>();
  }

  private void OnEnable()
  {
    m_rigidbody.isKinematic = false;
    m_movementInputValue = 0f;
    m_turnInputValue = 0f;

    m_CurrentSpeed = 0f;
    m_AccelerationGradient = -(m_ATop / m_TopSpeed);
  }

  private void OnDisable()
  {
    m_rigidbody.isKinematic = true;
  }

  private void Start()
  {
    m_movementAxisName = "Vertical" + m_PlayerNumber;
    m_turnAxisName = "Horizontal" + m_PlayerNumber;

    //m_originalPitch = m_MovementAudio.pitch;
  }

  private void Update()
  {
    // Store player input and make sure Engine Audio is playing.
    m_movementInputValue = Input.GetAxis(m_movementAxisName);
    m_turnInputValue = Input.GetAxis(m_turnAxisName);

    //EngineAudio();
  }

  /*private void EngineAudio()
  {
    if(m_CurrentSpeed > 0)
    {
      if (m_MovementAudio.clip = m_EngineIdling;
      {
        m_MovementAudio.clip = m_EngineIdling;
        m_Movement.Audio.Play();
      }
    }
    else
    {
      if (m_MovementAudio.clip == m_EngineIdling)
      {
        m_MovementAudio.clip = m_EngineDriving;
        m_MovementAudio.Play();
      }
    }
  }*/

  private void FixedUpdate()  // Handles movement
  {
    // Move and turn the car.
    Move();
    Turn();
    TiltChassi();
  }

  private void Move()
  {
    // Adjust the position of the car based on the player's input.

    //1. Case: Driving forward, pressing W (Accelerate)
    if (m_movementInputValue > 0 && m_CurrentSpeed >= 0)
    {
      AccelerateForward();
    }
    //2. Case: Driving forward, pressing S (Braking)
    else if (m_movementInputValue < 0 && m_CurrentSpeed > 0)
    {
      Brake();
    }
    //3. Case: Driving backwards, pressing S (Accelerate backwards) 
    else if (m_movementInputValue < 0 && m_CurrentSpeed <= 0)
    {
      AccelerateBackward();
    }
    //4. Case: Driving backwards, pressing W (Braking backwards)
    else if (m_movementInputValue > 0 && m_CurrentSpeed < 0)
    {
      BrakeBackward();
    }
    //5. Case: No Input, Rolling out;
    else
    {
      coast();
    }

    // speedvector
    m_movement = transform.forward * m_CurrentSpeed * Time.deltaTime;

    // move car to speedvector
    m_rigidbody.MovePosition(m_rigidbody.position + m_movement);

  }



  private void AccelerateForward()
  {
    //Acceleration starts at m_ATop, then decreses until TopSpeed is reached
    if (m_CurrentSpeed >= m_TopSpeed)
    {
      m_CurrentAcceleration = 0;
      m_CurrentSpeed = m_TopSpeed;
    }
    else
    {
      m_CurrentAcceleration = (m_CurrentSpeed * m_AccelerationGradient) + m_ATop;
    }

    m_CurrentSpeed = m_CurrentSpeed + m_CurrentAcceleration * Time.deltaTime;
  }



  private void Brake()
  {
    m_CurrentSpeed = m_CurrentSpeed - m_BrakeStrength * Time.deltaTime;
  }



  private void AccelerateBackward()
  {
    if (m_CurrentSpeed <= m_ReverseTopSpeed)
    {
      m_CurrentSpeed = m_ReverseTopSpeed;
    }

    m_CurrentSpeed = m_CurrentSpeed - m_ATop * Time.deltaTime;
  }



  private void BrakeBackward()
  {
    m_CurrentSpeed = m_CurrentSpeed + m_BrakeStrength * Time.deltaTime;
  }



  private void coast()
  {
    //Zone: Stop the car when its' speed reaches the "zone", or else it will decelerate forever and never reach 0. (Zone is small enough to not be noticed by the player)
    if (m_CurrentSpeed < (0 - m_DeadZone) || m_CurrentSpeed > (m_DeadZone))
    {
      if (m_CurrentSpeed < 0)
      {
        //m_CurrentSpeed = m_CurrentSpeed - (m_CurrentSpeed / (1 / m_RollDeceleration)) * Time.deltaTime;
        m_CurrentSpeed = m_CurrentSpeed - (m_CurrentSpeed * m_RollDeceleration) * Time.deltaTime;
      }
      else if (m_CurrentSpeed > 0)
      {
        m_CurrentSpeed = m_CurrentSpeed - (m_CurrentSpeed / (1 / m_RollDeceleration)) * Time.deltaTime;
      }
    }
    else
      m_CurrentSpeed = 0;
  }

  private void Turn()
  {
    // Adjust the rotation of the car based on the player's input.

    // Turnspeed increases until critical speed, then decreses.
    if (m_CurrentSpeed <= m_VCrit && m_CurrentSpeed >= -(m_VCrit))
    {
      m_turnValue = m_TurnSpeed * Mathf.Abs(m_CurrentSpeed) * Time.deltaTime;
    }
    else
    {
      //speed bigger than VCrit
      m_turnValue = (m_TurnSpeed * m_VCrit * Time.deltaTime) - (Mathf.Abs(m_CurrentSpeed) - m_VCrit) * m_turnGradient;
    }

    //inverse turn if car is going backwards (more realistic)
    if (m_CurrentSpeed >= 0)
    {
      m_turnValue = m_turnValue * m_turnInputValue;

      //Turn wheels
      m_LeftWheel.localRotation = Quaternion.Euler(0, 0, m_turnValue * 20);
      m_RightWheel.localRotation = Quaternion.Euler(0, 0, m_turnValue * 20);
    }
    else
    {
      m_turnValue = -(m_turnValue * m_turnInputValue);

      //Turn wheels
      m_LeftWheel.localRotation = Quaternion.Euler(0, 0, -(m_turnValue * 20));
      m_RightWheel.localRotation = Quaternion.Euler(0, 0, -(m_turnValue * 20));
    }

    m_turnValue = Mathf.SmoothDampAngle(lastTurnValue, m_turnValue, ref turnMoveVelocity, m_turnSmoothing);

    //rotate car
    Quaternion turnRotation = Quaternion.Euler(0f, m_turnValue, 0f);

    m_rigidbody.MoveRotation(m_rigidbody.rotation * turnRotation);

    lastTurnValue = m_turnValue;
  }



  private void TiltChassi()
  {
    // Rotate Chassi depending on speed
    float desiredSpeedDegree = (m_CurrentSpeed - lastSpeed) * -m_SpeedTravel;
    float desiredTurnDegree;

    // When Car goes backwards, rotation while turning must be inversed
    if (m_CurrentSpeed >= 0)
      desiredTurnDegree = m_turnValue * -m_TurnTravel;
    else
      desiredTurnDegree = m_turnValue * m_TurnTravel;

    //smooth out degrees over time
    desiredSpeedDegree = Mathf.SmoothDampAngle(m_Chassi.transform.localEulerAngles.x, desiredSpeedDegree, ref speedMoveVelocity, m_SpeedDamping);
    desiredTurnDegree = Mathf.SmoothDampAngle(m_Chassi.transform.localEulerAngles.y, desiredTurnDegree, ref turnMoveVelocity, m_TurnDamping);

    m_Chassi.localRotation = Quaternion.Euler(desiredSpeedDegree, desiredTurnDegree, 0);

    lastSpeed = m_CurrentSpeed;
  }
}
