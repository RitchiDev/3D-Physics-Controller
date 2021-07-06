using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumper : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private CharacterInputHandler m_Input;

    [Header("Jumping")]
    [SerializeField] private float m_JumpVelocity = 7.5f;
    [SerializeField] private float m_JumpTerminal = 22.5f;
    [SerializeField] private float m_JumpDuration = 0.6667f;
    [SerializeField] private float m_CoyoteTimeThreshold = 0.33f;
    [SerializeField] private float m_AutoJumpAfterLandThreshold = 0.33f;
    [SerializeField] private float m_JumpFallFactor = 2f;
    [SerializeField] private float m_JumpSkipGroundCheckDuration = 0.5f;

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
}
