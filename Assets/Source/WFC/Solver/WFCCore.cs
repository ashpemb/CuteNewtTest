using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class WFCCore
    {
        private OutputGrid _outputGrid;
        private PatternManager _patternManager;
        private int _maxIterations = 0;

        public WFCCore(int outputWidth, int outputHeight, int maxIterations, PatternManager patternManager)
        {
            _outputGrid = new OutputGrid(outputWidth, outputHeight, patternManager.GetNumberOfPatterns());
            _patternManager = patternManager;
            _maxIterations = maxIterations;
        }

        public int[][] CreateOutputGrid()
        {
            int iteration = 0;
            while (iteration < _maxIterations)
            {
                CoreSolver solver = new CoreSolver(_outputGrid, _patternManager);
                int innerIteration = 10000;
                while (!solver.CheckForConflicts() && !solver.CheckIfSolved())
                {
                    Vector2Int position = solver.GetLowestEntropyCell();
                    solver.CollapseCell(position);
                    solver.Propagate();
                    innerIteration--;
                    if (innerIteration <= 0)
                    {
                        Debug.Log("Propagation is taking too long");
                        return Array.Empty<int[]>();
                    }
                }

                if (solver.CheckForConflicts())
                {
                    Debug.Log("\n Conflict Occured in iteration: " + iteration);
                    iteration++;
                    _outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(_outputGrid, _patternManager);
                }
                else
                {
                    Debug.Log("solved on: " + iteration);
                    _outputGrid.PrintResultsToConsole();
                    break;
                }
            }

            if (iteration >= _maxIterations)
            {
                Debug.Log("Could not solve");
            }

            return _outputGrid.GetSolvedOutputGrid();
        }
    }
}