using System.ComponentModel.DataAnnotations;
using Frith;
using Frith.Components;
using Frith.Extensions;
using Frith.Managers;
using Frith.Systems;
using Match3.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3;

public class Game1 : Engine  
{
    public const string TITLE_SCREEN = "TITLE_SCREEN";
    public const string NEXT_SCREEN = "NEXT_SCREEN";
    public const string PLAYING_SCENE = "PLAYING_SCENE";

    private Entity cursor;


    public Game1()
    {
        
        IsMouseVisible = false;
        isFullScreen = true;
        

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        SetResolution(288, 162, 4);
        
        var registry = Globals<Registry>.Instance();

        
        registry.AddSystem(new TransformSystem());
		registry.AddSystem(new RenderSystem(this));

		sceneManager.AddScene(PLAYING_SCENE, new PlayingScene(this));
       // sceneManager.AddScene(NEXT_SCREEN, new NextScene())
        sceneManager.SwitchToScene(PLAYING_SCENE);


        cursor = registry.CreateEntity();
        cursor.LoadSpriteComponents("cursor", "Sprites/Cursor", Vector2.Zero, this, new Point(8,8));
        cursor.GetComponent<SpriteComponent>().Color = Color.Gainsboro;

    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        cursor.GetComponent<TransformComponent>().LocalPosition = inputManager.MousePosition();
    }

}
