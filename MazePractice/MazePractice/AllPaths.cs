using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazePractice
{
    public class AllPaths
    {
        public int PositionX;
        public int PositionY;
        public Texture2D texture;
        public Rectangle CollisionRect;
        public List<Path> PathList;
        public Texture2D PathTex;
        public List<HorizWalls> HorizWallsList = new List<HorizWalls>();
        public List<VertWalls> VertWallsList = new List<VertWalls>();
        public Random rnd;
    }
}
