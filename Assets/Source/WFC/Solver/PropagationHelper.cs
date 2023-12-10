using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PropagationHelper
    {
        private OutputGrid _outputGrid;
        private CoreHelper _coreHelper;
        private bool _cellWithNoSolutionPresent;
        private SortedSet<LowEntropyCell> _lowEntropySet = new SortedSet<LowEntropyCell>();
        private Queue<VectorPair> _pairsToPropagate = new Queue<VectorPair>();

        public SortedSet<LowEntropyCell> LowEntropySet => _lowEntropySet;

        public Queue<VectorPair> PairsToPropagate => _pairsToPropagate;

        public PropagationHelper(OutputGrid outputGrid, CoreHelper coreHelper)
        {
            _outputGrid = outputGrid;
            _coreHelper = coreHelper;
        }

        public bool CheckIfPairShouldBeProcessed(VectorPair propagationPair)
        {
            return _outputGrid.CheckIfValidPosition(propagationPair.CellToPropagatePosition) &&
                   !propagationPair.AreWeCheckingPreviousCellAgain();
        }

        public void AnalysePropagationResults(VectorPair propagationPair, int startCount, int newPossiblePatternCount)
        {
            if (newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagationPair.CellToPropagatePosition, propagationPair.BaseCellPosition);
                AddToLowEntropySet(propagationPair.CellToPropagatePosition);
            }

            if (newPossiblePatternCount == 0)
            {
                Debug.Log("Possible pattern count = 0 after propagation from" +propagationPair.BaseCellPosition+" to "+ propagationPair.CellToPropagatePosition +" \n Declaring Conflict");
                _cellWithNoSolutionPresent = true;
            }

            if (newPossiblePatternCount == 1)
            {
                _cellWithNoSolutionPresent =
                    _coreHelper.CheckCellSolutionForCollision(propagationPair.CellToPropagatePosition, _outputGrid);
                if (_cellWithNoSolutionPresent)
                {
                    Debug.Log("Collision after propagation from" +propagationPair.BaseCellPosition+" to "+ propagationPair.CellToPropagatePosition +" \n Declaring Conflict");
                }
            }
        }

        private void AddToLowEntropySet(Vector2Int inCell)
        {
            var elementOfLowEntropySet = _lowEntropySet.FirstOrDefault(x => x.Position == inCell);
            if (elementOfLowEntropySet == null && !_outputGrid.CheckIfCellIsCollapsed(inCell))
            {
                float entropy = _coreHelper.CalculateEntropy(inCell, _outputGrid);
                _lowEntropySet.Add(new LowEntropyCell(inCell, entropy));
            }
            else
            {
                if (elementOfLowEntropySet == null) return;
                _lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.Entropy = _coreHelper.CalculateEntropy(inCell, _outputGrid);
                _lowEntropySet.Add(elementOfLowEntropySet);
            }
        }

        public void AddNewPairsToPropagateQueue(Vector2Int cellToPropagatePosition, Vector2Int baseCellPosition)
        {
            var list = _coreHelper.Create4DirectionNeighbours(cellToPropagatePosition, baseCellPosition);
            foreach (var item in list)
            {
                _pairsToPropagate.Enqueue(item);
            }
        }

        public bool CheckForConflicts()
        {
            return _cellWithNoSolutionPresent;
        }

        public void SetConflictFlag()
        {
            _cellWithNoSolutionPresent = true;
        }

        public void EnqueueUncollapsedNeighbours(VectorPair propagatePair)
        {
            var uncollapsedNeighbours = _coreHelper.CheckIfNeighboursAreCollapsed(propagatePair, _outputGrid);
            foreach (var neighbour in uncollapsedNeighbours)
            {
                _pairsToPropagate.Enqueue(neighbour);
            }
        }
    }
}
