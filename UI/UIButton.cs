using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.UI
{
    public class UIButton : UIElement
    {
        public delegate void ButtonClickedDelegate(UIButton b);
        public event ButtonClickedDelegate ButtonPressed;

        internal Texture2D _BG;
        internal Texture2D _BGHover;
        internal Texture2D _BGClicked;
        internal Texture2D _ActiveTex;
        internal Texture2D iconTex;
        bool Clicked = false;
        bool Hovered = false;
        
        //UILabel _Label;

        public UIButton(string name, Vector2 pos, Vector2 size, UIManager uIManager, string l): base(uIManager)
        {
            this._Name = name;
            SetPosition(pos);
            _Size = size;
            this.AddLabel(l);
        }

        public UIButton(string name, Vector2 pos, Vector2 size, UIManager uIManager) : base(uIManager)
        {
            this._Name = name;
            SetPosition(pos);
            _Size = size;            
        }

        Rectangle _BoundingBox
        {
            get
            {
                return new Rectangle((int)this._Position.X, (int)this._Position.Y, (int)this._Size.X, (int)this._Size.Y);
            }
        }

        public override void LoadContent(string texName)
        {
            _BG = _UIManager.GetTexture(texName);
            _BGClicked = _UIManager.GetTexture(texName + "Clicked");
            _BGHover = _UIManager.GetTexture(texName + "Hover");            

            _ActiveTex = _BG;
        }

        internal void Update(GameTime gt)
        {
            if(Clicked)
            {
                //button was clicked on, now check if mouse click released also on button
                if(this._BoundingBox.Contains(InputHelper.MouseScreenPos))
                {
                    if (InputHelper.LeftButtonReleased)
                    {
                        OnButtonPressed();
                        _ActiveTex = _BG;
                        //Hovered = false;
                        Clicked = false;
                    }
                }
                //button was clicked on, but now the mouse is not on the button
                else if(!this._BoundingBox.Contains(InputHelper.MouseScreenPos))
                {
                    //mouse released off of button. do nothing
                    if(InputHelper.LeftButtonReleased)
                    {
                        Clicked = false;
                        _ActiveTex = _BG;
                        //Hovered = false;
                    }
                }
            }
            else if(this._BoundingBox.Contains(InputHelper.MouseScreenPos))
            {
                //_ActiveTex = _BGHover;
                Hovered = true;
            }
            else
            {
                _ActiveTex = _BG;
                Hovered = false;
            }


        }

        private void OnButtonPressed()
        {
            ButtonPressed?.Invoke(this);
        }

        internal void ProcessClick(Vector2 pos)
        {
            //button clicked, so mark as clicked and check in update if the click is released and stil in hitbox
            if(this._BoundingBox.Contains(pos))
            {
                _ActiveTex = _BGClicked;
                Clicked = true;

            }
        }

        public void SetIcon(string iconName)
        {
            iconTex = _UIManager.GetTexture(iconName);
        }

        public void AddLabel(string l)
        {
            UI.UILabel nL = new UILabel(new Vector2(this._Size.X / 2, this._Size.Y / 2), this._UIManager, l);
            nL.LoadContent("Fipps");
            AddChild(nL);
            //_Label = nL;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_ActiveTex, this._BoundingBox, Color.White);
            Vector2 textPos = this._BoundingBox.Center.ToVector2();

            //if(iconTex == null)
            //{
            //    if(_Label != null)
            //    {
            //        _Label.Draw(spriteBatch);
            //    }
            //}
            //else
            //{
            //    spriteBatch.Draw(iconTex, new Vector2(textPos.X - (iconTex.Width / 2), textPos.Y - (iconTex.Height / 2)), Color.White);
            //}

            
            if (Hovered)
            {
                if (Clicked) return;
                DrawRectangleOutline(spriteBatch, this._BoundingBox, _BGHover, Color.White, 1);
            }
            base.Draw(spriteBatch);

        }

        private void DrawRectangleOutline(SpriteBatch sb, Rectangle rect, Texture2D tex, Color col, int border = 3)
        {
            //Left Line
            sb.Draw(tex, new Rectangle(rect.X, rect.Y, border, rect.Height), col);

            //top line
            sb.Draw(tex, new Rectangle(rect.X, rect.Y, rect.Width + border, border), col);

            //right line
            sb.Draw(tex, new Rectangle(rect.X + rect.Width, rect.Y, border, rect.Height + border), col);

            //bottom line
            sb.Draw(tex, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width + border, border), col);
        }
    }
}
