using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazePractice
{
    public class Path
    {
        public Vector2 Position;
        public Texture2D texture;
        public Rectangle CollisionRect;
        public bool Opened = false;
        public List<Path> PathList;
        public List<HorizWalls> HorizWallsList;
        public List<VertWalls> VertWallsList;
        public Vector2 MiddlePoint;

        public bool UpPossible = false;
        public bool DownPossible = false;
        public bool LeftPossible = false;
        public bool RightPossible = false;

        bool UpChecked = false;
        bool DownChecked = false;
        bool LeftChecked = false;
        bool RightChecked = false;

        Random rnd = new Random();
        int HorizWallsWidth;
        int HorizWallsHeight;
        int VertWallsWidth;
        int VertWallsHeight;
        public bool NewPath = false;
        int NumberOfPathTiles;

        public Vector2 GridPosition = new Vector2(-1,-1);
        public Path(Texture2D tex, int x, int y, List<Path> _path, List<HorizWalls> _horizwalls, List<VertWalls> _vertwalls,Random _rnd,bool _NewPath,bool _UpPossible,bool _DownPossible,bool _LeftPossible,bool _RightPossible,int _NumberOfPathTiles,Dictionary<Vector2,Path> _TileGrid)
        {
            Position.X = x;
            Position.Y = y;
            texture = tex;
            CollisionRect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            NewPath = _NewPath;
            if (_NewPath == false)
            {


                PathList = _path;
                HorizWallsList = _horizwalls;
                VertWallsList = _vertwalls;
                HorizWallsWidth = HorizWallsList[0].texture.Width;
                HorizWallsHeight = HorizWallsList[0].texture.Height;
                VertWallsWidth = VertWallsList[0].texture.Width;
                VertWallsHeight = VertWallsList[0].texture.Height;
                rnd = _rnd;
                NumberOfPathTiles = _NumberOfPathTiles;
                MiddlePoint = new Vector2(Position.X+tex.Width/2,Position.Y+tex.Height/2);
                

            }
            UpPossible = _UpPossible;
            DownPossible = _DownPossible;
            LeftPossible = _LeftPossible;
            RightPossible = _RightPossible;
        }
        public void OpenWall()
        {
            if (Opened==true){
                int chooseDirection;
                chooseDirection = rnd.Next(0, 4);
                if (chooseDirection==0 && UpChecked==false)
                {
                    UpChecked = true;
                    CheckUp();
                }
                    
                else if (chooseDirection == 1 && DownChecked==false)
                {
                    DownChecked = true;
                    CheckDown();
                }
                else if (chooseDirection == 2 && LeftChecked==false)
                {
                    LeftChecked = true;
                    CheckLeft();
                }

                else if (chooseDirection == 3 && RightChecked == false)
                {
                    RightChecked = true;
                    CheckRight();
                }
                
            }
        }
        public void CheckUp()
        {
            for (int i = 0; i < NumberOfPathTiles;i++ )
            {
                if ((PathList[i].Position.X == this.Position.X) && (PathList[i].Position.Y == (this.Position.Y - HorizWallsHeight - texture.Height)))
                {
                    if (PathList[i].Opened == false)
                    {
                        PathList[i].Opened = true;
                        int j = 0;
                        foreach (HorizWalls HorizWalls in HorizWallsList)
                        {

                            if ((HorizWalls.Position.X == this.Position.X) && (HorizWalls.Position.Y == (this.Position.Y - HorizWallsHeight)))
                            {
                                PathList[i].DownPossible = true;
                                UpPossible = true;
                                HorizWallsList[j].OnDestroy();
                                HorizWallsList.RemoveAt(j);

                                break;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            
        }
        public void CheckDown()
        {
            for (int i = 0; i < NumberOfPathTiles; i++)
            {
                if ((PathList[i].Position.X == this.Position.X) && (PathList[i].Position.Y == (this.Position.Y + texture.Height + HorizWallsHeight)))
                {
                    if (PathList[i].Opened == false)
                    {
                        PathList[i].Opened = true;
                        int j = 0;
                        foreach (HorizWalls HorizWalls in HorizWallsList)
                        {
                            
                            if ((HorizWalls.Position.X == this.Position.X) && (HorizWalls.Position.Y == (this.Position.Y + texture.Height)))
                            {
                                PathList[i].UpPossible = true;
                                DownPossible = true;
                                HorizWallsList[j].OnDestroy();
                                HorizWallsList.RemoveAt(j);
                                
                                break;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
        public void CheckLeft()
        {
            for (int i = 0; i < NumberOfPathTiles; i++)
            {
                if ((PathList[i].Position.X == (this.Position.X - texture.Width - VertWallsWidth)) && (PathList[i].Position.Y == (this.Position.Y)))
                {
                    if (PathList[i].Opened == false)
                    {
                        PathList[i].Opened = true;
                        int j = 0;
                        foreach (VertWalls VertWalls in VertWallsList)
                        {

                            if ((VertWalls.Position.X == this.Position.X-VertWallsWidth) && (VertWalls.Position.Y == (this.Position.Y)))
                            {
                                PathList[i].RightPossible = true;
                                LeftPossible = true;
                                VertWallsList[j].OnDestroy();
                                VertWallsList.RemoveAt(j);
                                
                                break;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
        public void CheckRight()
        {
            for (int i = 0; i < NumberOfPathTiles; i++)
            {
                if ((PathList[i].Position.X == (this.Position.X + texture.Width + VertWallsWidth)) && (PathList[i].Position.Y == (this.Position.Y)))
                {
                    if (PathList[i].Opened == false)
                    {
                        PathList[i].Opened = true;
                        int j = 0;
                        foreach (VertWalls VertWalls in VertWallsList)
                        {
                            
                            if ((VertWalls.Position.X == this.Position.X+texture.Width) && (VertWalls.Position.Y == (this.Position.Y)))
                            {
                                PathList[i].LeftPossible = true;
                                RightPossible = true;
                                VertWallsList[j].OnDestroy();
                                VertWallsList.RemoveAt(j);
                                break;
                            }
                            j++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }

        public void Draw(SpriteBatch sp, SpriteFont sf)
        {
//if(NewPath==true)
                //sp.Draw(texture, CollisionRect, Color.White);
            if (GridPosition.Y>-1)
            {
                sp.DrawString(sf, GridPosition.ToString(), new Vector2(Position.X, Position.Y), Color.Blue);

            }
            //sp.Draw(texture, CollisionRect, Color.White); //Draws Collision Rectangle
            
        }


    }
}
