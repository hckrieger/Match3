using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
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

        private Entity selectedJewel1, selectedJewel2, selector;
        public SwapJewelsSystem(Game game, JewelGrid jewelGrid)
        {
            RequireComponent<TransformComponent>();
            RequireComponent<SpriteComponent>();
			RequireComponent<GridPositionComponent>();

            inputManager = game.Services.GetService<InputManager>();

            this.jewelGrid = jewelGrid;


		}



		void SelectJewel1(Point gridPos, Entity entity)
		{
			selectedJewel1 = entity;

			Logger.Info($"selected entity 1: {entity}");
			selector.GetComponent<TransformComponent>().LocalPosition = Vector2.Zero;
			selector.GetComponent<TransformComponent>().Parent = selectedJewel1;
			selector.GetComponent<SpriteComponent>().Visible = true;

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

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);



			foreach (var entity in GetSystemEntities())
			{

				if (entity.HasTag("selector"))
				{
					selector = entity;
				}
				var gridPos = entity.GetComponent<GridPositionComponent>().GridPosition;

				if (!entity.HasComponent<BoxColliderComponent>())
					continue;

				if (entity.GetComponent<BoxColliderComponent>().BoundingBox.Contains(inputManager.MousePosition()) &&
					inputManager.MouseButtonJustDown())
				{
					if (selectedJewel1 == null && switchableJewels.Count == 0)
					{


						SelectJewel1(gridPos, entity);

						break;

					}
					else
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
							selector.GetComponent<SpriteComponent>().Visible = false;
							selector.GetComponent<TransformComponent>().LocalPosition = new Vector2(-1000, -1000);
							jewelGrid.SwapJewels(selectedJewel1, selectedJewel2);

							selectedJewel1 = null;
							selectedJewel2 = null;
							switchableJewels.Clear();

							break;
						}
						else
						{
							SelectJewel1(gridPos, entity);
							break;
						}


					}
				}
			





			}




		}

    }
}