using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazePractice
{
    public class Pillars
    {
        public Vector2 Position;
        Texture2D texture;
        Rectangle CollisionRect;
        public Pillars(Texture2D tex, int x, int y)
        {

            Position.X = x;
            Position.Y = y;
            texture = tex;
            CollisionRect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch sp)
        {

            sp.Draw(texture, CollisionRect, Color.White);
            //sp.Draw(texture, CollisionRect,new Rectangle(12,12,12,12), Color.White); //Draws Collision Rectangle

        }


    }
}
