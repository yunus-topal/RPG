using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // define enum for move type
    public enum MoveType
    {
        FixedTime,
        FixedSpeed
    }
    
    private Coroutine _moveCoroutine;
    
    [SerializeField] private MoveType moveType = MoveType.FixedSpeed;
    [Header("Only used for FixedSpeed move type")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Only used for FixedTime move type")]
    [SerializeField] private float moveTime = 1f;

    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("walking");

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            // Raycast from the mouse position to find the position on a plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                _moveCoroutine = StartCoroutine(MovePlayer(hit.point));
            }
        }
    }
    
    
    private IEnumerator MovePlayer(Vector3 mousePos)
    {
        _animator.SetBool(IsWalking, true);
        Vector3 distance = new Vector3(mousePos.x, 0f, mousePos.z) - new Vector3(transform.position.x, 0f, transform.position.z);
        // rotate the player to face the mouse position
        transform.rotation = Quaternion.LookRotation(distance);

        if (moveType == MoveType.FixedSpeed) {
            // move the player to the mouse position
            while (distance.magnitude > 0.1f)
            {
                transform.position += distance.normalized * (moveSpeed * Time.deltaTime);
                distance = new Vector3(mousePos.x, 0f, mousePos.z) - transform.position;
                distance.y = 0;
                yield return null;
            }
        } else if (moveType == MoveType.FixedTime) {
            for(float f = 0; f < moveTime; f += Time.deltaTime)
            {
                transform.position += (distance / moveTime) * Time.deltaTime;
                yield return null;
            }
        }
        _animator.SetBool(IsWalking, false);
    }
}
