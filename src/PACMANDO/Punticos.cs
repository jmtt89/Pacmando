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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace WindowsGame3
{
 
    public class Punticos
    {

        Texture2D Crump;
        Vector2 Rec;
        Texture2D SCrump;
        int[,] Pos;
        float DimX;
        float DimY;

        Texture2D Bono;

        public Texture2D GETTEXTURABONO()
        {
            return Bono;
        }

        Vector2 PosBono;
        int BonusNiv;
        bool Comida;

        bool aparece;
        float TDD;
        float TTotal;
        float TDC;

        public Punticos(int[,] A,Texture2D AUX,Vector2 P)
        {
            Pos = A;
            DimX = AUX.Width/A.GetLength(1);
            DimY = AUX.Height/A.GetLength(0);
            Rec = P;
            Comida = false;
            aparece = false;

            TDC = 15;
            TTotal = 0;
            TDD = 5;
        }

        public void setComida()
        {
            Comida = true;
        }

        public Vector2 PosicionBono()
        {
            return PosBono;
        }

        public void Load(ContentManager content, string Dir, string Dir2,int Nivel)
        {
            Crump = content.Load<Texture2D>(Dir);
            SCrump = content.Load<Texture2D>(Dir2);
            BonusNiv = (Nivel-1) % 9;
            Bono = content.Load<Texture2D>("Bonos/"+BonusNiv);


        }

        public void ActualizarMatriz(int[,] MAT)
        {
            Pos = MAT;
        }

        public bool NivelTerminado()
        {
            bool Quedan = false;

            for (int i = 0; i < Pos.GetLength(0) && !Quedan; i++)
            {
                for (int j = 0; j < Pos.GetLength(1) && !Quedan; j++)
                {
                    if (Pos[i, j] == 1 || Pos[i, j] == 3)
                        Quedan = true;
                }

            }

            return !Quedan;
        }

        public void Update(float Tiempo)
        {
            TTotal += Tiempo;

            if (!aparece)
            {
                PosBono = new Vector2(0, 0);
            }

            if (TDD < TTotal && TTotal < TDC)
            {
                aparece = true;

            }
            else
            {
                aparece = false;
            }

        }

        public void Draw(SpriteBatch batch)
        {
            int count = 0;
            for (int i = 0; i < Pos.GetLength(0); i++)
            {
                for (int j = 0; j < Pos.GetLength(1); j++)
                {
                    if (Pos[i, j] == 1)
                    {
                        Vector2 sourceCrump = new Vector2((int)(Rec.X + DimX * j + DimX / 2), (int)(Rec.Y + DimY * i + DimX / 2));
                        batch.Draw(Crump, sourceCrump, Color.White);
                    }
                    if (Pos[i, j] == 3)
                    {
                        Vector2 sourceCrump = new Vector2((int)(Rec.X + DimX * j + DimX / 2), (int)(Rec.Y + DimY * i + DimX / 2));
                        batch.Draw(SCrump, sourceCrump, Color.White);
                      
                    }
                    if (Pos[i, j] == 2 && !Comida && aparece)
                    {
                        if (count == 16)
                        {
                            
                            PosBono = new Vector2((int)(Rec.X + DimX * j ), (int)(Rec.Y + DimY * (i+2))-5);
                            batch.Draw(Bono, PosBono, Color.White);
                        }
                        count++;
                    }


                }
            }


        }
    }
}