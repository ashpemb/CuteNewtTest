using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
    {
        public Vector2Int Position {
            get;
            set;
        }

        public float Entropy { get; set; }

        private float smallEntropyNoise;

        public LowEntropyCell(Vector2Int position, float entropy)
        {
            smallEntropyNoise = UnityEngine.Random.Range(0.001f, 0.005f);
            Position = position;
            Entropy = entropy;
        }
        
        public int CompareTo(LowEntropyCell other)
        {
            if (Entropy>other.Entropy)
            {
                return 1;
            }
            if (Entropy< other.Entropy)
            {
                return -1;
            }

            return 0;
        }

        public bool Equals(LowEntropyCell cell1, LowEntropyCell cell2)
        {
            return cell1.Position == cell2.Position;
        }

        public int GetHashCode(LowEntropyCell obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
