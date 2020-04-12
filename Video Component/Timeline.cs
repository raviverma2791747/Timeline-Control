using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Video_Component
{
    public partial class Timeline : Control
    {
        static class Drag
        {
            public static Point InitialLocation;
            public static Point FinalLocation;
            public static int InitialTrack;
            public static int FinalTrack;
            public static int InitialTrackObject;
            public static int FinalTrackObject;
            public static bool MoveNS;
            public static bool MoveWE;
            public static bool SizeN;
            public static bool SizeS;
            public static bool SizeW;
            public static bool SizeE;
            public static bool SizeNS;
            public static bool Track;
            public static bool Seek;
            public static int ChangeNS;
            public static int ChangeWE;
            public static TrackObject TO;
            public static Track T;

            static Drag()
            {
                InitialLocation = new Point();
                FinalLocation = new Point();
                InitialTrack = 0;
                FinalTrack = 0;
                InitialTrackObject = 0;
                FinalTrackObject = 0;
                MoveNS = false;
                MoveWE = false;
                SizeW = false;
                SizeE = false;
                SizeNS = false;
                Track = false;
                Seek = false;
                ChangeNS = 0;
                ChangeWE = 0;

            }

            public static void Refresh()
            {
                InitialLocation = new Point();
                FinalLocation = new Point();
                InitialTrack = 0;
                FinalTrack = 0;
                InitialTrackObject = 0;
                FinalTrackObject = 0;
                MoveNS = false;
                MoveWE = false;
                SizeW = false;
                SizeE = false;
                SizeNS = false;
                Track = false;
                Seek = false;
                ChangeNS = 0;
                ChangeWE = 0;
            }

            public static void Change()
            {
                ChangeNS = FinalLocation.Y - InitialLocation.Y;
                ChangeWE = FinalLocation.X - InitialLocation.X;
            }
        }

        class TopBar
        {
            public Point Location;
            public Size Size;
            private Rectangle Base;
            private SolidBrush BackColor;
            public Button Undo;
            public Button Redo;
            public Button Delete;
            public Button Cut;
            public Button Mic;

            public TopBar()
            {
                this.Location = new Point(0, 0);
                this.Size = new Size(1400, 30);
                this.Base = new Rectangle(this.Location, this.Size);
                this.BackColor = new SolidBrush(Color.FromArgb(26, 26, 26));
                this.Undo = new Button();
                this.Redo = new Button();
                this.Delete = new Button();
                this.Cut = new Button();
                this.Mic = new Button();
                //Initialize Button

                //Undo Button
                this.Undo.Name = "Undo";
                this.Undo.Text = "Undo";
                this.Undo.Enabled = true;
                this.Undo.Location = new Point(0, 0);
                this.Undo.Height = this.Size.Height;

                //Redo Button
                this.Redo.Name = "Redo";
                this.Redo.Text = "Redo";
                this.Redo.Enabled = true;
                this.Redo.Location = new Point(this.Undo.Width, 0);
                this.Redo.Height = this.Size.Height;

                //Delete Button
                this.Delete.Name = "Delete";
                this.Delete.Text = "Delete";
                this.Delete.Enabled = true;
                this.Delete.Location = new Point(this.Undo.Width + this.Redo.Width, 0);
                this.Delete.Height = this.Size.Height;

                //Cut Button
                this.Cut.Name = "Cut";
                this.Cut.Text = "Cut";
                this.Cut.Enabled = true;
                this.Cut.Location = new Point(this.Undo.Width + this.Redo.Width + this.Delete.Width, 0);
                this.Cut.Height = this.Size.Height;

                this.Mic.Name = "Mic";
                this.Mic.Text = "Mic";
                this.Mic.Enabled = true;
                this.Mic.Location = new Point(this.Size.Width - this.Undo.Width, 0);
                this.Mic.Height = this.Size.Height;

            }

            public TopBar(Point Location, Size Size)
            {
                this.Location = Location;
                this.Size = Size; ;
                this.Base = new Rectangle(this.Location, this.Size);
                this.BackColor = new SolidBrush(Color.Red);
                this.Undo = new Button();
                this.Redo = new Button();
                this.Delete = new Button();
                this.Cut = new Button();
                this.Mic = new Button();

            }


            public void OnPaint(PaintEventArgs pe)
            {
                /*Update Region*/
                //this.Size.Width 
                //this.Base = Si
                //end
                /*Paint Region*/
                pe.Graphics.FillRectangle(BackColor, Base);
                //end
            }
        }

        public class Scale
        {
            public static class Seek
            {
                public static Point Location;
                public static Size Size;
                public static int Value;
                private static Rectangle Base;
                private static SolidBrush BaseColor;
                private static Point[] SeekHead;

                static Seek()
                {
                    Location = new Point(0, 0);
                    Size = new Size(2, 20);
                    Value = 0;
                    Base = new Rectangle(Location, Size);
                    BaseColor = new SolidBrush(Color.BlueViolet);
                    SeekHead = new Point[] { new Point(Location.X, Location.Y), new Point(Location.X + 10, Location.Y), new Point(Location.X + 10, Location.Y + 20), new Point(Location.X + 5, Location.Y + 30), new Point(Location.X, Location.Y + 20) };
                }

                public static void OnPaint(PaintEventArgs pe)
                {
                    Update();
                    pe.Graphics.FillPolygon(BaseColor, SeekHead);
                    //pe.Graphics.FillRectangle(BaseColor, Base);
                }

                public static void Update()
                {
                    SeekHead[0] = new Point(Location.X, Location.Y);
                    SeekHead[1] = new Point(Location.X + 10, Location.Y);
                    SeekHead[2] = new Point(Location.X + 10, Location.Y + 20);
                    SeekHead[3] = new Point(Location.X + 5, Location.Y + 30);
                    SeekHead[4] = new Point(Location.X, Location.Y + 20);
                    Base.Location = Location;
                }

                public static bool Contains(Point P)
                {
                    Rectangle SH = new Rectangle(Seek.Location, new Size(10, 20));
                    if (SH.Contains(P))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public Point Location;
            public Size Size;
            int Unit;
            int HalfUnit;
            int TenthUnit;
            Pen Pen;

            public Scale()
            {
                this.Location = new Point(0, 0);
                this.Size = new Size(500, 20);
                this.Unit = 100;
                this.HalfUnit = Unit / 2;
                this.TenthUnit = Unit / 10;
                this.Pen = new Pen(Color.Black);
            }

            public Scale(Point Location, Size Size)
            {
                this.Location = Location;
                this.Size = Size;
                this.Unit = 100;
                this.HalfUnit = Unit / 2;
                this.TenthUnit = Unit / 10;
                this.Pen = new Pen(Color.Black);
            }

            public Scale(int X, int Y, int Width, int Height)
            {
                this.Location = new Point(X, Y);
                this.Size = new Size(Width, Height);
                this.Unit = 100;
                this.HalfUnit = Unit / 2;
                this.TenthUnit = Unit / 10;
                this.Pen = new Pen(Color.Black);
                Seek.Location = new Point(this.Location.X + 75,this.Location.Y);
            }

            public void OnPaint(PaintEventArgs pe)
            {
                Point Current = new Point(this.Location.X+80,this.Location.Y);
                Current.Y += Size.Height;
                SolidBrush B = new SolidBrush(Color.Blue);
                for (int i = 0; i < Size.Width - 80 / Unit; i++)
                {
                    pe.Graphics.DrawLine(Pen, Current.X, Current.Y, Current.X + Unit, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X, Current.Y - Size.Height, Current.X, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + HalfUnit, Current.Y - Size.Height / 2, Current.X + HalfUnit, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit, Current.Y - Size.Height / 3, Current.X + TenthUnit, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 2, Current.Y - Size.Height / 3, Current.X + TenthUnit * 2, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 3, Current.Y - Size.Height / 3, Current.X + TenthUnit * 3, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 4, Current.Y - Size.Height / 3, Current.X + TenthUnit * 4, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 6, Current.Y - Size.Height / 3, Current.X + TenthUnit * 6, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 7, Current.Y - Size.Height / 3, Current.X + TenthUnit * 7, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 8, Current.Y - Size.Height / 3, Current.X + TenthUnit * 8, Current.Y);
                    pe.Graphics.DrawLine(Pen, Current.X + TenthUnit * 9, Current.Y - Size.Height / 3, Current.X + TenthUnit * 9, Current.Y);
                    Current.X += Unit;
                }
                Scale.Seek.OnPaint(pe);
            }
        }

        class TrackObject
        {
            public Point Location;
            public static int Height;
            public int Width;
            private Rectangle Base;
            private Rectangle Border;
            public String Type;
            public SolidBrush BaseColor;
            public SolidBrush BorderColor;


            public TrackObject()
            {
                this.Location = new Point(80, 0);
                TrackObject.Height = Track.Size.Height;
                this.Width = 100;
                this.BaseColor = new SolidBrush(Color.Yellow);
                this.BorderColor = new SolidBrush(Color.Black);
                this.Base = new Rectangle(this.Location.X, this.Location.Y, this.Width, TrackObject.Height);
                this.Border = new Rectangle(this.Location.X + 1, this.Location.Y + 1, this.Width - 1, TrackObject.Height - 1);
                this.Type = "Default";

            }

            public TrackObject(Point Location)
            {
                this.Location = Location;
                TrackObject.Height = Track.Size.Height;
                this.Width = 100;
                this.BaseColor = new SolidBrush(Color.Yellow);
                this.BorderColor = new SolidBrush(Color.Black);
                this.Base = new Rectangle(this.Location.X, this.Location.Y, this.Width, TrackObject.Height);
                this.Border = new Rectangle(this.Location.X + 1, this.Location.Y + 1, this.Width - 1, TrackObject.Height - 1);
                this.Type = "Default";
            }

            public bool Contains(Point P)
            {
                if (this.Base.Contains(P))
                    return true;
                else
                    return false;
            }

            public bool ContainsBL(Point P)
            {
                Rectangle BL = new Rectangle(this.Location.X, this.Location.Y, 4, TrackObject.Height - 2);
                if (BL.Contains(P))
                    return true;
                else
                    return false;
            }
            public bool ContainsBR(Point P)
            {
                Rectangle BR = new Rectangle(this.Location.X + this.Width - 4, this.Location.Y, 4, TrackObject.Height - 2);
                if (BR.Contains(P))
                    return true;
                else
                    return false;
            }

            public void SizeBL(int ChangeBL)
            {
                this.Location.X = this.Location.X + ChangeBL;
                this.Width = this.Width + (-1) * ChangeBL;
                this.Update();
            }

            public void SizeBR(int ChangeBR)
            {
                this.Width = this.Width + ChangeBR;
                this.Update();
            }

            public void MoveWE(int MoveWE)
            {
                this.Location.X = this.Location.X + MoveWE;
                if (this.Location.X < 80)
                {
                    this.Location.X = 80;
                }
                /*else if(this.Location.X > this.Width - 80)
                {
                    this.Location.X = this.Width - 80;
                }*/
                this.Update();
            }

            public void OnPaint(PaintEventArgs pe)
            {
                Pen P = new Pen(this.BorderColor);
                P.Width = 2;
                pe.Graphics.FillRectangle(this.BaseColor, this.Base);
                pe.Graphics.DrawRectangle(P, this.Border);
            }

            public void Update()
            {
                TrackObject.Height = Track.Size.Height;
                this.Base.Location = this.Location;
                this.Base.Width = this.Width;
                this.Base.Height = TrackObject.Height;
                this.Border.Location = new Point(this.Location.X - 1, this.Location.Y - 1);
                this.Border.Height = TrackObject.Height - 1;
                this.Border.Width = this.Width - 1;
            }
        }

        class Track
        {
            public int Number;
            public static Point TracksLocation;
            public Point Location;
            public static Size Size;
            public List<TrackObject> TrackObjects;
            private Rectangle InfoBase;
            private Rectangle Base;
            public String Type;
            private SolidBrush BaseColor;
            private SolidBrush InfoColor;

            public Track()
            {

            }

            public Track(Point Location, int Number)
            {
                this.Location = Location;
                //this.Location.Y = this.Location.Y + 1;
                this.InfoBase = new Rectangle(this.Location, new Size(80, Track.Size.Height));
                this.Base = new Rectangle(this.Location.X + 80, this.Location.Y, Size.Width - 80, Size.Height);
                this.Type = "Default";
                this.BaseColor = new SolidBrush(Color.Gray);
                this.InfoColor = new SolidBrush(Color.White);
                this.TrackObjects = new List<TrackObject>();
                this.Number = Number;
            }

            public void Update()
            {
                this.Location.Y = TracksLocation.Y + (Number - 1) * Track.Size.Height;
                this.InfoBase.Location = this.Location;
                this.Base.Height = Track.Size.Height;
                this.Base.Location = new Point(80, this.Location.Y);
                this.InfoBase.Height = Track.Size.Height;
                for (int i = 0; i < TrackObjects.Count; i++)
                {
                    TrackObjects[i].Update();
                }
            }

            public void AddTrackObject()
            {
                if (TrackObjects.Count == 0)
                {
                    TrackObject T = new TrackObject(new Point(80, this.Location.Y));
                    TrackObjects.Add(T);
                }
            }
            public void AddTrackObject(TrackObject TO)
            {
                if (TrackObjects.Count == 0)
                {
                    TO.Location = new Point(TO.Location.X, this.Location.Y);
                    TrackObjects.Add(TO);
                }
            }

            public bool Contains(Point P)
            {
                if (this.Base.Contains(P))
                    return true;
                else
                    return false;
            }

            public bool ContainsBT(Point P)
            {
                Rectangle Top = new Rectangle(this.Location.X + 80, this.Location.Y, Track.Size.Width, 4);
                if (Top.Contains(P))
                    return true;
                else
                    return false;
            }

            public bool ContainsBD(Point P)
            {
                Rectangle Down = new Rectangle(this.Location.X + 80, this.Location.Y + Size.Height - 4, Track.Size.Width, 4);
                if (Down.Contains(P))
                    return true;
                else
                    return false;
            }

            public void OnPaint(PaintEventArgs pe)
            {
                this.Update();
                pe.Graphics.FillRectangle(this.InfoColor, this.InfoBase);
                //pe.Graphics.DrawString(this.Type,new Font(FontFamily.GenericSansSerif), new SolidBrush(Color.Black),);
                pe.Graphics.FillRectangle(this.BaseColor, this.Base);
                for (int i = 0; i < TrackObjects.Count; i++)
                {
                    TrackObjects[i].OnPaint(pe);
                }
            }
        }

        class BottomBar
        {
            public Point Location;
            public Size Size;
            public Button AddTrack;
            private Rectangle Base;
            private SolidBrush BackColor;

            public BottomBar(Point Location)
            {
                this.Location = Location;
                this.Size = new Size(1400, 30);
                this.Base = new Rectangle(this.Location, this.Size);
                this.BackColor = new SolidBrush(Color.FromArgb(26, 26, 26));
                this.AddTrack = new Button();
                //Initialize Button

                //Undo Button
                this.AddTrack.Name = "AddTrack";
                this.AddTrack.Text = "Add Track";
                this.AddTrack.Enabled = true;
                this.AddTrack.Location = this.Location;
                this.AddTrack.Height = this.Size.Height;
            }

            public void OnPaint(PaintEventArgs pe)
            {
                pe.Graphics.FillRectangle(this.BackColor, this.Base);
            }
        }

        //Data Members
        private TopBar _TopBar;
        private Scale _Scale;
        private BottomBar _BottomBar;
        private List<Track> Tracks;
        private ContextMenuStrip RightClickMenu;

        //Deployers
        private void DeployTopBar()
        {
            _TopBar = new TopBar();
            Controls.Add(_TopBar.Undo);
            Controls.Add(_TopBar.Redo);
            Controls.Add(_TopBar.Delete);
            Controls.Add(_TopBar.Cut);
            Controls.Add(_TopBar.Mic);
        }

        private void DeployScale()
        {
            if (_TopBar != null)
            {
                _Scale = new Scale(0, _TopBar.Size.Height, _TopBar.Size.Width, 30);
            }
            else
            {
                _Scale = new Scale();
            }
            this.MouseDown += SeekDownEvent;
            this.MouseMove += SeekMoveEvent;
            this.MouseUp += SeekUpEvent;
        }

        private void DeployTracks()
        {
            if (_Scale != null)
            {
                Tracks = new List<Track>();
                Track.TracksLocation = new Point(0, _Scale.Size.Height + _TopBar.Size.Height);
                Track.Size = new Size(_Scale.Size.Width, 80);
            }
            else
            {
                Tracks = new List<Track>();
                Track.TracksLocation = new Point(0, 0);
                Track.Size = new Size(1000, 50);
            }
            AddTrack();
            AddTrack();
            AddTrack();
            Tracks[0].AddTrackObject();
            //Tracks[1].AddTrackObject();
            //Tracks[2].AddTrackObject();
            this.MouseClick += RightClickTrackEvent;
            this.MouseDown += TrackMouseDownEvent;
            this.MouseMove += TrackMouseMoveEvent;
            this.MouseUp += TrackMouseUpEvent;
            this.MouseHover += TrackMouseHoverEvent;
            RightClickMenu = new ContextMenuStrip();
            RightClickMenu.Items.Add("Add Track");
            RightClickMenu.Items.Add("Add TrackObject");
            RightClickMenu.Items.Add("Remove Track");
            RightClickMenu.Items.Add("Add TrackObject");
        }

        public void DeployBottomBar()
        {
            _BottomBar = new BottomBar(new Point(0, this._Scale.Location.Y + this._Scale.Size.Height + Track.Size.Height * 3));
            _BottomBar.AddTrack.Click += AddTrackEvent;
            Controls.Add(_BottomBar.AddTrack);
        }
        //

        public void AddTrack()
        {
            Track T = new Track(new Point(0, Tracks.Count * Track.Size.Height + Track.TracksLocation.Y), Tracks.Count + 1);
            Tracks.Add(T);
        }


        //Events

        public void SeekDownEvent(object sender, MouseEventArgs e)
        {
            Point P = e.Location;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Scale.Seek.Contains(P))
                    {
                        Cursor.Current = Cursors.IBeam;
                        Drag.InitialLocation = P;
                        Drag.Seek = true;
                    }
                    break;
            }
        }

        public void SeekMoveEvent(object sender, MouseEventArgs e)
        {

        }

        public void SeekUpEvent(object sender, MouseEventArgs e)
        {
            Point P = e.Location;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Drag.Seek == true)
                    {
                        Drag.FinalLocation = P;
                        Drag.Change();
                        Scale.Seek.Location.X = Scale.Seek.Location.X + Drag.ChangeWE;
                        Drag.Refresh();
                    }
                    break;
            }
        }

        public void AddTrackEvent(object sender, EventArgs e)
        {
            AddTrack();
            Refresh();
        }

        public void RightClickTrackEvent(object sender, MouseEventArgs e)
        {
            Point RightClick = e.Location;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    for (int i = 0; i < Tracks.Count; i++)
                    {
                        if (Tracks[i].Contains(RightClick))
                        {
                            RightClickMenu.Show(this, RightClick);
                        }
                    }
                    break;
            }
        }

        public void TrackMouseHoverEvent(object sender, EventArgs e)
        {
            // Point P = Cursor.Position;

        }

        public void TrackMouseDownEvent(object sender, MouseEventArgs e)
        {
            Point P = e.Location;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Drag.Seek == false)
                    {
                        for (int i = 0; i < Tracks.Count; i++)
                        {
                            for (int j = 0; j < Tracks[i].TrackObjects.Count; j++)
                            {
                                if (Tracks[i].TrackObjects[j].ContainsBL(P))
                                {
                                    Cursor.Current = Cursors.SizeWE;
                                    Drag.InitialTrack = i;
                                    Drag.InitialTrackObject = j;
                                    Drag.InitialLocation = P;
                                    Drag.SizeW = true;
                                    break;
                                }
                                else if (Tracks[i].TrackObjects[j].ContainsBR(P))
                                {
                                    Cursor.Current = Cursors.SizeWE;
                                    Drag.InitialTrack = i;
                                    Drag.InitialTrackObject = j;
                                    Drag.InitialLocation = P;
                                    Drag.SizeE = true;
                                    break;
                                }
                                else if (Tracks[i].TrackObjects[j].Contains(P))
                                {
                                    Cursor.Current = Cursors.SizeAll;
                                    Drag.InitialTrack = i;
                                    Drag.InitialTrackObject = j;
                                    Drag.InitialLocation = P;
                                    Drag.MoveWE = true;
                                    Drag.MoveNS = true;
                                    break;
                                }

                            }
                            if (Tracks[i].ContainsBT(P))
                            {
                                Cursor.Current = Cursors.SizeNS;
                                Drag.InitialLocation = P;
                                Drag.SizeN = true;
                                Drag.Track = true;
                                break;
                            }
                            else if (Tracks[i].ContainsBD(P))
                            {
                                Cursor.Current = Cursors.SizeNS;
                                Drag.InitialLocation = P;
                                Drag.SizeS = true;
                                Drag.Track = true;
                                break;
                            }
                            else if (Tracks[i].Contains(P))
                            {
                                if (Drag.MoveWE == false && Drag.SizeE == false && Drag.SizeW == false)
                                {
                                    Cursor.Current = Cursors.SizeAll;
                                    Drag.InitialTrack = i;
                                    Drag.MoveNS = true;
                                    Drag.Track = true;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case MouseButtons.Right:
                    break;
            }
        }

        public void TrackMouseMoveEvent(object sender, MouseEventArgs e)
        {
            Point P = e.Location;
            /* for (int i = 0; i < Tracks.Count; i++)
             {
                 for (int j = 0; j < Tracks[i].TrackObjects.Count; j++)
                 {
                     if(Tracks[i].TrackObjects[j].ContainsBL(P) || Tracks[i].TrackObjects[j].ContainsBR(P))
                     {
                         Cursor.Current = Cursors.SizeWE;
                     }
                     else if (Tracks[i].TrackObjects[j].Contains(P))
                     {
                         Cursor.Current = Cursors.SizeAll;
                     }
                     else if (Tracks[i].ContainsBTD(P))
                     {
                         Cursor.Current = Cursors.SizeNS;
                     }
                     else if (Tracks[i].Contains(P))
                     {
                         Cursor.Current = Cursors.SizeAll;
                     }
                 }              
             }
             */
            if (e.Button == MouseButtons.Left)
            {
                if (Drag.SizeE == false && Drag.SizeW == false)
                {

                }
            }
        }

        public void TrackMouseUpEvent(object sender, MouseEventArgs e)
        {
            Point P = e.Location;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Drag.Seek == false)
                    {
                        if (Drag.SizeW == true && Drag.Track == false)
                        {
                            Cursor.Current = Cursors.SizeWE;
                            Drag.FinalLocation = P;
                            Drag.Change();
                            Tracks[Drag.InitialTrack].TrackObjects[Drag.InitialTrackObject].SizeBL(Drag.ChangeWE);
                        }
                        else if (Drag.SizeE == true && Drag.Track == false)
                        {
                            Cursor.Current = Cursors.SizeWE;
                            Drag.FinalLocation = P;
                            Drag.Change();
                            Tracks[Drag.InitialTrack].TrackObjects[Drag.InitialTrackObject].SizeBR(Drag.ChangeWE);
                        }
                        else if (Drag.MoveWE == true && Drag.MoveNS == true && Drag.Track == false)
                        {
                            Cursor.Current = Cursors.SizeAll;
                            Drag.FinalLocation = P;
                            if (Tracks[Drag.InitialTrack].Contains(P))
                            {
                                Drag.MoveNS = false;
                            }
                            else
                            {
                                Drag.MoveWE = false;
                            }
                            if (Drag.MoveWE == true)
                            {
                                Drag.Change();
                                Tracks[Drag.InitialTrack].TrackObjects[Drag.InitialTrackObject].MoveWE(Drag.ChangeWE);
                            }
                            else if (Drag.MoveNS == true && Drag.Track == false)
                            {

                                for (int i = 0; i < Tracks.Count; i++)
                                {
                                    if (Tracks[i].Contains(P))
                                    {
                                        Drag.FinalTrack = i;
                                    }
                                }
                                Drag.TO = Tracks[Drag.InitialTrack].TrackObjects[Drag.InitialTrackObject];
                                Tracks[Drag.InitialTrack].TrackObjects.RemoveAt(Drag.InitialTrackObject);
                                Tracks[Drag.FinalTrack].AddTrackObject(Drag.TO);
                            }
                        }
                        else if (Drag.SizeN == true && Drag.Track == true)
                        {
                            Cursor.Current = Cursors.SizeNS;
                            Drag.FinalLocation = P;
                            Drag.Change();
                            if (Track.Size.Height + Drag.ChangeNS * (-1) > 30 || Track.Size.Height + Drag.ChangeNS * (-1) < 100)
                            {
                                Track.Size.Height = Track.Size.Height + Drag.ChangeNS * (-1);
                            }
                        }
                        else if (Drag.SizeS == true && Drag.Track == true)
                        {
                            Cursor.Current = Cursors.SizeNS;
                            Drag.FinalLocation = P;
                            Drag.Change();
                            if (Track.Size.Height + Drag.ChangeNS > 30 || Track.Size.Height + Drag.ChangeNS < 100)
                            {
                                Track.Size.Height = Track.Size.Height + Drag.ChangeNS;
                            }
                        }
                        Refresh();
                    }
                    Drag.Refresh();
                    break;

                case MouseButtons.Right:
                    break;
            }
        }

        //Entry Point
        public Timeline()
        {
            InitializeComponent();
            DeployTopBar();
            DeployScale();
            DeployTracks();
            DeployBottomBar();
        }

        //Draw or Painter
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_TopBar != null)
            {
                _TopBar.OnPaint(pe);
            }
            if (_Scale != null)
            {
                _Scale.OnPaint(pe);
            }
            if (Tracks != null)
            {
                for (int i = 0; i < Tracks.Count; i++)
                {
                    Tracks[i].OnPaint(pe);
                }
            }
            if (_BottomBar != null)
            {
                _BottomBar.OnPaint(pe);
            }
        }
    }
}
