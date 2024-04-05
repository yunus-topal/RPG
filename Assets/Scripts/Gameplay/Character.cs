using System;
using UnityEngine;

namespace Gameplay {
    public abstract class Character : MonoBehaviour, IComparable<Character> {
        private int _initiative;
        protected int Initiative { get;  set; }
        
        public int CompareTo(Character other) {
            if (other == null)
                return 1;

            return Initiative.CompareTo(other.Initiative);
        }        
        
    }
}