using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {
    public class Combat : MonoBehaviour
    {
        private List<Character> _characters = new();
        // turn queue, first in first out. Add items to the last index.
        private List<Character> _turnQueue = new();

        private void Initialize(List<Character> characters) {
            _characters = characters;
            characters.Sort();
            
            // add everyone twice to the turn queue
            _turnQueue.Clear();
            _turnQueue.AddRange(_characters);
            _turnQueue.AddRange(_characters);
        }
        
        public void PlayNextTurn() {
            if (_turnQueue.Count == 0) return;
            
            Character currentCharacter = _turnQueue[0];
            if (currentCharacter is PlayerCharacter playerCharacter) {
                // player turn
                Debug.Log("Call player turn logic");
            } else if (currentCharacter is EnemyCharacter enemyCharacter) {
                // TODO: call enemy turn logic
                Debug.Log("Call enemy turn logic");
            }
            
            if(_turnQueue.Count <= _characters.Count) {
                _turnQueue.AddRange(_characters);
            }
        }

        private void CharacterLeft(Character character) {
            _characters.Remove(character);
            // leftover turns should stay, but dead character should be removed.
            _turnQueue.RemoveAll(c => c == character);
        }

        private void CharacterJoined(Character character) {
            int leftovers = _turnQueue.Count % _characters.Count;
            _characters.Add(character);
            _characters.Sort();
            
            // find the insert index.
            int index = _characters.IndexOf(character);
            _turnQueue.Insert(leftovers + index,character);
        }

        // TODO: Implement this method
        private void MergeCombats(Combat otherCombat) {
            
        }
    }
}
