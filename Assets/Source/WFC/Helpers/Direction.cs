using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public static class DirectionHelper
    {
        public static Direction GetOppositeDirectionTo(this Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.LEFT;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}