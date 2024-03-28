using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Image characterSprite;
        [SerializeField] private TextMeshProUGUI healthText;
        
        public void UpdateUI(PlayerController currentPlayer) {
            healthText.text = $"HP: {currentPlayer.PlayerCurrentHp}/{currentPlayer.PlayerMaxHp}";
            characterSprite.sprite = currentPlayer.CharacterSprite;
            var cam = Camera.main;
            cam.gameObject.GetComponent<CameraController>().CameraTarget = currentPlayer.gameObject.transform;
        }
    
    }
}
