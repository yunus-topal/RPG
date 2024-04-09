﻿using System;
using UnityEngine;

namespace Gameplay {
    public abstract class Character : MonoBehaviour, IComparable<Character> {
        protected enum CharState {
            Exploration,
            Combat
        }
        
        private CharState _state = CharState.Exploration;
        protected CharState State { get; set; }
        private int _initiative;
        protected int Initiative { get;  set; }
        
        public int CompareTo(Character other) {
            if (other == null)
                return 1;

            return Initiative.CompareTo(other.Initiative);
        }        
        
    }
}