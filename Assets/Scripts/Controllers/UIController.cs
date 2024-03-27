using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image characterSprite;
        [SerializeField] private TextMeshProUGUI healthText;
    
        private Coroutine _moveCoroutine;
        private PlayerController _currentPlayer;

        private void Start() {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) {
                Debug.Log("Player not found!");
                return;
            }
            _currentPlayer = player.GetComponent<PlayerController>();
            UpdateUI();
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
                    var hitTag = hit.transform.gameObject.tag;
                    switch (hitTag) {
                        case "Player":
                            _currentPlayer = hit.transform.gameObject.GetComponent<PlayerController>();
                            UpdateUI();
                            break;
                        case "Ground":
                            if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(_currentPlayer.PlayerMove(hit.point));
                            break;
                        case "Enemy":
                            Debug.Log("Enemy clicked");
                            if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(_currentPlayer.PlayerAttack(hit.transform.gameObject));
                            break;
                        default:
                            Debug.Log("Unknown tag clicked: " + hitTag);
                            break;
                    }
                }
            }
        }

        private void UpdateUI() {
            healthText.text = $"HP: {_currentPlayer.PlayerCurrentHp}/{_currentPlayer.PlayerMaxHp}";
            characterSprite.sprite = _currentPlayer.CharacterSprite;
        }
    
    }
}
