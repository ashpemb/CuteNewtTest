using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WaveFunctionCollapse
{
    public class CoreHelper
    {
        private float _totalFrequency = 0;
        private float _totalFrequencyLog = 0;
        private PatternManager _patternManager;

        public CoreHelper(PatternManager _inPatternManager)
        {
            _patternManager = _inPatternManager;

            for (int i = 0; i < _patternManager.GetNumberOfPatterns(); i++)
            {
                _totalFrequency += _patternManager.GetPatternFrequency(i);
            }

            _totalFrequencyLog = MathF.Log(_totalFrequency, 2);
        }

        public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
        {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromFrequencies(possibleValues);
            float randomValue = Random.Range(0, valueFrequenciesFractions.Sum());
            float sum = 0;
            int index = 0;
            foreach (var item in valueFrequenciesFractions)
            {
                sum += item;
                if (randomValue <= sum)
                {
                    return index;
                }

                index++;
            }

            return index - 1;
        }

        private List<float> GetListOfWeightsFromFrequencies(List<int> possibleValues)
        {
            // return possibleValues.Select(_patternManager.GetPatternFrequency).ToList();
            var valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, val) =>
                {
                    acc.Add(_patternManager.GetPatternFrequency(val));
                    return acc;
                },
                acc => acc).ToList();
            return valueFrequencies;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            List<VectorPair> list = new List<VectorPair>()
            {
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(1, 0), previousCell, Direction.RIGHT),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(-1, 0), previousCell, Direction.LEFT),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, 1), previousCell, Direction.UP),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, -1), previousCell, Direction.DOWN),
            };
            return list;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates)
        {
            return Create4DirectionNeighbours(cellCoordinates, cellCoordinates);
        }

        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid)
        {
            float sum = 0;
            foreach (var possibleIndex in outputGrid.GetPossibleValuesForPosition(position))
            {
                sum += _patternManager.GetPatternFrequencyLog2(possibleIndex);
            }

            return _totalFrequencyLog - (sum / _totalFrequency);
        }

        public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return Create4DirectionNeighbours(pairToCheck.CellToPropagatePosition, pairToCheck.BaseCellPosition)
                .Where(x => outputGrid.CheckIfValidPosition(x.CellToPropagatePosition) &&
                            !outputGrid.CheckIfCellIsCollapsed(x.CellToPropagatePosition))
                .ToList();
        }

        public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid)
        {
            foreach (var neighbour in Create4DirectionNeighbours(cellCoordinates))
            {
                if (!outputGrid.CheckIfValidPosition(neighbour.CellToPropagatePosition))
                {
                    continue;
                }

                HashSet<int> possibleIndices = new HashSet<int>();
                foreach (var patternIndexAtNeighbour in outputGrid.GetPossibleValuesForPosition(neighbour.CellToPropagatePosition))
                {
                    var possibleNeighboursForBase =
                        _patternManager.GetPossibleNeighboursForPatternInDirection(patternIndexAtNeighbour,
                            neighbour.DirectionFromBase.GetOppositeDirectionTo());
                    possibleIndices.UnionWith(possibleNeighboursForBase);
                }
//if no possible indices exists in both hash sets then we have a collision
                if (!possibleIndices.Contains(outputGrid.GetPossibleValuesForPosition(cellCoordinates).First()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}