using System;
using UnityEngine;

namespace Gameplay {
    public abstract class Character : MonoBehaviour, IComparable<Character> {
        public enum CharState {
            Exploration,
            Combat
        }

        public CharState State { get; set; } = CharState.Exploration;

        private int _initiative;
        protected int Initiative { get;  set; }
        
        [SerializeField] private Sprite characterSprite;
        public Sprite CharacterSprite => characterSprite;
        public int CompareTo(Character other) {
            if (other == null)
                return 1;

            return Initiative.CompareTo(other.Initiative);
        }        
        
    }
}