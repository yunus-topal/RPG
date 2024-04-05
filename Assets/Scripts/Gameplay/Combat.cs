using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    public class Combat : MonoBehaviour
    {
        private List<Character> _characters = new();
        private Queue<Character> _turnQueue = new();

        private void Initialize(List<Character> characters) {
            this._characters = characters;
            characters.Sort();
            // add everyone twice to the turn queue
            EnqueueCharacters();
            EnqueueCharacters();
        }
        
        private void PlayNextTurn() {
            if (_turnQueue.Count == 0) return;
            
            Character currentCharacter = _turnQueue.Dequeue();
            if (currentCharacter is PlayerCharacter playerCharacter) {
                // player turn
                Debug.Log("Player's turn");
            } else if (currentCharacter is EnemyCharacter enemyCharacter) {
                // enemy turn
                Debug.Log("Enemy's turn");
            }
            
            if(_turnQueue.Count <= _characters.Count) {
                EnqueueCharacters();
            }
        }
        
        private void EnqueueCharacters() {
            foreach (Character character in _characters) {
                _turnQueue.Enqueue(character);
            }
        }

        private void CharacterLeft(Character character) {
            _characters.Remove(character);
            // leftover turns should stay, but dead character should be removed.
            Queue<Character> newQueue = new Queue<Character>();
            for(int i = 0; i < _turnQueue.Count; i++) {
                Character poppedChar = _turnQueue.Dequeue();
                if (poppedChar != character) {
                    newQueue.Enqueue(poppedChar);
                }
            }
            _turnQueue = newQueue;
        }

        private void CharacterJoined(Character character) {
            int leftovers = _turnQueue.Count % _characters.Count;
            
            Queue<Character> newQueue = new Queue<Character>();
            for(int i = 0; i < leftovers; i++) {
                Character poppedChar = _turnQueue.Dequeue();
                newQueue.Enqueue(poppedChar);
            }
            
            _characters.Add(character);
            _characters.Sort();
            EnqueueCharacters();
        }

        // TODO: Implement this method
        private void MergeCombats(Combat otherCombat) {
            
        }
    }
}
