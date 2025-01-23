using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Match3.Component
{
    public struct GridPositionComponent(Point gridPosition)
    {
        public Point GridPosition { get; set; } = gridPosition;
    }
}