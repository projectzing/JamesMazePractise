using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MazePractice
{
    public class MazeExit
    {
        public int PositionX;
        public int PositionY;
        public Rectangle CollisionRect;
        public Texture2D Tex;
        List<Path> PathList;
        public Vector2 GridPosition;

        public MazeExit(Texture2D _Tex, int _PositionX,int _PositionY,List<Path>_PathList)
        {
            Tex = _Tex;
            PositionX = _PositionX;
            PositionY = _PositionY;
            CollisionRect = new Rectangle((int)PositionX,(int)PositionY,Tex.Width,Tex.Height);
            PathList = _PathList;
            FindPath();

        }
        public void FindPath()
        {
            foreach (Path Path in PathList)
            {
                if (PositionX < Path.Position.X + Path.texture.Width)
                {
                    if (PositionX + Tex.Width > Path.Position.X)
                    {
                        if (PositionY + Tex.Height > Path.Position.Y)
                        {
                            if (PositionY < Path.Position.Y + Path.texture.Height)
                            {
                                GridPosition = Path.GridPosition;
                            }
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(Tex, CollisionRect, Color.White);
        }
    }
}
