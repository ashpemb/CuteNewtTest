using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class OutputGrid
    {
        private Dictionary<int, HashSet<int>> indexPossiblePatternDictionary = new Dictionary<int, HashSet<int>>();
        public int Width {get;}
        public int Height {get;}
        private int maxNumberOfPatterns = 0;

        public OutputGrid(int width, int height, int numberOfPatterns)
        {
            Width = width;
            Height = height;
            maxNumberOfPatterns = numberOfPatterns;

            ResetAllPossibilities();
        }

        public void ResetAllPossibilities()
        {
            HashSet<int> allPossiblePatternList = new HashSet<int>();
            allPossiblePatternList.UnionWith(Enumerable.Range(0, maxNumberOfPatterns).ToList());
            
            indexPossiblePatternDictionary.Clear();
            for (int i = 0; i < Height * Width; i++)
            {
                indexPossiblePatternDictionary.Add(i, new HashSet<int>(allPossiblePatternList));
            }
        }

        public bool CheckCellExists(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            return indexPossiblePatternDictionary.ContainsKey(index);
        }

        private int GetIndexFromCoordinates(Vector2Int position)
        {
            return position.x + Width * position.y;
        }

        public bool CheckIfCellIsCollapsed(Vector2Int position)
        {
            return GetPossibleValuesForPosition(position).Count <= 1;
        }

        public HashSet<int> GetPossibleValuesForPosition(Vector2Int position)
        {
            int index = GetIndexFromCoordinates(position);
            if (indexPossiblePatternDictionary.ContainsKey(index))
            {
                return indexPossiblePatternDictionary[index];
            }

            return new HashSet<int>();
        }

        public bool CheckIfGridIsSolved()
        {
            return !indexPossiblePatternDictionary.Any(x => x.Value.Count > 1);
        }

        internal bool CheckIfValidPosition(Vector2Int position)
        {
            return MyCollectionExtension.ValidateCoordinates(position.x, position.y, Width, Height);
        }

        public Vector2Int GetRandomCell()
        {
            int randomIndex = Random.Range(0, indexPossiblePatternDictionary.Count);
            return GetCoordsFromIndex(randomIndex);
        }

        private Vector2Int GetCoordsFromIndex(int randomIndex)
        {
            Vector2Int coords = Vector2Int.zero;
            coords.x = randomIndex / Width;
            coords.y = randomIndex % Height;
            return coords;
        }

        public void SetPatternOnPosition(int x, int y, int patternIndex)
        {
            int index = GetIndexFromCoordinates(new Vector2Int(x, y));
            indexPossiblePatternDictionary[index] = new HashSet<int>() { patternIndex };
        }

        public int[][] GetSolvedOutputGrid()
        {
            int[][] returnGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(Height, Width);
            if (!CheckIfGridIsSolved())
            {
                return MyCollectionExtension.CreateJaggedArray<int[][]>(0, 0);
            }

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    int index = GetIndexFromCoordinates(new Vector2Int(col, row));
                    returnGrid[row][col] = indexPossiblePatternDictionary[index].First();
                }
            }

            return returnGrid;
        }

        public void PrintResultsToConsole()
        {
            StringBuilder builder = null;
            List<string> list = new List<string>();
            for (int row = 0; row < Height; row++)
            {
                builder = new StringBuilder();
                for (int col = 0; col < Width; col++)
                {
                    var result = GetPossibleValuesForPosition((new Vector2Int(col, row)));
                    if (result.Count == 1)
                    {
                        builder.Append(result.First() + " ");
                    }
                    else
                    {
                        string newString = "";
                        foreach (var item in result)
                        {
                            newString += item + ",";
                        }

                        builder.Append(newString);
                    }
                }
                list.Add(builder.ToString());
            }
            list.Reverse();
            foreach (var item in list)
            {
                Debug.Log(item);
            }
            Debug.Log("---");
        }
    }
}
