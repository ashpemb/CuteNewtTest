using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace WaveFunctionCollapse
{

    public class ValuesManager<T>
    {
        private int[][] _grid;
        private Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        private int index = 0;

        public ValuesManager(IValue<T>[][] gridOfValues)
        {
            CreateGridOfIndices(gridOfValues);
        }

        private void CreateGridOfIndices(IValue<T>[][] gridOfValues)
        {
            _grid = MyCollectionExtension.CreateJaggedArray<int[][]>(gridOfValues.Length, gridOfValues[0].Length);
            for (int row = 0; row < gridOfValues.Length; row++)
            {
                for (int col = 0; col < gridOfValues[0].Length; col++)
                {
                    SetIndexToGridPosition(gridOfValues, row, col);
                }
            }
        }

        private void SetIndexToGridPosition(IValue<T>[][] gridOfValues, int row, int col)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValues[row][col]))
            {
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[row][col]));
                _grid[row][col] = key.Key;
            }
            else
            {
                _grid[row][col] = index;
                valueIndexDictionary.Add(_grid[row][col], gridOfValues[row][col]);
                index++;
            }
        }

        public int GetGridValue(int x, int y)
        {
            if (x >= _grid[0].Length || y >= _grid.Length || y < 0 || x < 0)
            {
                throw new IndexOutOfRangeException("Grid doesn't contain " + x + "," + y + " value");
            }

            return _grid[y][x];
        }

        public IValue<T> GetValueFromIndex(int index)
        {
            if (valueIndexDictionary.ContainsKey(index))
            {
                return valueIndexDictionary[index];
            }

            throw new Exception("No index " + index);
        }
        
        public int GetGridValuesIncludingOffset(int x, int y)
        {
            int yMax = _grid.Length;
            int xMax = _grid[0].Length;
            
            if ((x %= xMax) < 0) x += xMax;
            if ((y %= yMax) < 0) y += yMax;
            return GetGridValue(x, y);
        }

        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            int[][] arrayToReturn = MyCollectionExtension.CreateJaggedArray<int[][]>(patternSize, patternSize);
            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    arrayToReturn[row][col] = GetGridValuesIncludingOffset(x + col, y + row);
                }
            }

            return arrayToReturn;
        }

        public Vector2 GetGridSize()
        {
            if (_grid != null)
            {
                return new Vector2(_grid[0].Length, _grid.Length);
            }

            return Vector2.zero;
        }
    }

}
