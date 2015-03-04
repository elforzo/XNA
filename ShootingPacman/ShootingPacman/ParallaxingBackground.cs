using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootingPacman
{
    class ParallaxingBackground
    {
        Texture2D texture;
        Vector2[] positions;
        int speed;
        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            // Load BG
            texture = content.Load<Texture2D>(texturePath);
            // Set BG speed
            this.speed = speed;

            // Divide screen with texture width to determine the number of tiles needed.
            // One extra added to avoid gaps
            positions = new Vector2[screenWidth / texture.Width + 1];

            // Set init. pos. of parallaxing bg
            for (int i = 0; i < positions.Length; i++)
            {
                // Place tiles side by side
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        
        public void Update()
        {
            // Update pos. of BG
            for (int i = 0; i < positions.Length; i++)
            {
                // Update pos by adding speed
                positions[i].X += speed;
                // If speed make BG move left
                if (speed <= 0)
                {
                    // Check the texture is out of view then put it at the END of the screen
                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width*(positions.Length-1);
                    }
                }
                // If speed moves BG right
                else
                {
                    // Check if texture is out of view then position at the START of the screen
                    if (positions[i].X >= texture.Width*(positions.Length-1))
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}
