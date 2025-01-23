using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Frith;
using Frith.Components;
using Frith.Managers;
using Match3.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3.Systems
{
    public class SwapJewelsSystem : Frith.System
    {
        private JewelGrid jewelGrid;
        private InputManager inputManager;

        private List<Entity> switchableJewels = new List<Entity>();

        private Entity selectedJewel1, selectedJewel2;
        public SwapJewelsSystem(Game game, JewelGrid jewelGrid)
        {
            RequireComponent<TransformComponent>();
            RequireComponent<SpriteComponent>();

            inputManager = game.Services.GetService<InputManager>();

            this.jewelGrid = jewelGrid;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
 
 

            foreach (var entity in GetSystemEntities().Where(m => m.BelongsToGroup("jewel")))
            {
                //If there's no selected jewel
                if (selectedJewel1 == null && switchableJewels.Count == 0)
                {
                    SelectJewel(entity);



                    if (switchableJewels.Count > 0)
                        break;
                    
                }

                //If there is a selected jewel....
                if (selectedJewel1 != null)
                {
                    foreach (var jewel in switchableJewels)
                    {
                        
                        if (entity == jewel)
                        {
                            
                                var x = 0;
                        } else 
                        {
                            Entity initialEntity = entity;
                            SelectJewel(entity);
                            if (entity != initialEntity)
                                break;
                        }
                    }
                }

 
            }
               
        }

        private void SelectJewel(Entity entity)
        {
                var gridPos = entity.GetComponent<GridPositionComponent>(); 
                var entityRect = entity.GetComponent<BoxColliderComponent>();
                if (entityRect.BoundingBox.Contains(inputManager.MousePosition()) && 
                        inputManager.MouseButtonJustDown())
                    {
                        selectedJewel1 = entity; 

                        switchableJewels.Clear();

                        switchableJewels.Add(jewelGrid.Grid[gridPos.GridPosition.X, gridPos.GridPosition.Y + 1]);
                        switchableJewels.Add(jewelGrid.Grid[gridPos.GridPosition.X, gridPos.GridPosition.Y - 1]);
                        switchableJewels.Add(jewelGrid.Grid[gridPos.GridPosition.X + 1, gridPos.GridPosition.Y]);
                        switchableJewels.Add(jewelGrid.Grid[gridPos.GridPosition.X - 1, gridPos.GridPosition.Y]);

                    }
        }


    }
}