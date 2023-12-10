using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategySize2OrMore : IFindNeighbourStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult)
        {
            Dictionary<int, PatternNeighbours> results = new Dictionary<int, PatternNeighbours>();
            foreach (var patternData in patternFinderResult.PatternIndexDictionary)
            {
                foreach (var possibleNeighbourForPattern in patternFinderResult.PatternIndexDictionary)
                {
                    FindNeighboursInAllDirections(results, patternData, possibleNeighbourForPattern);
                }
            }

            return results;
        }

        private void FindNeighboursInAllDirections(Dictionary<int, PatternNeighbours> results, KeyValuePair<int, PatternData> patternData, KeyValuePair<int, PatternData> possibleNeighbourForPattern)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (patternData.Value.CompareGrid(dir, possibleNeighbourForPattern.Value))
                {
                    if (!results.ContainsKey(patternData.Key))
                    {
                        results.Add(patternData.Key, new PatternNeighbours());
                    }

                    results[patternData.Key].AddPatternToDictionary(dir, possibleNeighbourForPattern.Key);
                }
            }
        }
    }
}
