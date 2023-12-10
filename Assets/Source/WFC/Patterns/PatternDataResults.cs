using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Helpers;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PatternDataResults
    {
        private int[][] _patternIndicesGrid;
        public Dictionary<int, PatternData> PatternIndexDictionary { get; private set; }
        public PatternDataResults(int[][] patternIndicesGrid, Dictionary<int, PatternData> patternIndexDictionary)
        {
            _patternIndicesGrid = patternIndicesGrid;
            PatternIndexDictionary = patternIndexDictionary;
        }

        public int GetGridLengthX()
        {
            return _patternIndicesGrid[0].Length;
        }
        
        public int GetGridLengthY()
        {
            return _patternIndicesGrid.Length;
        }

        public int GetIndexAt(int x, int y)
        {
            return _patternIndicesGrid[y][x];
        }

        public int GetNeighbourInDirection(int x, int y, Direction dir)
        {
            if (!_patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y))
            {
                return -1;
            }

            switch (dir)
            {
                case Direction.UP:
                    if (_patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y+1))
                    {
                        return GetIndexAt(x, y + 1);
                    }
                    break;
                case Direction.DOWN:
                    if (_patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x,y-1))
                    {
                        return GetIndexAt(x, y - 1);
                    }
                    break;
                case Direction.LEFT:
                    if (_patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x-1,y))
                    {
                        return GetIndexAt(x-1, y);
                    }
                    break;
                case Direction.RIGHT:
                    if (_patternIndicesGrid.CheckJaggedArray2IfIndexIsValid(x+1,y))
                    {
                        return GetIndexAt(x+1, y);
                    }
                    break;
                default:
                    return -1;
            }

            return -1;
        }
    }
}
