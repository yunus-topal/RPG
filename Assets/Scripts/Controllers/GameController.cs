using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public enum GameState {
        Combat,
        Exploration
    }
    public class GameController : MonoBehaviour
    {
        //private Coroutine _moveCoroutine;
        private GameObject[] _players;
        private PlayerCharacter _currentPlayer;
        public PlayerCharacter CurrentPlayer => _currentPlayer;
        private UIController _uiController;
        private List<Combat> _combats = new List<Combat>();

        [Header("UI Elements")]
        [SerializeField] private GameObject portraitPrefab;
        [SerializeField] private Material selectedCircleMaterial;
        [SerializeField] private Material deselectedCircleMaterial;
        
        [Header("Game Values")]
        [SerializeField] private float aggroRange = 5f;
        private void Start() {
            _players = GameObject.FindGameObjectsWithTag("Player");
            if(_players.Length == 0) {
                Debug.LogError("No players found!");
                return;
            }
            _currentPlayer = _players[0].GetComponent<PlayerCharacter>();
            _uiController = FindObjectOfType<UIController>();
            _uiController.UpdateHUD(_currentPlayer);
            CreatePortraitsAndCircles();
            UpdateCircles();
        }

        public void StartCombat(Character character) {
            Debug.Log("Combat started by " + character.name);
            Combat newCombat = new Combat();
            // Get all characters involved in combat by overlapping sphere
            Collider[] colliders = Physics.OverlapSphere(character.transform.position, aggroRange);
            List<Character> characters = new List<Character>();
            foreach (var c in colliders) {
                if (c.gameObject.CompareTag("Player")) {
                    var playerComponent = c.gameObject.GetComponent<PlayerCharacter>();
                    characters.Add(playerComponent);
                    playerComponent.State = Character.CharState.Combat;
                } else if (c.gameObject.CompareTag("Enemy")) {
                    var enemyComponent = c.gameObject.GetComponent<EnemyCharacter>();
                    characters.Add(enemyComponent);
                    enemyComponent.State = Character.CharState.Combat;
                }
            }
            Debug.Log("number of characters in combat: " + characters.Count);
            newCombat.Initialize(characters);
            CreateCombatPortraits(newCombat);
        }
    
        // Update is called once per frame
        void Update()
        {
            // handle interaction
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
            {
                // Raycast from the mouse position to find the position on a plane
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    var hitTag = hit.transform.gameObject.tag;
                    switch (hitTag) {
                        case "Player":
                            var oldPlayer = _currentPlayer;
                            _currentPlayer = hit.transform.gameObject.GetComponent<PlayerCharacter>();
                            _uiController.UpdateHUD(_currentPlayer);
                            UpdateCircles();
                            break;
                        /*
                        case "Ground":
                            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(_currentPlayer.PlayerMove(hit.point));
                            break;
                        case "Enemy":
                            Debug.Log("Enemy clicked");
                            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
                            _moveCoroutine = StartCoroutine(_currentPlayer.PlayerAttack(hit.transform.gameObject));
                            break;
                        */
                        default:
                            Debug.Log("Unknown tag clicked: " + hitTag);
                            break;
                    }
                }
            }
        }
        
        void UpdateCircles() {
            foreach (var playerObject in _players) {
                playerObject.GetComponent<PlayerCharacter>().CircleSpriteRenderer.material = deselectedCircleMaterial;
            }
            _currentPlayer.CircleSpriteRenderer.material = selectedCircleMaterial;
        }

        void CreatePortraitsAndCircles() {
            // create portrait for each player object
            foreach (var playerObject in _players) {
                // create portrait for each player object
                var playerController = playerObject.GetComponent<PlayerCharacter>();
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

        void CreateCombatPortraits(Combat combat){
            // create portrait for each player object
            foreach (var character in combat.Characters) {
                var portrait = Instantiate(portraitPrefab, Vector3.zero, Quaternion.identity);
                portrait.transform.SetParent(GameObject.Find("CombatPortraits").transform);
                portrait.GetComponent<Image>().sprite = character.CharacterSprite;
                
                if (character is PlayerCharacter) {
                    Debug.Log("Player character found in combat.");
                    // set portrait image and button click event
                    portrait.GetComponent<Button>().onClick.AddListener(() => {
                        _currentPlayer = character as PlayerCharacter;
                        _uiController.UpdateHUD(_currentPlayer);
                        UpdateCircles();
                    });
                }else if(character is EnemyCharacter) {
                    Debug.Log("Enemy character found in combat.");
                    // TODO: add move camera to enemy character on click function
                }
            }
        }
        
    }
}
