﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootingPacman
{
    class Enemy
    {
        public Animation EnemyAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int Damage;
        public int Value;
        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }
        float enemyMovespeed;

        public void Initialize(Animation animation, Vector2 position)
        {
            EnemyAnimation = animation;
            Position = position;
            Active = true;
            Health = 10;
            Damage = 10;
            enemyMovespeed = 6f;
            Value = 100;
        }

        public void Update(GameTime gameTime)
        { 
            // Enemy moves left => - to x-pos
            Position.X -= enemyMovespeed;
            EnemyAnimation.Position = Position;
            EnemyAnimation.Update(gameTime);

            // Remove if dead or out of screen
            if (Position.X < -Width || Health <= 0)
            {
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            EnemyAnimation.Draw(spriteBatch);
        }


    }
}
