using System.Linq;
using Frith;
using Frith.Components;
using Frith.Extensions;
using Microsoft.Xna.Framework;

namespace Match3.Systems
{
    public class ColorToJewelSystem : Frith.System
    {
        private Game game;
        public ColorToJewelSystem(Game game)
        {
            RequireComponent<TransformComponent>();
            RequireComponent<SpriteComponent>();

            this.game = game;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var entity in GetSystemEntities().Where(m => m.BelongsToGroup("jewel")))
            {
                switch (entity.GetSpriteFrameIndex(game))
                {
                    case 0: 
                        entity.GetComponent<SpriteComponent>().Color = Color.DarkRed;
                        break;
                    case 1: 
                        entity.GetComponent<SpriteComponent>().Color = Color.DarkOrange;
                        break;                       
                    case 2: 
                        entity.GetComponent<SpriteComponent>().Color = Color.Yellow;
                        break;
                    case 3: 
                        entity.GetComponent<SpriteComponent>().Color = Color.DarkGreen;
                        break;         
                    case 4: 
                        entity.GetComponent<SpriteComponent>().Color = Color.DarkBlue;
                        break;
                    case 5: 
                        entity.GetComponent<SpriteComponent>().Color = Color.Indigo;
                        break;
                    default: 
                        entity.GetComponent<SpriteComponent>().Color = Color.White;
                        break;                           
                }
            }
        }
    }
}