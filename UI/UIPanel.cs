﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace FishGame.UI
{
    public class UIPanel : UIElement
    {

        List<UIButton> ButtonList;
        List<UILabel> LabelList;

        int minWidth = 50;
        protected int adJustedWidth = 200;
        int minHeight = 60;
        protected int adjustedHeight = 200;
        protected int scrollPos = 0;

        public Vector2 _InitialPos = Vector2.Zero;
        public bool _Showing = true;
        private bool xTracked = false;
        private bool yTracked = false;
        bool leftTracked = false;
        bool topTracked = false;
        bool trackMove = false;
        public bool _Resizable = true;
        

        Texture2D edgeTex;
        Texture2D _Texture;
        protected Texture2D extraTex;

        protected SpriteFont count;
        

        public Vector2 _Center
        {
            get
            {
                return new Vector2(adJustedWidth / 2, adjustedHeight / 2);
            }
        }

        public Rectangle _BoundingBox
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y, adJustedWidth, adjustedHeight);
            }
        }

        protected Rectangle _TopEdge
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y, adJustedWidth, 5);
            }
        }

        private Rectangle _BottomEdge
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y - 5 + adjustedHeight, adJustedWidth, 5);
            }
        }

        protected Rectangle _LeftEdge
        {
            get
            {
                return new Rectangle((int)_Position.X, (int)_Position.Y, 5, adjustedHeight);
            }
        }

        private Rectangle _RightEdge
        {
            get
            {
                return new Rectangle((int)_Position.X + adJustedWidth - 5, (int)_Position.Y, 5, adjustedHeight);
            }
        }

        public UIPanel(UIManager uim) : base(uim)
        {
            ButtonList = new List<UIButton>();
            LabelList = new List<UILabel>();
        }

        public override void LoadContent(string name)
        {
            _Texture = _UIManager.GetTexture(name + "BG");
            edgeTex = _UIManager.GetTexture("edgeTex");
        }

        public void PlaceLabel(Vector2 pos)
        {
            UILabel l = new UILabel(new Vector2(this._Position.X + pos.X, this._Position.Y + pos.Y), _UIManager);
            l.LoadContent("Fipps");
            LabelList.Add(l);
        }

        public void PlaceLabel(Vector2 pos, string text)
        {
            UILabel l = new UILabel(new Vector2(this._Position.X + pos.X, this._Position.Y + pos.Y), _UIManager, text);
            l.LoadContent("Fipps");
            LabelList.Add(l);
            AddChild(l);
        }

        public void PlaceButton(string Name, Vector2 pos, Vector2 size, string label)
        {
            UIButton b = new UIButton(Name, new Vector2(this._Position.X + pos.X, this._Position.Y + pos.Y), size, _UIManager, label);
            b.OffsetPos = pos;
            b.LoadContent("ButtonTex");
            _UIManager.AttachButton(b);
            AddChild(b);
            ButtonList.Add(b);

        }

        public UIButton GetButton(string name)
        {
            return ButtonList.Find(x => x._Name == name);
        }

        public void Update(GameTime gt)
        {
            if (!_Showing) return;


            foreach (UIButton b in ButtonList)
            {
                b.Update(gt);
            }

            if (!_Resizable) return;

            if (xTracked || yTracked || topTracked || leftTracked)
            {
                if (InputHelper.LeftButtonReleased)
                {

                    xTracked = false;
                    yTracked = false;
                    leftTracked = false;
                    topTracked = false;
                }
                else
                {
                    scrollPos = 0;
                    Vector2 currentPos = InputHelper.MouseScreenPos;
                    Vector2 prevPos = InputHelper.PrevMouseScreenPos;

                    int xDiff = (int)(currentPos.X - prevPos.X);
                    int yDiff = (int)(currentPos.Y - prevPos.Y);
                    Vector2 initPos = _InitialPos;
                    if (topTracked)
                    {
                        adjustedHeight -= yDiff;
                        _InitialPos.Y += yDiff;
                    }

                    if (leftTracked)
                    {
                        adJustedWidth -= xDiff;
                        _InitialPos.X += xDiff;
                    }

                    if (xTracked) adJustedWidth += xDiff;
                    if (yTracked) adjustedHeight += yDiff;

                    if (adjustedHeight < minHeight)
                    {
                        adjustedHeight = minHeight;
                        _InitialPos.Y = initPos.Y;
                    }
                    if (adJustedWidth < minWidth)
                    {
                        adJustedWidth = minWidth;
                        _InitialPos.X = initPos.X;
                    }

                }


            }
            else if (trackMove)
            {

                if (InputHelper.RightButtonReleased)
                {
                    trackMove = false;
                }
                else
                {
                    Vector2 currentPos = InputHelper.MouseScreenPos;
                    Vector2 prevPos = InputHelper.PrevMouseScreenPos;

                    int xDiff = (int)(currentPos.X - prevPos.X);
                    int yDiff = (int)(currentPos.Y - prevPos.Y);

                    _InitialPos.X += xDiff;
                    _InitialPos.Y += yDiff;

                }

            }
        }

        public void SetSize(Vector2 size)
        {
            adJustedWidth = (int)size.X;
            adjustedHeight = (int)size.Y;
        }
        public void Resize(Vector2 difference)
        {
            Console.WriteLine(this._Texture.Width);
            adJustedWidth += (int)difference.X;
        }

        public virtual void ProcessClick(Vector2 pos)
        {
            if(this._BoundingBox.Contains(pos))
            {
                foreach(UIButton b in ButtonList)
                {
                    b.ProcessClick(pos);
                }
            }
        }

        public bool CheckForResize(Vector2 pos)
        {
            bool track = false;
            if (_TopEdge.Contains(pos))
            {
                topTracked = true;
                track = true;
            }
            else if (_BottomEdge.Contains(pos))
            {

                yTracked = true;
                track = true;
            }


            if (_LeftEdge.Contains(pos))
            {
                leftTracked = true;
                track = true;
            }
            else if (_RightEdge.Contains(pos))
            {
                xTracked = true;
                track = true;
            }
            return track;
        }

        public bool CheckForMove(Vector2 pos)
        {

            bool track = false;
            if (_TopEdge.Contains(pos) || _BottomEdge.Contains(pos) || _LeftEdge.Contains(pos) || _RightEdge.Contains(pos))
            {
                trackMove = true;
                track = true;
            }
            return track;
        }

        public void HidePanel()
        {
            _Showing = false;
        }

        public void ShowPanel()
        {
            _Showing = true;
        }

        public void ToggleShow()
        {
            _Showing = !_Showing;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_Showing)
            {
                Rectangle sr = new Rectangle((int)_Position.X, (int)_Position.Y, adJustedWidth, adjustedHeight);
                spriteBatch.Draw(_Texture, sr, Color.White);

                if (_Resizable)
                {
                    spriteBatch.Draw(edgeTex, _TopEdge, Color.White);
                    spriteBatch.Draw(edgeTex, _BottomEdge, Color.White);
                    spriteBatch.Draw(edgeTex, _LeftEdge, Color.White);
                    spriteBatch.Draw(edgeTex, _RightEdge, Color.White);
                }

                base.Draw(spriteBatch);

            }
        }

        //internal void MarkToTrack(MouseState mState)
        //{
        //    TrackMouse = true;
        //    prevMousePos = mState;
        //}

        public void MarkToMove()
        {
            trackMove = true;
        }

        public void TestClick()
        {
            Console.WriteLine("Button click!");
        }
    }
}