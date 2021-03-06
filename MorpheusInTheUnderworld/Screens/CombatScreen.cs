﻿
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Microsoft.Xna.Framework.Graphics;
using MorpheusInTheUnderworld.Classes.Systems;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Content;
using RogueSharp;
using RogueSharp.MapCreation;
using MorpheusInTheUnderworld.Classes;
using System.IO;

namespace MorpheusInTheUnderworld.Screens
{
    /// <summary>
    /// This is the main Gameplay Screen
    /// </summary>
    class CombatScreen : GameScreen
    {
        private World world;
        private SpriteBatch spriteBatch;
        private OrthographicCamera orthographicCamera;
        private Viewport viewport;
        private EntityFactory entityFactory;

        ContentManager combatScreenContent;

        Texture2D minimapTile;
        Texture2D blackTexture;

        public CombatScreen(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            viewport = GraphicsDevice.Viewport;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, viewport.Width, viewport.Height);
            orthographicCamera = new OrthographicCamera(viewportAdapter);
            combatScreenContent = new ContentManager(Game.Services, "Content");

            //DotPlayerSystem dotPlayerSystem = new DotPlayerSystem();
            world = new WorldBuilder()
                     .AddSystem(new WorldSystem())
                     .AddSystem(new CameraSystem(orthographicCamera))
                     .AddSystem(new PlayerSystem(orthographicCamera))
                     //.AddSystem(dotPlayerSystem)
                     //.AddSystem(new MapRenderSystem(spriteBatch, gameplayScreenContent))
                     //.AddSystem(new DotPlayerRenderSystem(spriteBatch))
                     .AddSystem(new RenderSystem(spriteBatch, orthographicCamera))
                     .AddSystem(new TilesRenderSystem(spriteBatch, orthographicCamera))
                     .Build();

            Game.Components.Add(world);

            entityFactory = new EntityFactory(world, combatScreenContent);

            for (int i = 0; i < 50; i++)
            {
                entityFactory.CreateTile32(new Vector2(i * 32, viewport.Height / 2));
            }
            //Entity map = entityFactory.CreateMap(new Vector2(viewport.Width - (viewport.Width / 4)-200, viewport.Height - 300), "Content/Map/map_1.txt");

            //entityFactory.CreateDotPlayer(map);
            entityFactory.CreatePlayer(Vector2.Zero);
            entityFactory.CreateEnemy(new Vector2(128,0));



        }
        public override void LoadContent()
        {
            base.LoadContent();
            minimapTile = combatScreenContent.Load<Texture2D>("Graphics/minimap_tile");
            blackTexture = combatScreenContent.Load<Texture2D>("Graphics/tile_16x16");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Game.Components.Remove(world);
            combatScreenContent.Unload();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (KeyboardExtended.GetState().WasKeyJustDown(Keys.Escape))
                ScreenManager.LoadScreen(new MainMenuScreen(Game));
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);



            spriteBatch.End();

        }
    }
}
