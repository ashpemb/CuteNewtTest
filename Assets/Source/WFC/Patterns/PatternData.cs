using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PatternData
    {
        private Pattern _pattern;
        private int _frequency = 1;
        private float _frequencyRelative;
        private float _frequencyRelativeLog2;


        public Pattern Pattern
        {
            get => _pattern;
        }

        public float FrequencyRelative
        {
            get => _frequencyRelative;
        }

        public float FrequencyRelativeLog2
        {
            get => _frequencyRelativeLog2;
        }

        public PatternData(Pattern inPattern)
        {
            _pattern = inPattern;
        }

        public void AddToFrequency()
        {
            _frequency++;
        }

        public void CalculateRelativeFrequency(int total)
        {
            _frequencyRelative = (float)_frequency / total;
            _frequencyRelativeLog2 = MathF.Log(_frequencyRelative, 2);
        }

        public bool CompareGrid(Direction dir, PatternData data)
        {
            return _pattern.ComparePatternToAnotherPattern(dir, data.Pattern);
        }
    }
}
