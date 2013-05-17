using System;
using System.IO;
using System.Collections;
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
    public class Mapa : Microsoft.Xna.Framework.GameComponent
    {
        private Texture2D[] Board;
        private Vector2 PosV;
        private int Frame;

        private int[,] Pos;

        private Punticos P;
        
        private Pacman Jugador;
        private Vector2 PosJ;
        private int Velpacman = 10; //Velocidad de Actualizacion entre sprites 

        private Escrito TEX;
        private Vector2 PosTex;
        private int Niv;

        private Fantasmas Enemigo1;
        private Fantasmas Enemigo2;
        private Fantasmas Enemigo3;
        private Fantasmas Enemigo4;

        private bool JugadorVivo;
        private bool JugadorVivoDameTime;

        private int PhantonKiller; //Combo Fantasma
        private bool FrutiManiac;   //Combo Frutas
        private int PuntajeGeneral;//Puntaje del Nivel

        private bool TodoCatch;
        private float TiempoCelebracion;

        public Mapa(Game game)
            : base(game)
        {

            //Cambio entre XNA 2.0 y 4.0 
            //string filename = "Content/Grid.txt";
            //string path = Path.Combine(StorageContainer.TitleLocation, filename);
            string path = game.Content.RootDirectory + "/Grid.txt";
            Pos = new int[31, 28];

            int i = 0;
            int j = 0;
            string line = "";
            using(StreamReader tr = new StreamReader(path)){
                line = tr.ReadLine();

                while (line != null)
                {
                    foreach (char c in line)
                    {
                        if (c == '1')
                        {
                            Pos[i, j] = 1;
                        }
                        else if (c == '0')
                        {
                            Pos[i, j] = 0;
                        }
                        else if (c == '2')
                        {
                            Pos[i, j] = 2;
                        }
                        else if (c == '3')
                        {
                            Pos[i, j] = 3;
                        }
                        else if (c == '4')
                        {
                            Pos[i, j] = 4;
                        }
                        else if (c == '5')
                        {
                            Pos[i, j] = 5;
                        }
                        else if (c == '6')
                        {
                            Pos[i, j] = 6;
                        }
                        else if (c == ' ')
                            j = j - 1;

                        j++;
                    }
                    j = 0;
                    i++;
                    line = tr.ReadLine();
                }
            }
        }

        public void LoadWorld(ContentManager content, string Dir,int Nivel,int CantPuntos)
        {

            FrutiManiac = false;
            PuntajeGeneral = CantPuntos;
            Board = new Texture2D[] {
                content.Load< Texture2D >(Dir),
                content.Load<Texture2D>(Dir+"2"),
                content.Load<Texture2D>(Dir+"Power"),
            };
            Frame = 0;

            PosV = new Vector2(100, 50);
            P = new Punticos(Pos, Board[0],PosV);
            P.Load(content, "Imagenes/Crump", "Imagenes/PowerPill",Nivel);

            Jugador = new Pacman(Pos,Velpacman);
            Jugador.Load(content);
            PosJ = Jugador.GetPos();
            JugadorVivo = true;
            JugadorVivoDameTime = true;
            
            TEX = new Escrito(content, "Fuentes/SF1");
            Niv = Nivel;
            TodoCatch = false;
            TiempoCelebracion = 3.0f;

            Enemigo1 = new Fantasmas(Pos, 8, Color.Red);
            Enemigo2 = new Fantasmas(Pos, 8, Color.Green);
            Enemigo3 = new Fantasmas(Pos, 8, Color.Purple);
            Enemigo4 = new Fantasmas(Pos, 8, Color.Yellow);
            Enemigo1.Load(content, 0.5f + Nivel * 0.03f);
            Enemigo2.Load(content, 0.6f + Nivel * 0.03f);
            Enemigo3.Load(content, 0.7f + Nivel * 0.03f);
            Enemigo4.Load(content, 0.8f + Nivel * 0.03f);
        }

        public void ReEspam(ContentManager content)
        {
            Jugador = new Pacman(Pos, Velpacman);
            Jugador.Load(content);
            PosJ = Jugador.GetPos();
            JugadorVivo = true;
            JugadorVivoDameTime = true;
        }

        public bool ComeFruta()
        {
            return FrutiManiac;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void DetectarMov()
        {
            int Up;
            int Dw;
            int Rg;
            int Lf;

            PosJ = Jugador.GetPos();
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                try
                {
                    Rg = Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) + 1];
                }
                catch (IndexOutOfRangeException)
                {
                    Rg = Pos[(int)Math.Floor(PosJ.Y), 27];
                }

                if (Rg == 1)
                {
                    Jugador.MoverD();
                    Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) + 1] = 4;
                    PuntajeGeneral += 10;
                }
                else if (Rg == 6)
                {
                    Jugador.TeleporI();
                }
                else if (Rg == 3)
                {
                    Jugador.MoverD();
                    Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) + 1] = 4;
                    Fantasmas.SetCagao();
                    PhantonKiller = 0;
                    PuntajeGeneral += 30;
                    Frame = 2;
                }
                else if (Rg == 4 || Rg == 5)
                {
                    Jugador.MoverD();
                }
                else
                    Jugador.Detenerse();

                P.ActualizarMatriz(Pos);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                try
                {
                    Lf = Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    Lf = Pos[(int)Math.Floor(PosJ.Y), 0];
                }

                if (Lf == 1)
                {
                    Jugador.MoverI();
                    Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) - 1] = 4;
                    PuntajeGeneral += 10;
                }
                else if (Lf == 6)
                {
                    Jugador.TeleporD();
                }
                else if (Lf == 3)
                {
                    Jugador.MoverI();
                    Pos[(int)Math.Floor(PosJ.Y), (int)Math.Floor(PosJ.X) - 1] = 4;
                    Fantasmas.SetCagao();
                    PhantonKiller = 0;
                    PuntajeGeneral += 30;
                    Frame = 2;
                }
                else if (Lf == 4 || Lf == 5)
                {
                    Jugador.MoverI();
                }
                else
                    Jugador.Detenerse();

                P.ActualizarMatriz(Pos);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                try
                {
                    Dw = Pos[(int)Math.Floor(PosJ.Y) + 1, (int)Math.Floor(PosJ.X)];
                }
                catch (IndexOutOfRangeException)
                {
                    Dw = 0;
                }
                if ( Dw== 1)
                {
                    Jugador.MoverAb();
                    Pos[(int)Math.Floor(PosJ.Y) + 1, (int)Math.Floor(PosJ.X)] = 4;
                    PuntajeGeneral += 10;
                }
                else if (Dw == 3)
                {
                    Jugador.MoverAb();
                    Pos[(int)Math.Floor(PosJ.Y) + 1, (int)Math.Floor(PosJ.X)] = 4;
                    Fantasmas.SetCagao();
                    PhantonKiller = 0;
                    PuntajeGeneral += 30;
                    Frame = 2;
                }
                else if (Dw == 4 || Dw == 5)
                {
                    Jugador.MoverAb();
                }
                else
                    Jugador.Detenerse();

                P.ActualizarMatriz(Pos);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                try
                {
                    Up = Pos[(int)Math.Floor(PosJ.Y) - 1, (int)Math.Floor(PosJ.X)];
                }
                catch (IndexOutOfRangeException)
                {
                    Up = 0;
                }
                if ( Up == 1)
                {
                    Jugador.MoverAr();
                    Pos[(int)Math.Floor(PosJ.Y) - 1, (int)Math.Floor(PosJ.X)] = 4;
                    PuntajeGeneral += 10;
                }
                else if (Up == 3)
                {
                    Jugador.MoverAr();
                    Pos[(int)Math.Floor(PosJ.Y) - 1, (int)Math.Floor(PosJ.X)] = 4;
                    Fantasmas.SetCagao();
                    PhantonKiller = 0;
                    PuntajeGeneral += 30;
                    Frame = 2;
                }
                else if (Up == 4 || Up == 5)
                {
                    Jugador.MoverAr();
                }
                else
                    Jugador.Detenerse();

                P.ActualizarMatriz(Pos);

            }
        }

        public void DetectarCol()
        {
            Vector2 CJ = Jugador.GetPos();
            Vector2 CE1 = Enemigo1.GetPos();
            Vector2 CE2 = Enemigo2.GetPos();
            Vector2 CE3 = Enemigo3.GetPos();
            Vector2 CE4 = Enemigo4.GetPos();
            Vector2 CB = P.PosicionBono();   //No es el i y j es la posicion en el board ya

            float Al = Board[0].Height/Pos.GetLength(0);
            float An = Board[0].Width / Pos.GetLength(1);

            
            BoundingSphere J = new BoundingSphere(new Vector3(CJ.X * An + An / 2 + PosV.X,
                                                              CJ.Y * Al + Al / 2 + PosV.Y,
                                                              0), An / 2);
            
            BoundingBox E1 = new BoundingBox(new Vector3(CE1.X * An + PosV.X, CE1.Y * Al + PosV.Y,0),
                                             new Vector3(CE1.X * An + PosV.X + Enemigo1.GETTEXTURE().Width, 
                                                         CE1.Y * Al + PosV.Y + Enemigo1.GETTEXTURE().Height, 0));

            BoundingBox E2 = new BoundingBox(new Vector3(CE2.X * An + PosV.X, CE2.Y * Al + PosV.Y, 0),
                                             new Vector3(CE2.X * An + PosV.X + Enemigo2.GETTEXTURE().Width,
                                                         CE2.Y * Al + PosV.Y + Enemigo2.GETTEXTURE().Height, 0));

            BoundingBox E3 = new BoundingBox(new Vector3(CE3.X * An + PosV.X, CE3.Y * Al + PosV.Y, 0),
                                             new Vector3(CE3.X * An + PosV.X + Enemigo3.GETTEXTURE().Width,
                                                         CE3.Y * Al + PosV.Y + Enemigo3.GETTEXTURE().Height, 0));

            BoundingBox E4 = new BoundingBox(new Vector3(CE4.X * An + PosV.X, CE4.Y * Al + PosV.Y, 0),
                                             new Vector3(CE4.X * An + PosV.X + Enemigo4.GETTEXTURE().Width,
                                                         CE4.Y * Al + PosV.Y + Enemigo4.GETTEXTURE().Height, 0));

            BoundingBox BN = new BoundingBox(new Vector3(CB.X, CB.Y, 0),
                                             new Vector3(CB.X + P.GETTEXTURABONO().Width, 
                                                         CB.Y + P.GETTEXTURABONO().Height, 0));


            if (J.Intersects(E1)||J.Intersects(E2)||J.Intersects(E3)||J.Intersects(E4))
            {
                if (Fantasmas.getCagao())
                {
                    if (J.Intersects(E1) && !(Enemigo1.GetDelete()))
                    {
                        Enemigo1.DeletFant();
                        PhantonKiller += 200;
                        PuntajeGeneral += PhantonKiller;
                    }
                    else if (J.Intersects(E2) && !(Enemigo2.GetDelete()))
                    {
                        Enemigo2.DeletFant();
                        PhantonKiller += 200;
                        PuntajeGeneral += PhantonKiller;
                    }
                    else if (J.Intersects(E3) && !(Enemigo3.GetDelete()))
                    {
                        Enemigo3.DeletFant();
                        PhantonKiller += 200;
                        PuntajeGeneral += PhantonKiller;
                    }
                    else if (J.Intersects(E4) && !(Enemigo4.GetDelete()))
                    {
                        Enemigo4.DeletFant();
                        PhantonKiller += 200;
                        PuntajeGeneral += PhantonKiller;
                    }

                    
                }
                else
                    JugadorVivo = false;
            }

            if (J.Intersects(BN) && !(FrutiManiac))
            {
                P.setComida();
                FrutiManiac = true;
                PuntajeGeneral += Niv * 100;
            }


        }

        public override void Update(GameTime gameTime)
        {
            float Tiempo = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (P.NivelTerminado())
            {
                UpdateWin(Tiempo);
            }

            P.Update(Tiempo);

            Enemigo1.UpdateMov(Tiempo);
            Enemigo2.UpdateMov(Tiempo);
            Enemigo3.UpdateMov(Tiempo);
            Enemigo4.UpdateMov(Tiempo);

            Fantasmas.UpdateFrame(Tiempo);

            if (JugadorVivo)
            {
                DetectarMov();
                if (TRANSFORMACION(Tiempo))
                    Jugador.UpdateframeB(Tiempo);
                else
                    Jugador.UpdateFrame(Tiempo);

                DetectarCol();
            }
            else
            {
                JugadorVivoDameTime = !Jugador.AnimDead(Tiempo);
            }

            if (!(Fantasmas.getCagao()))
            {
                PhantonKiller = 0;
                Frame = 0;
            }
            

            base.Update(gameTime);
        }

        float TIEMPOT = 0;
        float TIME = 7.5f;
        bool PAR = true;

        private bool TRANSFORMACION(float T)
        {
            
            bool C = Fantasmas.getCagao();

            if (Fantasmas.getCagao())
            {
                TIEMPOT += T;

                if (TIEMPOT > TIME * 3/4)
                {
                    if (PAR)
                    {
                        Frame = 0;
                        PAR = false;
                    }
                    else
                    {
                        PAR = true;
                        Frame = 2;
                    }
                    TIEMPOT -= 0.5f;

                }
            }
            else
                TIEMPOT = 0;

            return C;
        }

        public void UpdateWin(float gameTime)
        {

            Frame++;
            Frame = Frame % 2;
            TiempoCelebracion -= gameTime;

            TodoCatch = (TiempoCelebracion <= 0);
            

            
        }

        public void Draw(SpriteBatch batch) 
        {

            if (P.NivelTerminado())
            {
                batch.Draw(Board[Frame], PosV, Color.White);
                PosTex = new Vector2(PosV.X + 10, PosV.Y - 20);
                TEX.Load("Puntaje Jugador: " + PuntajeGeneral +
                         "                                                                                                      " +
                         "Nivel: " + Niv,
                         PosTex);
                TEX.Draw(batch);
            }
            else
            {
                batch.Draw(Board[Frame], PosV, Color.White);
                P.Draw(batch);
                Jugador.Draw(batch, PosV, Board[0]);

                Enemigo1.Draw(batch, PosV, Board[0]);
                Enemigo2.Draw(batch, PosV, Board[0]);
                Enemigo3.Draw(batch, PosV, Board[0]);
                Enemigo4.Draw(batch, PosV, Board[0]);

                PosTex = new Vector2(PosV.X + 10, PosV.Y - 20);
                TEX.Load("Puntaje Jugador: " + PuntajeGeneral +
                         "                                                                                                      " +
                         "Nivel: " + Niv,
                         PosTex);
                TEX.Draw(batch);
            }

        }

        public bool ItsAlive()
        {
            return JugadorVivoDameTime;
        }

        public int PuntosAcum()
        {
            return PuntajeGeneral;
        }

        public bool NivelTerminado()
        {
            return TodoCatch;
        }

    }
}