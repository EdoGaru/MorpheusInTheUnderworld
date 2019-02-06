﻿using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MorpheusInTheUnderworld.Collisions;
using World = MonoGame.Extended.Entities.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Animations;
using MonoGame.Extended;
using MorpheusInTheUnderworld.Classes.Components;
using MonoGame.Extended.Sprites;

namespace MorpheusInTheUnderworld
{
    class EntityFactory
    {
        private readonly World _world;
        private readonly ContentManager _contentManager;

        public EntityFactory(World world, ContentManager contentManager)
        {
            _world = world;
            _contentManager = contentManager;
        }

        public Entity CreatePlayer(Vector2 position)
        {
            var dudeTexture = _contentManager.Load<Texture2D>("Graphics/hero");
            var dudeAtlas = TextureAtlas.Create("dudeAtlas", dudeTexture, 16, 16);

            var entity = _world.CreateEntity();
            var animationFactory = new SpriteSheetAnimationFactory(dudeAtlas);
            animationFactory.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2, 1 }));
            animationFactory.Add("walk", new SpriteSheetAnimationData(new[] { 6, 7, 8, 9, 10, 11 }, frameDuration: 0.1f));
            animationFactory.Add("combat", new SpriteSheetAnimationData(new[] { 17 }, frameDuration: 0.3f, isLooping: false));
            entity.Attach(new AnimatedSprite(animationFactory, "idle"));
            entity.Attach(new Transform2(position, 0, Vector2.One*2));
            entity.Attach(new Body { Position = position, Size = new Vector2(32, 32), BodyType = BodyType.Dynamic });
            entity.Attach(new Player());
            return entity;
        }

        public void CreateTile32(Vector2 position)
        {
            var entity = _world.CreateEntity();
            var tileTexture = _contentManager.Load<Texture2D>("Graphics/tile_32x32");
            Vector2 size = new Vector2(32, 32);

            entity.Attach(new Tile());
            entity.Attach(new Sprite(tileTexture));
            entity.Attach(new Transform2(position, 0, Vector2.One));
            entity.Attach(new Body { Position = position, Size = size, BodyType = BodyType.Static });
        }
    }
}
