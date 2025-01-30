using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Frith;
using Frith.Components;
using Frith.Extensions;
using Frith.Utilities;
using Match3.Component;
using Microsoft.Xna.Framework;

namespace Match3
{
    public class JewelGrid(Game game)
    {
        public Entity[,] Grid { get; set; }

    
        private readonly Game game = game;


        public Point VectorToGridPoint(Vector2 vector)
        {
            return new Point((int)vector.X / 16, (int)vector.Y / 16);
        }

        public Vector2 GridPointToVector(Point position)
        {
            var vector = new Vector2(position.X, position.Y) * new Vector2(16, 16);
            return vector;
        }

        // public Vector2 RectangleToPoint(Rectangle boundingBox)
        // {
            
        // }

        

        private void ChangePosition(Entity jewel, Point adjacentPosition, Point destinationPoint)
        {
            ref var jewelTransform = ref jewel.GetComponent<TransformComponent>();
            jewelTransform.LocalPosition += new Vector2(adjacentPosition.X * 16, adjacentPosition.Y * 16);

            Grid[destinationPoint.X, destinationPoint.Y] = jewel;

            ref var jewelGridComponent = ref jewel.GetComponent<GridPositionComponent>();
            jewelGridComponent.GridPosition = new Point(destinationPoint.X, destinationPoint.Y);

        }

        private bool PositionCheck(int x, int y, Point pos2, Point pos3)
        {
            return Grid[x, y]?.GetSpriteFrameIndex(game) == Grid[x + pos2.X, y + pos2.Y]?.GetSpriteFrameIndex(game) &&
                    Grid[x + pos2.X, y + pos2.Y]?.GetSpriteFrameIndex(game) == Grid[x + pos3.X, y + pos3.Y]?.GetSpriteFrameIndex(game);          
        }

        public bool MatchOpportunityCheck(int x, int y)
        {
            if (x > 0 && y > 1 && PositionCheck(x, y, new Point(-1, -1), new Point(-1, -2)) ||
                x < 7 && y > 1 && PositionCheck(x, y, new Point(1, -1), new Point(1, -2)) ||
                x > 0 && y < 6 && PositionCheck(x, y, new Point(-1, 1), new Point(-1, 2)) ||
                x < 7 && y < 6 && PositionCheck(x, y, new Point(1, 1), new Point(1, 2)) ||
                x > 1 && y > 0 && PositionCheck(x, y, new Point(-1, -1), new Point(-2, -1)) ||
                x < 6 && y > 0 && PositionCheck(x, y, new Point(1, -1), new Point(2, -1)) ||
                x > 1 && y < 7 && PositionCheck(x, y, new Point(-1, 1), new Point(-2, 1)) ||
                x < 6 && y < 7 && PositionCheck(x, y, new Point(1, 1), new Point(2, 1)) ||
                x > 0 && x < 7 && y < 7 && PositionCheck(x, y, new Point(-1, 1), new Point(1, 1)) ||
                x > 0 && x < 7 && y > 0 && PositionCheck(x, y, new Point(-1, -1), new Point(1, -1)) ||
                x < 7 && y > 0 && y < 7 && PositionCheck(x, y, new Point(1, -1), new Point(1, 1)) ||
                x > 0 && y > 0 && y < 7 && PositionCheck(x, y, new Point(-1, -1), new Point(-1, 1)) ||
                x < 5 && PositionCheck(x, y, new Point(2, 0), new Point(3, 0)) ||
                x > 2 && PositionCheck(x, y, new Point(-2, 0), new Point(-3, 0)) ||
                y > 2 && PositionCheck(x, y, new Point(0, -2), new Point(0, -3)) ||
                y < 5 && PositionCheck(x, y, new Point(0, 2), new Point(0, 3)))
                {
                    return true;
                }

                return false;
        }

        public bool MatchCheck(int x, int y)
        {

            if ((y < 6 && PositionCheck(x, y, new Point(0, 1), new Point(0, 2))) ||
                (y > 1 && PositionCheck(x, y, new Point(0, -1), new Point(0, -2))) ||
                (x < 6 && PositionCheck(x, y, new Point(1, 0), new Point(2, 0))) ||
                (x > 1 && PositionCheck(x, y, new Point(-1, 0), new Point(-2, 0))) ||
                (x > 0 && x < 6 && PositionCheck(x, y, new Point(-1, 0), new Point(1, 0))) ||
                (y > 0 && y < 6 && PositionCheck(x, y, new Point(0, -1), new Point(0, 1))))
            {
                return true;
            }

            return false;
        }

        public void EliminateMatches()
        {
            var checks = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (MatchCheck(x, y))
                    {
                        Grid[x, y].SetSpriteFrame(MathUtils.RandomInt(6), game);
                        
                        if (x > 0)
                            x--;

                        if (y > 0)
                            y--;

                        checks++;
                    }
                }
            }

            Logger.Info($"Matches eliminated: {checks}");
        }

        public void SwapJewels(Entity jewelA, Entity jewelB)
        {
            var jewelAPoint = VectorToGridPoint(jewelA.GetComponent<TransformComponent>().LocalPosition);
            var jewelBPoint = VectorToGridPoint(jewelB.GetComponent<TransformComponent>().LocalPosition);

            var jewelAPositionChange = jewelBPoint - jewelAPoint;
            var jewelBPositionChange = jewelAPoint - jewelBPoint;
            

            ChangePosition(jewelA, jewelAPositionChange, jewelBPoint);
            ChangePosition(jewelB, jewelBPositionChange, jewelAPoint);

            
           
        }
    }
}