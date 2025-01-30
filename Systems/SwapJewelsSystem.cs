using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
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
            RequireComponent<BoxColliderComponent>();

            inputManager = game.Services.GetService<InputManager>();

            this.jewelGrid = jewelGrid;
        }

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);


			foreach (var entity in GetSystemEntities().Where(e => e.BelongsToGroup("jewel")))
			{
				var gridPos = entity.GetComponent<GridPositionComponent>().GridPosition;

				void SelectJewel1()
				{
					selectedJewel1 = entity;

					Logger.Info($"selected entity 1: {entity}");

					switchableJewels.Clear();


					if (gridPos.Y < 7)
						switchableJewels.Add(jewelGrid.Grid[gridPos.X, gridPos.Y + 1]);

					if (gridPos.Y > 0)
						switchableJewels.Add(jewelGrid.Grid[gridPos.X, gridPos.Y - 1]);

					if (gridPos.X < 7)
						switchableJewels.Add(jewelGrid.Grid[gridPos.X + 1, gridPos.Y]);

					if (gridPos.X > 0)
						switchableJewels.Add(jewelGrid.Grid[gridPos.X - 1, gridPos.Y]);
				}

				if (selectedJewel1 == null && switchableJewels.Count == 0)
				{
					
					if (entity.GetComponent<BoxColliderComponent>().BoundingBox.Contains(inputManager.MousePosition()) &&
						inputManager.MouseButtonJustDown())
					{
						SelectJewel1();

						break;
					}
				}
				else
				{
					if (entity.GetComponent<BoxColliderComponent>().BoundingBox.Contains(inputManager.MousePosition()) &&
						inputManager.MouseButtonJustDown())
					{
						foreach (var jewel in switchableJewels)
						{
							if (entity == jewel)
							{
								selectedJewel2 = entity;
								Logger.Info($"selected entity 2: {entity}");
								break;
							}
						}

						if (selectedJewel2 != null)
						{
							
							jewelGrid.SwapJewels(selectedJewel1, selectedJewel2);

							selectedJewel1 = null;
							selectedJewel2 = null;
							switchableJewels.Clear();
							break;
						} else 
						{
							SelectJewel1();
							break;
						}
					}








				}



			}


		}

    }
}