using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class GameController : MonoBehaviour
    {
        private Coroutine _moveCoroutine;
        private GameObject[] _players;
        private PlayerController _currentPlayer;
        private UIController _uiController;
        [SerializeField] private GameObject portraitPrefab;
        [SerializeField] private Material selectedCircleMaterial;
        [SerializeField] private Material deselectedCircleMaterial;
        private void Start() {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) {
                Debug.Log("Player not found!");
                return;
            }
            _players = GameObject.FindGameObjectsWithTag("Player");
            if(_players.Length == 0) {
                Debug.LogError("No players found!");
                return;
            }
            _currentPlayer = _players[0].GetComponent<PlayerController>();
            _uiController = FindObjectOfType<UIController>();
            _uiController.UpdateHUD(_currentPlayer);
            CreatePortraitsAndCircles();
            UpdateCircles();
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
                            var oldPlayer = _currentPlayer;
                            _currentPlayer = hit.transform.gameObject.GetComponent<PlayerController>();
                            _uiController.UpdateHUD(_currentPlayer);
                            UpdateCircles();
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
        
        void UpdateCircles() {
            foreach (var playerObject in _players) {
                playerObject.GetComponent<PlayerController>().CircleSpriteRenderer.material = deselectedCircleMaterial;
            }
            _currentPlayer.CircleSpriteRenderer.material = selectedCircleMaterial;
        }

        void CreatePortraitsAndCircles() {
            // create portrait for each player object
            foreach (var playerObject in _players) {
                // create portrait for each player object
                var playerController = playerObject.GetComponent<PlayerController>();
                var portrait = Instantiate(portraitPrefab, Vector3.zero, Quaternion.identity);
                portrait.transform.SetParent(GameObject.Find("PartyPortraits").transform);
                
                // set portrait image and button click event
                portrait.GetComponent<Image>().sprite = playerController.CharacterSprite;
                portrait.GetComponent<Button>().onClick.AddListener(() => {
                    _currentPlayer = playerController;
                    _uiController.UpdateHUD(_currentPlayer);
                    UpdateCircles();
                });
                // set circles,
                playerController.CircleSpriteRenderer.material = deselectedCircleMaterial;
            }
        }
    }
}
