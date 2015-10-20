using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Sprites;

namespace MazePractice
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D[] Textures = new Texture2D[8];
        public List<Path> PathList = new List<Path>();
        public List<Pillars> PillarList = new List<Pillars>();
        public List<HorizWalls> HorizWallsList = new List<HorizWalls>();
        public List<VertWalls> VertWallsList = new List<VertWalls>();
        static Random rnd = new Random();
        KeyboardState keystate;
        //Numbers Need to be odd to have Centre Square, Don't need to be the same
        public static int MazeWidth = 13;
        public static int MazeHeight = 7;
        int[] PathShuffler;
        Dictionary<Vector2, Path> TileGrid = new Dictionary<Vector2,Path>();
        int OpenPaths;
        public Player Player1;
        public Player Player2;
        public Player Player3;
        MazeExit MazeExit;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        public Player AddPlayer(Player __Player)
        {
            int PlayerXSquare = rnd.Next(0, MazeWidth);
            int PlayerYSquare = rnd.Next(0, MazeHeight);
            int PlayerX = ((PlayerXSquare * 64) + ((PlayerXSquare) * 16))+32;
            int PlayerY = ((PlayerYSquare * 64) + ((PlayerYSquare) * 16))+32;
            __Player = new Player(Textures[4], PlayerX, PlayerY,PathList,MazeExit,TileGrid);
            return __Player;
        }
        public void AddExit()
        {
            int ExitXSquare = rnd.Next(0, MazeWidth);
            int ExitYSquare = rnd.Next(0, MazeHeight);
            int ExitX = ((ExitXSquare * 64) + ((ExitXSquare) * 16)) + 32;
            int ExitY = ((ExitYSquare * 64) + ((ExitYSquare) * 16)) + 32;

            MazeExit = new MazeExit(Textures[7], ExitX, ExitY, PathList);
        }
        
        #region CreateMazeBase
        public void createPaths(Texture2D tex)
        {
            int PathPositionX = Textures[3].Width;
            int PathPositionY = Textures[2].Height;
            int PathColumns = MazeWidth;
            int PathRows = MazeHeight;
            for (int i = 0; i < PathColumns; i++)
            {
                for (int j = 0; j < PathRows; j++)
                {
                    PathList.Add(new Path(tex, (int)PathPositionX, (int)PathPositionY, PathList, HorizWallsList, VertWallsList,rnd,false,false,false,false,false,MazeWidth*MazeHeight,TileGrid));
                    PathPositionY += (tex.Height + Textures[2].Height);
                    TileGrid.Add(new Vector2(i, j), PathList[PathList.Count - 1]);
                }
                PathPositionY = Textures[2].Height;
                PathPositionX += tex.Width+Textures[3].Width;
            }
        }
        public void createVertWalls(Texture2D tex)
        {
            int VertWallsPositionX = 0;
            int VertWallsPositionY = Textures[3].Width;
            int VertWallsColumns = MazeWidth+1;
            int VertWallsRows = MazeHeight;
            int VertWallsDistance = Textures[3].Width+Textures[0].Height;
            for (int i = 0; i < VertWallsColumns; i++)
            {
                for (int j = 0; j < VertWallsRows; j++)
                {
                    VertWallsList.Add(new VertWalls(tex, (int)VertWallsPositionX, (int)VertWallsPositionY, PathList,Textures[6]));
                    VertWallsPositionY += VertWallsDistance;

                }
                VertWallsPositionY = Textures[3].Width;
                VertWallsPositionX += Textures[3].Width+Textures[0].Width;
            }
        }
        public void createHorizWalls(Texture2D tex)
        {
            int HorizWallsPositionX = Textures[2].Height;
            int HorizWallsPositionY = 0;
            int HorizWallsColumns = MazeWidth;
            int HorizWallsRows = MazeHeight+1;
            int HorizWallsDistance = Textures[0].Height+Textures[2].Height;
            for (int i = 0; i < HorizWallsColumns; i++)
            {
                for (int j = 0; j < HorizWallsRows; j++)
                {
                    HorizWallsList.Add(new HorizWalls(tex, (int)HorizWallsPositionX, (int)HorizWallsPositionY,PathList,Textures[5]));
                    HorizWallsPositionY += HorizWallsDistance;

                }
                HorizWallsPositionY = 0;
                HorizWallsPositionX += Textures[2].Height + Textures[0].Width;
            }
        }
        
        public void createPillars(Texture2D tex)
        {
            int PillarPositionX = 0;
            int PillarPositionY = 0;
            int PillarColumns = MazeWidth + 1;
            int PillarRows = MazeHeight + 1;
            int PillarDistance = Textures[0].Height + Textures[1].Height ;
            for (int i = 0; i < PillarColumns; i++)
            {
                for (int j = 0; j < PillarRows; j++)
                {
                    PillarList.Add(new Pillars(tex, (int)PillarPositionX, (int)PillarPositionY));
                    PillarPositionY += PillarDistance;

                }
                PillarPositionY = 0;
                PillarPositionX += Textures[0].Width+Textures[1].Width;
            }
        }
        #endregion CreateMazeBase 
        public void ShuffleList()
        {
            int n = MazeHeight*MazeWidth;
            PathShuffler = new int[n];
            for (int i = 0; i < n; i++)
            {
                PathShuffler[i] = i;
            }
            for (int i = 0; i < n; i++)
            {
                int r = rnd.Next(0, n);
                int temp = PathShuffler[i];
                PathShuffler[i] = PathShuffler[r];
                PathShuffler[r] = temp;
            }
        }
        public void CreateMazeRoutes()
        {
            ShuffleList();
            for (int i = 0; i < MazeHeight*MazeWidth; i++)
            {
               PathList[PathShuffler[i]].OpenWall();
            }
        }

        public void SetGridPositions()
        {
            for (int i = 0; i < MazeWidth; i++)
            {
                for (int j = 0; j < MazeHeight; j++)
                {
                    TileGrid[new Vector2(i, j)].GridPosition = new Vector2(i, j);
                }
            }
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Textures[0]=Content.Load<Texture2D>(@"Textures//tex_Path");
            Textures[1] = Content.Load<Texture2D>(@"Textures//tex_pillar");
            Textures[2]= Content.Load<Texture2D>(@"Textures//tex_WallH");
            Textures[3] = Content.Load<Texture2D>(@"Textures//tex_WallV");
            Textures[4] = Content.Load<Texture2D>(@"Textures//player");
            Textures[5] = Content.Load<Texture2D>(@"Textures//tex_PathHoriz");
            Textures[6] = Content.Load<Texture2D>(@"Textures//tex_PathVert");
            Textures[7] = Content.Load<Texture2D>(@"Textures//tex_Exit");
            font = Content.Load<SpriteFont>("font");

            createHorizWalls(Textures[2]);
            createVertWalls(Textures[3]);
            createPaths(Textures[0]);
            createPillars(Textures[1]);

            SetGridPositions();

            PathList[PathList.Count / 2].Opened = true;
            OpenPaths = PathList.Count;
            while (OpenPaths != 0)
            {
                CreateMazeRoutes();
                OpenPaths = 0;
                int NumberOfPathTiles = MazeHeight * MazeWidth;
                for (int i = 0; i < NumberOfPathTiles;i++ )
                {
                    if (PathList[i].Opened == false)
                    {
                        OpenPaths += 1;
                    }
                }
            }

            AddExit();

            Player1=AddPlayer(Player1);
            Player2=AddPlayer(Player2);
            Player3=AddPlayer(Player3);
        }

       
        protected override void Update(GameTime gameTime)
        {
            keystate = Keyboard.GetState();
            Player1.Update();
            Player2.Update();
            Player3.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            for (int i = 0; i < PillarList.Count; i++)
                PillarList[i].Draw(spriteBatch);
            for (int i = 0; i < VertWallsList.Count; i++)
                VertWallsList[i].Draw(spriteBatch);
            for (int i = 0; i < HorizWallsList.Count; i++)
                HorizWallsList[i].Draw(spriteBatch);

            Player1.Draw(spriteBatch,font);
            Player2.Draw(spriteBatch, font);
            Player3.Draw(spriteBatch, font);
            MazeExit.Draw(spriteBatch);


            #region Debug FONT Draw

            // Grid Coordinate
            List<Vector2> keyList = new List<Vector2>(this.TileGrid.Keys);
            /*for (int i = 0; i < TileGrid.Count; i++)
            {
                spriteBatch.DrawString(font, keyList[i].ToString(), new Vector2(PathList[i].Position.X, PathList[i].Position.Y), Color.Blue);
            }
            //Tile Direction (True False etc.)*/
            for (int i = 0; i < PathList.Count; i++)
            {
                PathList[i].Draw(spriteBatch,font);

                //spriteBatch.DrawString(font, PathList[i].UpPossible.ToString(),new Vector2(PathList[i].Position.X,PathList[i].Position.Y),Color.Blue);
                //spriteBatch.DrawString(font, PathList[i].DownPossible.ToString(), new Vector2(PathList[i].Position.X, PathList[i].Position.Y+13), Color.Blue);
                //spriteBatch.DrawString(font, PathList[i].LeftPossible.ToString(), new Vector2(PathList[i].Position.X, PathList[i].Position.Y+26), Color.Blue);
                //spriteBatch.DrawString(font, PathList[i].RightPossible.ToString(), new Vector2(PathList[i].Position.X, PathList[i].Position.Y+39), Color.Blue);
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
