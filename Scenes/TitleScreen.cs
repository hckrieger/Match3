using System;
using Frith;
using Frith.Components;
using Frith.Extensions;
using Frith.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Match3;

public class TitleScreen : Scene
{
    private InputManager inputManager;

    private Entity entity, entity2;

    private SceneManager sceneManager;

    public TitleScreen(Game game) : base(game)
    {
        inputManager = game.Services.GetService<InputManager>();
        sceneManager = game.Services.GetService<SceneManager>();
    }

    public override void OnCreate()
    {
        base.OnCreate();


        entity = registry.CreateEntity(this);
        entity.LoadSpriteComponents("entity1", "Sprites/Match3", new Vector2(0, 0), game);
        entity.GetComponent<SpriteComponent>().Color = Color.Blue;

        entity2 = registry.CreateEntity(this);
        entity2.LoadSpriteComponents("entity2", "box", new Vector2(10, 10), game);
        entity2.GetComponent<SpriteComponent>().Color = Color.Green;

        entity2.GetComponent<TransformComponent>().Parent = entity;
       
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        ref var entityTransform = ref entity.GetComponent<TransformComponent>();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float speed = 50;

        if (inputManager.KeyHeld(Keys.A))
        {
            entityTransform.LocalPosition += new Vector2(-speed, 0) * dt;
        }  


        if (inputManager.KeyHeld(Keys.D))
        {
            entityTransform.LocalPosition += new Vector2(speed, 0) * dt;
        }  


        if (inputManager.KeyHeld(Keys.W))
        {
            entityTransform.LocalPosition += new Vector2(0, -speed) * dt;
        }  


        if (inputManager.KeyHeld(Keys.S))
        {
            entityTransform.LocalPosition += new Vector2(0, speed) * dt;
        }  

        if (inputManager.KeyJustDown(Keys.Space))
        {
            if (entity2.HasParent())
            {
                entity2.RemoveParent();
            } else {
                entity2.SetParent(entity);
                entity2.GetComponent<TransformComponent>().LocalPosition = new Vector2(10, 10);
            }
            
        }
        //Logger.Info($"{entity2.GetComponent<TransformComponent>().Parent}");
    }
}
