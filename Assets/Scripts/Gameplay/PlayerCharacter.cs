using System;
using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay {
    public class PlayerCharacter : Character
    {
        // define enum for move type
        private enum MoveType
        {
            FixedTime,
            FixedSpeed
        }
        // UI fields
        [SerializeField] private int playerCurrentHP = 100;
        [SerializeField] private int playerMaxHP = 100;
        
        
        [SerializeField] private SpriteRenderer circleSpriteRenderer;
        
        public SpriteRenderer CircleSpriteRenderer => circleSpriteRenderer;
        public int PlayerCurrentHp => playerCurrentHP;
        public int PlayerMaxHp => playerMaxHP;
    
        // movement fields
        [SerializeField] private MoveType moveType = MoveType.FixedSpeed;
        [Header("Only used for FixedSpeed move type")]
        [SerializeField] private float moveSpeed = 5f;
        [Header("Only used for FixedTime move type")]
        [SerializeField] private float moveTime = 1f;
    
        [SerializeField] private float attackRange = 1f;

        // animation fields
        private Animator _animator;
        private static readonly int IsWalking = Animator.StringToHash("walking");
        private static readonly int AttackTrigger = Animator.StringToHash("attack_trigger");
        
        // game fields
        private GameController _gameController;
        private Coroutine _moveCoroutine;

        private void Start() {
            _animator = GetComponent<Animator>();
            _gameController = FindObjectOfType<GameController>();
            Initialize();
        }

        private void Initialize() {
            Initiative = UnityEngine.Random.Range(1, 100);
        }

        private void Update() {
            if (_gameController.CurrentPlayer == this) {
                if(State == CharState.Exploration) {
                    ExplorationUpdate();
                } else if (State == CharState.Combat) {
                    CombatUpdate();
                }
            }
            else {
                if(State == CharState.Exploration) {
                    FollowCurrentPlayer();
                }
            }
        }

        private void ExplorationUpdate() {
            // handle interaction
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
            {
                // Raycast from the mouse position to find the position on a plane
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    var hitTag = hit.transform.gameObject.tag;
                    switch (hitTag) {
                        case "Ground":
                            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(PlayerMove(hit.point));
                            break;
                        case "Enemy":
                            Debug.Log("Enemy clicked");
                            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(PlayerAttack(hit.transform.gameObject));
                            break;
                        
                        default:
                            Debug.Log("Unknown tag clicked: " + hitTag);
                            break;
                    }
                }
            }
        }

        private void CombatUpdate() {
            // TODO: implement combat logic
            
            // TODO: callback Combat class to play next turn
        }
        
        private void FollowCurrentPlayer() {
            // TODO implement follow logic
        }

        public IEnumerator PlayerMove(Vector3 mousePos)
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

        public IEnumerator PlayerAttack(GameObject enemy = null)
        {
            // TODO: enemy size should be considered in the calculation
            // calculate the target location for player to move to
            Vector3 targetPos = enemy.transform.position - (enemy.transform.position - transform.position).normalized * attackRange;
            // move the player to the enemy position
            yield return StartCoroutine(PlayerMove(targetPos));
            _animator.SetTrigger(AttackTrigger);
            // TODO: implement attack logic
            enemy.GetComponent<EnemyCharacter>().GetHit(40f);
        }
    }
}
