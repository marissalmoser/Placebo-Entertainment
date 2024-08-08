using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Oscillator
    {
        public Oscillator(float frequency)
        {
            CurrentFrequency = frequency;
        }
        public bool Wrapped { get; private set; }
        public float CurrentPhase { get; private set; }
        [field: SerializeField] public float CurrentFrequency { get; private set; } = 1f;

        public void Advance(float amt)
        {
            Wrapped = false;
            CurrentPhase += amt * CurrentFrequency;
            if (CurrentPhase > Mathf.PI * 2f)
            {
                Wrapped = true;
                CurrentPhase = 0f;
            }
        }

        public float Evaluate()
        {
            return Mathf.Sin(CurrentPhase);
        }
    }
}