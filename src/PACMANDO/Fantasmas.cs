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

    public class Fantasmas
    {

        private Texture2D[] Bfantasma;

        public Texture2D GETTEXTURE()
        {
            return Bfantasma[0];
        }

        private Vector2 posicion;
        private static float TPFrame; // Frecuencia de actualizacion del fantasma
        private static float TTotal;
        private static int Frame;

        private Vector2 Hacia;

        private float VelFan;          // Velocidad del Fantasma
        private float TiempoMov=1.0f/10f;   // Frecuencia de tiempo en que se moveran los fantasmas
        private float TiempoT=0;

        private Color color;

        private int[,] Grid;

        private bool MovFant = false; // Si el fantasma se esta moviendo
        private int Dir = 0; //Direccion de movimiento

        private static bool Cagao;   //si se come la powerpill
        private static float TiempoCagao;   //Tiempo de duracion Peligro
        private static float TiempoCagTrans;//Tiempo acumulado en estado cagao
        private Color[] ColorCagao;
        private static int atenuo;

        private bool Delete;

        public Fantasmas(int[,] GRD,int FramesXseg,Color Col)
        {
            Hacia = new Vector2(0, 0);
            Grid = GRD;
            TPFrame = 1.0f/FramesXseg;
            TTotal = 0;
            Frame = 0;
            TiempoCagao = TPFrame * 60;
            TiempoCagTrans = 0;
            color = Col;
            ColorCagao = new Color[] {
                Color.Blue,
                Color.White,
            };

            Delete = false;
            Cagao = false;
        }

        public void Load(ContentManager content,float Vel)
        {
            Bfantasma = new Texture2D[] {
                content.Load<Texture2D>("Imagenes/GohstBase"),// 0 - Fantasma Base
                content.Load<Texture2D>("Imagenes/GohstBase2"),// 1 - Fantasma Base2 
                content.Load<Texture2D>("Imagenes/GohstBase3"),// 2 - Fantasma Base2 (Despues de la mordida)
                content.Load<Texture2D>("Imagenes/GhostEyes"),// 3 - Ojos Enfadados (Normales)
                content.Load<Texture2D>("Imagenes/GhostEyesAsustado"),// 4 - Ojos Abiertos (Asustada)
                content.Load<Texture2D>("Imagenes/GhostEyesCenter"),// 5 - Centro de los Ojos 
            };
            VelFan = Vel;
            LocFant();
        }

        private void LocFant()
        {
            Random Ram = new Random();
            int loc = (int)Math.Floor((double)Ram.Next(2,20));
            int fnd = 0;
            bool find = false;
            for (int i = 0; i < Grid.GetLength(0) && !find; i++)
            {
                for (int j = 0; j < Grid.GetLength(1) && !find; j++)
                {
                    if (Grid[i, j] == 2)
                    {
                        if (fnd == loc)
                        {
                            find = true;
                            posicion.X = j;
                            posicion.Y = i;
                        }
                        else
                        {
                            fnd++;
                        }
                    }
                }
            }
        }

        public static void UpdateFrame(float Time)
        {
            TTotal += Time;
            if (TTotal > TPFrame)
            {
                Frame++; 
                Frame = Frame % 2;
                atenuo = 1;
                TTotal -= TPFrame;
            }

            if (Cagao)
            {
                TiempoCagTrans = TiempoCagTrans + Time;
            }

            if (TiempoCagao < TiempoCagTrans)
            {
                Cagao = false;
                TiempoCagTrans = 0;
            }
            else if (TiempoCagao * 3/4 < TiempoCagTrans)
            {
                atenuo = (Frame + 1) % 2; 
            }

            
 


        }

        public void Draw(SpriteBatch batch, Vector2 Base, Texture2D Tex)
        {
            int Alto  = Tex.Height/Grid.GetLength(0);
            int Ancho = Tex.Width/Grid.GetLength(1);

            Vector2 Bf = new Vector2((int)(Base.X + Ancho * Math.Floor(posicion.X)),(int)(Base.Y + Alto * Math.Floor(posicion.Y))); // Base Del Fantasma

            if (!Delete)
            {
                Vector2 Bo = new Vector2(Bf.X + Ancho / 2, Bf.Y + Alto * 2/3 ); //Base de Los Ojos
                Vector2 Co = new Vector2(Hacia.X + Bo.X + Ancho / 8, Hacia.Y + Bo.Y + Alto / 4); //Centro de los Ojos
                if (!Cagao)
                {
                    
                    batch.Draw(Bfantasma[Frame], Bf, Color.White);
                    batch.Draw(Bfantasma[3], Bo, Color.White);
                    batch.Draw(Bfantasma[5], Co, Color.White);
                }
                else
                {
                    batch.Draw(Bfantasma[Frame], Bf, Color.White);
                    batch.Draw(Bfantasma[4], Bo, Color.White);
                    batch.Draw(Bfantasma[5], Co, Color.White);
                }
            }
            else
            {
                if (Cagao)
                {
                    Vector2 Bo = new Vector2(Bf.X + Ancho / 4, Bf.Y + Alto / 4); //Base de Los Ojos
                    Vector2 Co = new Vector2(Hacia.X + Bo.X + Ancho / 8, Hacia.Y + Bo.Y + Alto / 4); //Centro de los Ojos
                    batch.Draw(Bfantasma[2], Bo, Color.White);

                }
                else
                {
                    Delete = false;

                }
            }

        }

        public static void SetCagao()
        {
            Cagao = true;
        }

        public static bool getCagao()
        {
            return Cagao;
        }

        public bool GetDelete()
        {
            return Delete;
        }

        public void DeletFant()
        {
            //Algoritmo pa que valla pal centro



            Delete = true;
        }

        // Aqui va lo que no se hacer Que los fantasmas se muevan
        public void MoverAr()
        {
            posicion.Y -= VelFan;
            Hacia.X = 0;
            Hacia.Y = -2;
                
                
        }

        public void MoverAb()
        {
            posicion.Y += VelFan;
            Hacia.X = 0;
            Hacia.Y = 2;
        }

        public void MoverI()
        {
            posicion.X -= VelFan;
            Hacia.X = -2;
            Hacia.Y = 0;
        }

        public void MoverD()
        {
            posicion.X += VelFan;
            Hacia.X = 2;
            Hacia.Y = 0;
        }

        public void UpdateMov(float Time)
        {
            TiempoT += Time;
            if (TiempoT > TiempoMov)
            {
                int Up = Grid[(int)Math.Floor(posicion.Y) - 1, (int)Math.Floor(posicion.X)];
                int Dw = Grid[(int)Math.Floor(posicion.Y) + 1, (int)Math.Floor(posicion.X)];
                int Rg = Grid[(int)Math.Floor(posicion.Y), (int)Math.Floor(posicion.X) + 1];
                int Lf = Grid[(int)Math.Floor(posicion.Y), (int)Math.Floor(posicion.X) - 1];

                int[] Ar = new int[] {1,2,3,4};

                if ( Up == 0 || Up == 6) // ^
                {
                    Ar[2] = 5;
                }
                if ( Dw== 0 || Dw==6 )// V
                {
                    Ar[3] = 5;
                }
                if ( Lf== 0 || Lf== 6)// <-
                {
                    Ar[0] = 5;                
                }
                if (Rg==0 || Rg ==6)// ->
                {
                    Ar[1] = 5;
                }

                Random Azar = new Random();
                
                if (!MovFant || Azar.NextDouble() >0.8)
                {
                    Dir = (int)Math.Floor((double)Azar.Next(4));
                }

                while (Ar[Dir] == 5)
                {
                    Dir = (int)Math.Floor((double)Azar.Next(4));
                }


                if (Ar[Dir] == 1)
                {
                    MoverI();
                    MovFant = true;
                }
                else if (Ar[Dir] == 2)
                {
                    MoverD();
                    MovFant = true;
                }
                else if (Ar[Dir] == 3)
                {
                    MoverAr();
                    MovFant = true;
                }
                else if (Ar[Dir] == 4)
                {
                    MoverAb();
                    MovFant = true;
                }

                TiempoT -= TiempoMov;
            }

        }

        public Vector2 GetPos()
        {
            return posicion;
        }


            
            
        

    }

}