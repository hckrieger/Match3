using Frith;
using Frith.Components;
using Frith.Extensions;
using Frith.Managers;
using Frith.Systems;
using Frith.Utilities;
using Match3.Component;
using Match3.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3.Scenes
{
    public class PlayingScene : Scene
    {
        private JewelGrid jewelGrid;
        private Entity[,] grid = new Entity[8,8];
        private Entity rootGrid;
        public const int GRID_DIMENSION = 8;

        private InputManager inputManager;
        public PlayingScene(Game game) : base(game)
        {
            inputManager = game.Services.GetService<InputManager>();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            rootGrid = registry.CreateEntity(this);
            var rootTransform = new TransformComponent(){ LocalPosition = new Vector2(100, 7) };
            rootGrid.AddComponent(rootTransform);


            jewelGrid = new JewelGrid(game);
            jewelGrid.Grid = grid;

            var colorToJewelSystem = new ColorToJewelSystem(game);
            var swapJewelsSystem = new SwapJewelsSystem(game, jewelGrid);

            registry.AddSystem(new CollisionSystem());
            registry.AddSystem(colorToJewelSystem);
            registry.AddSystem(swapJewelsSystem);

            Entity selector = registry.CreateEntity();
            selector.LoadSpriteComponents("selector", "Sprites/Match3",
                                        Vector2.Zero, game, new Point(16, 16), 6);
            selector.GetComponent<SpriteComponent>().Visible = false;
            

           

            for (int y = 0; y < GRID_DIMENSION; y++)
            {
                for (int x = 0; x < GRID_DIMENSION; x++)
                {
                    grid[x, y] = registry.CreateEntity(this);
                    grid[x, y].Group("jewel");
                    grid[x, y].LoadSpriteComponents($"jewel{x}x{y}", "Sprites/Match3", 
                                                            new Vector2(x * 16, y * 16), 
                                                            game, new Point(16, 16), MathUtils.RandomInt(6));
                    grid[x, y].AddComponent(new GridPositionComponent(new Point(x, y)));                                    
                    grid[x, y].AddComponent(new BoxColliderComponent());
                    
                    grid[x, y].SetParent(rootGrid);
                }
                    
            }


            jewelGrid.EliminateMatches();


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (inputManager.KeyJustDown(Keys.Space))
            {
                int opportunityCheck = 0;
                for (int y = 0; y < GRID_DIMENSION; y++)
                {
                    for (int x = 0; x < GRID_DIMENSION; x++)
                    {
                        if (jewelGrid.MatchOpportunityCheck(x, y))
                        {
                            opportunityCheck++;
                            Logger.Info($"({x}, {y})");
                        }
                    }

                }

                Logger.Info($"opportunities: {opportunityCheck}");
            }
        }









    }
}