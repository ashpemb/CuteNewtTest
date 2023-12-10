using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

namespace WaveFunctionCollapse
{
    public class PatternNeighbours
    {
        public Dictionary<Direction, HashSet<int>> DirectionPatternNeighbourDictionary =
            new Dictionary<Direction, HashSet<int>>();

        public void AddPatternToDictionary(Direction dir, int patternindex)
        {
            if (DirectionPatternNeighbourDictionary.ContainsKey(dir))
            {
                DirectionPatternNeighbourDictionary[dir].Add(patternindex);
            }
            else
            {
                DirectionPatternNeighbourDictionary.Add(dir, new HashSet<int>() { patternindex });
            }
        }
        
        public HashSet<int> GetNeighboursInDirection(Direction dir)
        {
            if (DirectionPatternNeighbourDictionary.ContainsKey(dir))
            {
                return DirectionPatternNeighbourDictionary[dir];
            }

            return new HashSet<int>();
        }

        public void AddNeighbour(PatternNeighbours neighbours)
        {
            foreach (var item in neighbours.DirectionPatternNeighbourDictionary)
            {
                if (!DirectionPatternNeighbourDictionary.ContainsKey(item.Key))
                {
                    DirectionPatternNeighbourDictionary.Add(item.Key, new HashSet<int>());
                }

                DirectionPatternNeighbourDictionary[item.Key].UnionWith(item.Value);
                
            }
        }
    }
}
