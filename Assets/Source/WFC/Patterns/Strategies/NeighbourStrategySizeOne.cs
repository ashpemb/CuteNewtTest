using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategySize1 : IFindNeighbourStrategy
    {
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult)
        {
            Dictionary<int, PatternNeighbours> results = new Dictionary<int, PatternNeighbours>();
            FindNeighboursForEachPattern(patternFinderResult, results);
            return results;
        }

        private void FindNeighboursForEachPattern(PatternDataResults patternFinderResult, Dictionary<int, PatternNeighbours> results)
        {
            for (int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                for (int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    PatternNeighbours neighbours =
                        PatternFinder.CheckNeighboursInEachDirection(col, row, patternFinderResult);
                    PatternFinder.AddNeighboursToDictionary(results, patternFinderResult.GetIndexAt(col,row), neighbours);
                }
            }
        }
    }
}
