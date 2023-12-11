using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class CoreSolver
    {
        private PatternManager _patternManager;
        private OutputGrid _outputGrid;
        private CoreHelper _coreHelper;
        private PropagationHelper _propagationHelper;

        public CoreSolver(OutputGrid outputGrid, PatternManager patternManager)
        {
            _outputGrid = outputGrid;
            _patternManager = patternManager;
            _coreHelper = new CoreHelper(patternManager);
            _propagationHelper = new PropagationHelper(outputGrid, _coreHelper);
        }

        public void Propagate()
        {
            bool canQueueForPropagation = true;
            //Algorithm can get stuck here, endlessly adding to PairsToPropagate
            while (_propagationHelper.PairsToPropagate.Count > 0)
            {
                var propagatePair = _propagationHelper.PairsToPropagate.Dequeue();
                if (_propagationHelper.CheckIfPairShouldBeProcessed(propagatePair))
                {
                    ProcessCell(propagatePair, canQueueForPropagation);
                }

                if (_propagationHelper.CheckForConflicts() || _outputGrid.CheckIfGridIsSolved())
                {
                    return;
                }

                //stop gap to prevent endless queueing
                if (_propagationHelper.PairsToPropagate.Count > 10000)
                {
                    // Debug.Log("Propagation might be going infinite, breaking out");
                    // return;
                    canQueueForPropagation = false;
                }
            }
            
            if (_propagationHelper.PairsToPropagate.Count == 0 && _propagationHelper.LowEntropySet.Count == 0)
            {
                _propagationHelper.SetConflictFlag();
                Debug.Log("No Pairs to Propagate and No low entropy cells to collapse \n Declaring Conflict");
            }
        }

        private void ProcessCell(VectorPair propagatePair, bool canQueueForPropagation)
        {
            //Lower conflict rate but can become an infinite loop
            if (_outputGrid.CheckIfCellIsCollapsed(propagatePair.CellToPropagatePosition))
            {
                if (canQueueForPropagation)
                {
                    _propagationHelper.EnqueueUncollapsedNeighbours(propagatePair);
                }
            }
            else
            {
                PropagateNeighbour(propagatePair);
            }
            
            //causes high conflict rate but prevents infinite propagation queue
            // if (!_outputGrid.CheckIfCellIsCollapsed(propagatePair.BaseCellPosition))
            // {
            //     CollapseCell(propagatePair.BaseCellPosition);
            //     _propagationHelper.EnqueueUncollapsedNeighbours(propagatePair);
            // }
            // else
            // {
            //     PropagateNeighbour(propagatePair);
            // }
        }

        private void PropagateNeighbour(VectorPair propagatePair)
        {
            var possibleValuesAtNeighbour =
                _outputGrid.GetPossibleValuesForPosition(propagatePair.CellToPropagatePosition);
            int startCount = possibleValuesAtNeighbour.Count;
            RemoveImpossibleNeighbours(propagatePair, possibleValuesAtNeighbour);

            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;
            _propagationHelper.AnalysePropagationResults(propagatePair, startCount, newPossiblePatternCount);
        }

        private void RemoveImpossibleNeighbours(VectorPair propagatePair, HashSet<int> possibleValuesAtNeighbour)
        {
            HashSet<int> possibleIndices = new HashSet<int>();

            foreach (var patternIndexAtBase in _outputGrid.GetPossibleValuesForPosition(propagatePair.BaseCellPosition))
            {
                var possibleNeighboursForBase =
                    _patternManager.GetPossibleNeighboursForPatternInDirection(patternIndexAtBase,
                        propagatePair.DirectionFromBase);
                possibleIndices.UnionWith(possibleNeighboursForBase);
            }
            
            possibleValuesAtNeighbour.IntersectWith(possibleIndices);
        }

        public Vector2Int GetLowestEntropyCell()
        {
            if (_propagationHelper.LowEntropySet.Count <= 0)
            {
                return _outputGrid.GetRandomCell();
            }
            else
            {
                var lowestEntropyElement = _propagationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropyElement.Position;
                _propagationHelper.LowEntropySet.Remove(lowestEntropyElement);
                return returnVector;
            }
        }

        public void CollapseCell(Vector2Int cellCoords)
        {
            var possibleValue = _outputGrid.GetPossibleValuesForPosition(cellCoords).ToList();
            if (possibleValue.Count == 0 || possibleValue.Count == 1)
            {
                return;
            }

            int index = _coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
            _outputGrid.SetPatternOnPosition(cellCoords.x, cellCoords.y, possibleValue[index]);

            if (!_coreHelper.CheckCellSolutionForCollision(cellCoords, _outputGrid))
            {
                _propagationHelper.AddNewPairsToPropagateQueue(cellCoords, cellCoords);
            }
            else
            {
                Debug.Log("Collision in" + cellCoords.ToString() +" \n Declaring Conflict");
                _propagationHelper.SetConflictFlag();
            }
        }

        public bool CheckIfSolved()
        {
            return _outputGrid.CheckIfGridIsSolved();
        }

        public bool CheckForConflicts()
        {
            return _propagationHelper.CheckForConflicts();
        }
    }
}
