using UnityEngine;

namespace Gameplay {
    public class EnemyStatus : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private Animator _animator;
        private static readonly int HitTrigger = Animator.StringToHash("hit_trigger");
        private static readonly int DeathTrigger = Animator.StringToHash("death_trigger");

        private void Start() {
            _animator = GetComponent<Animator>();
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
