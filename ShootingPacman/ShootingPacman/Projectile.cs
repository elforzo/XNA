using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootingPacman
{
    class Projectile
    {
        public Texture2D Texture;
        public Vector2 Position;
        public bool Active;
        public int Damage;
        Viewport viewport; // Represetns the viewable boundary of the game
        public int Width
        {
            get { return Texture.Width; }
        }
        public int Height
        {
            get { return Texture.Height; }
        }
        float projectileMoveSpeed;


        public void Initilize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;
            Damage = 2;
            projectileMoveSpeed = 20f;
        }

        public void Update()
        {
            // Always move to the right
            Position.X += projectileMoveSpeed;
            // Deactivate if out of screen
            if (Position.X + Texture.Width / 2 > viewport.Width)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
                new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
        
    }
}
