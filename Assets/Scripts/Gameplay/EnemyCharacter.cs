using System;
using Controllers;
using UnityEngine;

namespace Gameplay {
    public class EnemyCharacter : Character
    {
        [SerializeField] private float health = 100f;
        [SerializeField] private Material circleMaterial;
        [SerializeField] private SpriteRenderer circleSpriteRenderer;
        [SerializeField] private float aggroRange = 5f;

        private Animator _animator;
        private static readonly int HitTrigger = Animator.StringToHash("hit_trigger");
        private static readonly int DeathTrigger = Animator.StringToHash("death_trigger");
        private GameController _gameController;

        private void Start() {
            _animator = GetComponent<Animator>();
            circleSpriteRenderer.material = circleMaterial;
            _gameController = FindObjectOfType<GameController>();
            Initialize();
        }

        private void Initialize() {
            Initiative = UnityEngine.Random.Range(1, 100);
        }

        private void Update() {
            // check if there is a player nearby
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange);

            // Iterate through the colliders
            foreach (Collider collider in colliders)
            {
                // Do something with the collider, e.g., check its tag or interact with it
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("Player detected!");
                    _gameController.GameState = GameState.Combat;
                }
            }
        }

        public void GetHit(float damage) {
            _animator.SetTrigger(HitTrigger);
            health -= damage;
            if (health <= 0) {
                _animator.SetTrigger(DeathTrigger);
            }
        
        }
    
        public void DestroyEnemy() {
            Destroy(gameObject);
        }
    
    }
}
