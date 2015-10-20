using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazePractice
{
    public class Player
    {
        KeyboardState keystate;
        Texture2D tex;
        Vector2 GridPosition;
        Vector2 PathToFind;
        Rectangle CollisionRect;
        List<Path> PathList;
        Path PathDetected;
        Path CurrentPath;
        MazeExit Exit;

        public int Speed = 4;

        bool PathFound=false;
        bool UpPossible=false;
        bool DownPossible=false;
        bool LeftPossible=false;
        bool RightPossible=false;
        bool CheckingRecentOpenPath = false;
        bool YU=false;
        bool ResetFalsePathCounter = false;
        public bool Moving = false;

        int positionX;
        int positionY;
        int RootX;
        int RootY;

        public int FalsePathCounter = 0;
        Dictionary<Vector2, int> FalsePathTracker = new Dictionary<Vector2,int>();
        Dictionary<Path, bool> UpVisited = new Dictionary<Path, bool>();
        Dictionary<Path, bool> DownVisited = new Dictionary<Path, bool>();
        Dictionary<Path, bool> LeftVisited = new Dictionary<Path, bool>();
        Dictionary<Path, bool> RightVisited = new Dictionary<Path, bool>();
        List<Path> Tiles = new List<Path>();

        public Vector2 CurrentGridPosition;
        public Vector2 NextGridPosition;
        public List<Vector2> MostRecentOpenPath = new List<Vector2>();

        List<Vector2> PathToFollow = new List<Vector2>();

        Dictionary<Vector2, Path> TileGrid;

        public Player(Texture2D _tex, int x, int y, List<Path> _pathlist,MazeExit _Exit,Dictionary<Vector2,Path> _TileGrid)
        {
            positionX = x;
            positionY = y;
            tex = _tex;
            PathList=_pathlist;
            CollisionRect = new Rectangle(positionX, positionY, tex.Width, tex.Height);
            Exit = _Exit;
            PathToFind = Exit.GridPosition;
            TileGrid = _TileGrid;
            RootX = positionX;
            RootY = positionY;
            AddTilesToList();
            FillDictionaries(UpVisited);
            FillDictionaries(DownVisited);
            FillDictionaries(LeftVisited);
            FillDictionaries(RightVisited);
        }
        public void PathDetection()
        {
            foreach (Path Path in PathList)
            {
                if (positionX < Path.Position.X+Path.texture.Width)
                {
                    if (positionX + tex.Width > Path.Position.X)
                    {
                        if (positionY + tex.Height > Path.Position.Y)
                        {
                            if (positionY < Path.Position.Y + Path.texture.Height)
                            {
                                if (Path.NewPath == false) 
                                { 
                                    PathDetected = Path;
                                }
                                    
                                if (PathDetected.GridPosition.Y > -0.5f)
                                {
                                    GridPosition = PathDetected.GridPosition;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void MoveToExit()
        {
            PathDetection();
            RootX = positionX;
            RootY = positionY;
            CurrentGridPosition = PathDetected.GridPosition;

            while (PathFound == false)

            {
                
                CurrentPath = TileGrid[PathDetected.GridPosition];
                CurrentGridPosition = CurrentPath.GridPosition;
                UpPossible = false;
                DownPossible = false;
                LeftPossible = false;
                RightPossible = false;
                CheckPossibleDirection(CurrentPath);
                PickDirection(CurrentPath);
                TestForNextTile();

                PathDetection();
                CurrentPath = TileGrid[PathDetected.GridPosition];
                CurrentGridPosition = CurrentPath.GridPosition;
                if (ResetFalsePathCounter)
                {
                    FalsePathCounter = 1;
                    ResetFalsePathCounter = false;
                }
                if (CheckingRecentOpenPath != true)
                {
                    PathToFollow.Add(CurrentGridPosition);
                    FalsePathTracker.Add(CurrentGridPosition, FalsePathCounter);
                }

                
                if (CurrentPath.GridPosition == Exit.GridPosition)
                {
                    positionX = RootX;
                    positionY = RootY;
                    PathFound = true;
                }

            }
        }
        public void CheckPossibleDirection(Path _CurrentPath)
        {
            if (_CurrentPath.UpPossible)
            {
                if (UpVisited[_CurrentPath]==false)
                {
                    UpPossible = true;
                }
            }

            if (_CurrentPath.DownPossible)
            {
                if (DownVisited[_CurrentPath] == false)
                {
                    DownPossible = true;
                }
            }
            if (_CurrentPath.LeftPossible)
            {
                if (LeftVisited[_CurrentPath] == false)
                {
                    LeftPossible = true;
                }
            }
            if (_CurrentPath.RightPossible)
            {
                if (RightVisited[_CurrentPath] == false)
                {
                    RightPossible = true;
                }
            }
        }

        public void PickDirection(Path _CurrentPath)
        {
            if (UpPossible == true)
            {
                NextGridPosition = TileGrid[new Vector2(_CurrentPath.GridPosition.X,_CurrentPath.GridPosition.Y-1)].GridPosition;
                FalsePathCounter++;
                if (DownPossible || LeftPossible || RightPossible)
                {
                    if (CheckingRecentOpenPath != true)
                    {
                        MostRecentOpenPath.Add(_CurrentPath.GridPosition);
                    }
                    ResetFalsePathCounter = true;
                }
                UpVisited[_CurrentPath]= true;
                DownVisited[TileGrid[NextGridPosition]] = true;
                CheckingRecentOpenPath = false;
            }
            else if (DownPossible == true)
            {
                NextGridPosition = TileGrid[new Vector2(_CurrentPath.GridPosition.X, _CurrentPath.GridPosition.Y + 1)].GridPosition;
                FalsePathCounter++;

                if (LeftPossible || RightPossible)
                {
                    if (CheckingRecentOpenPath != true)
                    {
                        MostRecentOpenPath.Add(_CurrentPath.GridPosition);
                    }
                    ResetFalsePathCounter = true;
                }
                DownVisited[_CurrentPath] = true;
                UpVisited[TileGrid[NextGridPosition]] = true;
                CheckingRecentOpenPath = false;
            }
            else if (LeftPossible == true)
            {
                NextGridPosition = TileGrid[new Vector2(_CurrentPath.GridPosition.X-1, _CurrentPath.GridPosition.Y)].GridPosition;
                FalsePathCounter++;

                if (RightPossible)
                {
                    if (CheckingRecentOpenPath != true)
                    {
                        MostRecentOpenPath.Add(_CurrentPath.GridPosition);
                    }
                    ResetFalsePathCounter = true;
                }
                LeftVisited[_CurrentPath] = true;
                RightVisited[TileGrid[NextGridPosition]] = true;
                CheckingRecentOpenPath = false;
            }
            else if (RightPossible == true)
            {
                NextGridPosition = TileGrid[new Vector2(_CurrentPath.GridPosition.X+1, _CurrentPath.GridPosition.Y)].GridPosition;
                FalsePathCounter++;
                RightVisited[_CurrentPath] = true;
                LeftVisited[TileGrid[NextGridPosition]] = true;
                CheckingRecentOpenPath = false;
            }
            else
            {
                for (int i = 0; i < FalsePathTracker[CurrentGridPosition]; i++)
                {
                    PathToFollow.Remove(PathToFollow.Last());
                }

                if (CheckingRecentOpenPath == true)
                {
                    MostRecentOpenPath.Remove(MostRecentOpenPath.Last());
                    
                }

                FalsePathCounter = 0;
                NextGridPosition = MostRecentOpenPath.Last();

                CheckingRecentOpenPath = true;
                
            }
        }
        public void TestForNextTile()
        {
            positionX = (int)TileGrid[NextGridPosition].Position.X + 10;
            positionY = (int)TileGrid[NextGridPosition].Position.Y+10;
        }
        public void MoveToNextPosition()
        {
            if (PathToFollow.Count != 0)
            {
                if (GridPosition.X < PathToFollow[0].X)
                {
                    positionX += Speed;
                }
                else if (PathDetected.GridPosition.X > PathToFollow[0].X)
                {
                    positionX -= Speed;
                }
                else if (PathDetected.GridPosition.Y < PathToFollow[0].Y)
                {
                    positionY += Speed;
                }
                else if (PathDetected.GridPosition.Y > PathToFollow[0].Y)
                {
                    positionY -= Speed;
                }
                else
                {
                    if (CheckContainsPoint(CollisionRect, TileGrid[PathDetected.GridPosition].MiddlePoint))
                    {
                        PathToFollow.RemoveAt(0);
                    }
                    else
                    {
                        if (PathDetected.MiddlePoint.X >= positionX+16)
                        {
                            positionX += Speed;
                        }
                        else if (PathDetected.MiddlePoint.X <= positionX)
                        {
                            positionX -= Speed;
                        }
                        if (PathDetected.MiddlePoint.Y >= positionY+16)
                        {
                            positionY += Speed;
                        }
                        else if (PathDetected.MiddlePoint.Y <= positionY)
                        {
                            positionY -= Speed;
                        }
                    }
                }
            }
        }
        public void FillDictionaries(Dictionary<Path, bool> DictionaryToFill)
        {
            foreach (Path Tile in Tiles)
            {
                DictionaryToFill.Add(Tile, false);
            }
            
        }
        public void AddTilesToList()
        {
            foreach (Path Path in PathList)
            {
                if (Path.NewPath == false)
                {
                    Tiles.Add(Path);
                }
            }
        }
        #region Movement
        public void MoveUp()
        {
            int UpLimit=0;

            if (PathDetected.UpPossible == false)
            {
                UpLimit = (int)PathDetected.Position.Y + 2;
            }
                
           if (positionY - Speed > UpLimit)
           {
               positionY -= Speed;
           }
           else
           {
               positionY = UpLimit;
           }
        }

        public void MoveDown()
        {
            int DownLimit = 10000;

            if (PathDetected.DownPossible == false)
            {
                DownLimit = (int)PathDetected.Position.Y + PathDetected.texture.Height - 2;
            }
            if (positionY + tex.Height + Speed < DownLimit)
            {
                positionY += Speed;
            }
            else
            {
                positionY = DownLimit - tex.Height;
            }
        }
        public void MoveLeft()
        {
            int LeftLimit = 0;

            if (PathDetected.LeftPossible == false)
            {
                LeftLimit = (int)PathDetected.Position.X + 2;
            }
            if (positionX > LeftLimit)
            {
                positionX -= Speed;
            }
            else
            {
                positionX = LeftLimit;
            }
        }
        public void MoveRight()
        {
            int RightLimit = 10000;

            if (PathDetected.RightPossible == false)
            {
                RightLimit = (int)PathDetected.Position.X +PathDetected.texture.Width - 2;
            }
            if (positionX + tex.Width < RightLimit)
            {
                positionX += Speed;
            }
            else
            {
                positionX = RightLimit - tex.Width;
            }
        }

        public void Move()
        {
            if (keystate.IsKeyDown(Keys.W))
            {
                MoveUp();
            }
            if (keystate.IsKeyDown(Keys.S))
            {
                MoveDown();
            }
            if (keystate.IsKeyDown(Keys.A))
            {
                MoveLeft();
            }
            if (keystate.IsKeyDown(Keys.D))
            {
                MoveRight();
            }
            
        }
#endregion Movement
        public void Draw(SpriteBatch sp, SpriteFont sf)
        {
              sp.Draw(tex, CollisionRect, Color.White);
              sp.DrawString(sf, GridPosition.ToString(), new Vector2(50, 580), Color.Red);
              sp.DrawString(sf, new Vector2(PathToFind.X,PathToFind.Y).ToString(), new Vector2(400, 580), Color.Red);
              sp.DrawString(sf, new Vector2(NextGridPosition.X, NextGridPosition.Y).ToString(), new Vector2(200, 580), Color.Red);
              if (PathToFollow.Count>0)
                  for (int i = 0; i < PathToFollow.Count; i++)
                  {
                      sp.DrawString(sf, new Vector2(PathToFollow[i].X,PathToFollow[i].Y).ToString(), new Vector2(1100, 30*i), Color.Red);
                  }
              sp.DrawString(sf, FalsePathCounter.ToString(), new Vector2(100, 30), Color.Red);
              sp.DrawString(sf, CheckingRecentOpenPath.ToString(), new Vector2(200, 30), Color.Red);
              
        }
        public bool CheckContainsPoint(Rectangle ContainsRectangle, Vector2 PointToCheck)
        {
            if (ContainsRectangle.X < PointToCheck.X)
            {
                if (ContainsRectangle.Width+CollisionRect.X > PointToCheck.X)
                {
                    if (ContainsRectangle.Y < PointToCheck.Y)
                    {
                        if (ContainsRectangle.Height + CollisionRect.Y > PointToCheck.Y)
                        {
                            return true;
                        }
                    }
                }
                
            }
            return false;
        }

        public void Update()
        {
            keystate = Keyboard.GetState();
            PathDetection();
            if (keystate.IsKeyDown(Keys.P))
            {
                MoveToNextPosition();
            }
            //foreach (Path path in PathList)
           // {
                //path.NewPath = false;
          //  }

            /*if (keystate.IsKeyDown(Keys.Y)&&YU==true)
            {
                if (InitializePathFind == true)
                {
                    RootGridPosition = PathDetected.GridPosition;
                    CurrentGridPosition = PathDetected.GridPosition;
                    PathDetection();
                    InitializePathFind = false;
                }
                CurrentPath = TileGrid[PathDetected.GridPosition];
                CurrentGridPosition = CurrentPath.GridPosition;
                UpPossible = false;
                DownPossible = false;
                LeftPossible = false;
                RightPossible = false;
                CheckPossibleDirection(CurrentPath);
                PickDirection(CurrentPath);
                MoveToNextTile();

                PathDetection();
                CurrentPath = TileGrid[PathDetected.GridPosition];
                CurrentGridPosition = CurrentPath.GridPosition;
                if (ResetFalsePathCounter)
                {
                    FalsePathCounter = 1;
                    ResetFalsePathCounter = false;
                }
                if (CheckingRecentOpenPath != true)
                {
                    PathToFollow.Add(CurrentGridPosition);
                    FalsePathTracker.Add(CurrentGridPosition, FalsePathCounter);
                }

                
                if (CurrentPath.GridPosition == Exit.GridPosition)
                {
                    PathFound = true;
                }
                
                YU = false;
            }*/

            if (keystate.IsKeyDown(Keys.U))
            {
                YU = true;
            }

            

            //PathDetected.NewPath = true;
            Move();
            if (keystate.IsKeyDown(Keys.Y) && YU == true)
            {
               MoveToExit();
            }
            CollisionRect = new Rectangle(positionX, positionY, tex.Width, tex.Height);
            

        }
    }
}
