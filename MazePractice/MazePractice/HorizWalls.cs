using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazePractice
{
    public class HorizWalls : AllPaths
    {
        public Vector2 Position;
        public HorizWalls(Texture2D tex, int x, int y, List<Path> _PathList, Texture2D _PathTex)
        {

            Position.X = x;
            Position.Y = y;
            texture = tex;
            CollisionRect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            PathList = _PathList;
            PathTex = _PathTex;
        }
        public void Draw(SpriteBatch sp)
        {

            sp.Draw(texture, CollisionRect, Color.White);
            //sp.Draw(texture, CollisionRect,new Rectangle(12,12,12,12), Color.White); //Draws Collision Rectangle

        }
        public void OnDestroy()
        {
            PathList.Add(new Path(PathTex, (int)Position.X, (int)Position.Y, PathList, HorizWallsList, VertWallsList, rnd, true, true, true, false, false,0,null));
        }
    }
}
