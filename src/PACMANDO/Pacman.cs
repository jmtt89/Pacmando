using System;
using System.IO;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace WindowsGame3 
{

    public class Pacman
    {
        Texture2D[] PacMan;

        public Texture2D GETTEXTURE()
        {
            return PacMan[0];
        }

        Texture2D Dedman; //muerte de pacman
        int TotalFrame = 20;


        float TTotal;
        float TPFrame;
        int Frame;

        int Estado; // Estado :: ^ 0, -> 1, V 2, <- 3

        int[,] Grid;
        Vector2 Pos;

        Vector2 PosI;


        float VelMov = 0.2f;

        bool life;

        public Pacman(int[,] grd,int FramesXseg)
        {
            TPFrame = 1.0f / FramesXseg;
            TTotal = 0;
            Frame = 0;
            Estado = 0;
            life = true;

            bool Find = false;
            Grid = grd;
            for (int i = 0; i < grd.GetLength(0)&&!Find; i++)
            {
                for (int j = 0; j < grd.GetLength(1) && !Find; j++)
                {
                    if (grd[i, j] == 5)
                    {
                        Pos.X = j;
                        Pos.Y = i;
                        PosI = Pos;
                        Find = true;
                    }
                }
            }
        }
        
        public void Load(ContentManager Content)
        {
            PacMan = new Texture2D[] {
                Content.Load<Texture2D>("Imagenes/pacmando0"),// 0 - Inicio
                Content.Load<Texture2D>("Imagenes/pacmando1"),
                Content.Load<Texture2D>("Imagenes/pacmando2"),
                Content.Load<Texture2D>("Imagenes/pacmando3"),// 1,2,3 ->
                Content.Load<Texture2D>("Imagenes/pacmando4"),
                Content.Load<Texture2D>("Imagenes/pacmando5"),
                Content.Load<Texture2D>("Imagenes/pacmando6"),// 4,5,6 V
                Content.Load<Texture2D>("Imagenes/pacmando7"),
                Content.Load<Texture2D>("Imagenes/pacmando8"),
                Content.Load<Texture2D>("Imagenes/pacmando9"),// 7,8,9 <-
                Content.Load<Texture2D>("Imagenes/pacmando10"),
                Content.Load<Texture2D>("Imagenes/pacmando11"),
                Content.Load<Texture2D>("Imagenes/pacmando12"),// 10,11,12 ^
                /////////////// Con BARBA /////////////////
                Content.Load<Texture2D>("Imagenes/pacmando0.1"),// 13 - Inicio
                Content.Load<Texture2D>("Imagenes/pacmando1.1"),
                Content.Load<Texture2D>("Imagenes/pacmando2.1"),
                Content.Load<Texture2D>("Imagenes/pacmando3.1"),// 14,15,16 ->
                Content.Load<Texture2D>("Imagenes/pacmando4.1"),
                Content.Load<Texture2D>("Imagenes/pacmando5.1"),
                Content.Load<Texture2D>("Imagenes/pacmando6.1"),// 17,18,19 V
                Content.Load<Texture2D>("Imagenes/pacmando7.1"),
                Content.Load<Texture2D>("Imagenes/pacmando8.1"),
                Content.Load<Texture2D>("Imagenes/pacmando9.1"),// 20,21,22 <-
                Content.Load<Texture2D>("Imagenes/pacmando10.1"),
                Content.Load<Texture2D>("Imagenes/pacmando11.1"),
                Content.Load<Texture2D>("Imagenes/pacmando12.1"),// 23,24,25 ^
            };

            Dedman = Content.Load<Texture2D>("Imagenes/DyingSheetNew");

        }

        public void Draw(SpriteBatch batch,Vector2 Base,Texture2D Tex)
        {
            float Alto = Tex.Height/Grid.GetLength(0);
            float Ancho = Tex.Width/Grid.GetLength(1);

            Vector2 Dr = new Vector2((int)(Base.X + Ancho * Math.Floor(Pos.X))-4, (int)(Base.Y + Alto * Math.Floor(Pos.Y))-5);
            if (life)
            {
                batch.Draw(PacMan[Frame], Dr, Color.White);
            }
            else
            {
                Ancho = Dedman.Width / TotalFrame;
                Alto = Dedman.Height;

                Rectangle SourceRec = new Rectangle((int)Ancho * Frame, 0, (int)Ancho, (int)Alto);
                batch.Draw(Dedman, Dr, SourceRec, Color.White);
            }
            
        }

        public void UpdateFrame(float T)
        {
            TTotal += T;
            if (TTotal > TPFrame)
            {
                // Lo que modifica el Frame
                if (Estado == 1)
                {
                    if (Frame == 1 || Frame == 14)
                        Frame = 2;
                    else if (Frame == 2 || Frame == 15)
                        Frame = 3;
                    else if (Frame == 3 || Frame == 16)
                        Frame = 1;
                    else
                        Frame = 1;
                }
                else if (Estado == 2)
                {
                    if (Frame == 4 || Frame == 17)
                        Frame = 5;
                    else if (Frame == 5 || Frame == 18)
                        Frame = 6;
                    else if (Frame == 6 || Frame == 19)
                        Frame = 4;
                    else 
                        Frame = 4;
                }
                else if (Estado == 3)
                {
                    if (Frame == 7 || Frame == 20)
                        Frame = 8;
                    else if (Frame == 8 || Frame == 21)
                        Frame = 9;
                    else if (Frame == 9 || Frame == 22)
                        Frame = 7;
                    else
                        Frame = 7;
                }
                else if (Estado == 4)
                {
                    if (Frame == 10 || Frame == 23)
                        Frame = 11;
                    else if (Frame == 11 || Frame == 24)
                        Frame = 12;
                    else if (Frame == 12 || Frame == 25)
                        Frame = 10;
                    else
                        Frame = 10;
                }
                else
                    Frame = 0;

                TTotal -= TPFrame;
            }
        }

        public void UpdateframeB(float T)
        {
            TTotal += T;
            if (TTotal > TPFrame)
            {
                // 0   1  2  3  4  5  6  7  8  9 10 11 12  
                //13  14 15 16 17 18 19 20 21 22 23 24 25

                // Lo que modifica el Frame
                if (Estado == 1)
                {
                    if (Frame == 14 || Frame == 1)
                        Frame = 15;
                    else if (Frame == 15 || Frame == 2)
                        Frame = 16;
                    else if (Frame == 16 || Frame == 3)
                        Frame = 14;
                    else
                        Frame = 14;
                }
                else if (Estado == 2)
                {
                    if (Frame == 17 || Frame == 4)
                        Frame = 18;
                    else if (Frame == 18 || Frame == 5)
                        Frame = 19;
                    else if (Frame == 19 || Frame == 6)
                        Frame = 17;
                    else
                        Frame = 17;
                }
                else if (Estado == 3)
                {
                    if (Frame == 20 || Frame == 7)
                        Frame = 21;
                    else if (Frame == 21 || Frame == 8)
                        Frame = 22;
                    else if (Frame == 22 || Frame == 9)
                        Frame = 20;
                    else
                        Frame = 20;
                }
                else if (Estado == 4)
                {
                    if (Frame == 23 || Frame == 10)
                        Frame = 24;
                    else if (Frame == 24 || Frame == 11)
                        Frame = 25;
                    else if (Frame == 25 || Frame == 13)
                        Frame = 23;
                    else
                        Frame = 23;
                }
                else
                    Frame = 13;

                TTotal -= TPFrame;
            }

        }

        public bool AnimDead(float T)
        {
            life = false;

            TTotal += T;
            if (TTotal > TPFrame)
            {
                Frame++;
                TTotal -= TPFrame;
            }

            return (Frame == 20);
        }

        public void MoverI()
        {
            Pos.X -= VelMov;
            PosI.X -= 0.2f;

            Estado = 3;

        }

        public void MoverD()
        {

            Pos.X += VelMov;
            PosI.X += 0.2f;

            Estado = 1;


        }

        public void MoverAr()
        {
            Pos.Y -= VelMov;
            PosI.Y -= 0.2f;

                Estado = 4;
        }

        public void MoverAb()
        {
            Pos.Y += VelMov;
            PosI.Y += 0.2f;

            Estado = 2;
        }

        public void Detenerse()
        {
            Estado = 0;
        }

        public void TeleporI()
        {
            Pos.X = Pos.X - Grid.GetLength(1) + 2;
        }

        public void TeleporD()
        {
            Pos.X = Pos.X + Grid.GetLength(1) - 2;
        }

        public Vector2 GetPos()
        {
            return Pos;
        }


    }
}