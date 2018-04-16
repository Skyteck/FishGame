using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.UI
{
    public class UILabel : UIElement
    {
        public string _Label;
        private SpriteFont font;

        public UILabel(Vector2 pos, UIManager uim) : base(uim)
        {
            this.OffsetPos = pos;
        }

        public UILabel(Vector2 pos, UIManager uim, string text) : base(uim)
        {
            this.OffsetPos = pos;
            _Label = text;
        }
        

        public override void LoadContent(string fontName)
        {
            font = _UIManager.GetFont(fontName);

            if(_Label != null)
            {
                _Size = font.MeasureString(_Label);
                OffsetPos.X -= (int)(_Size.X / 2);
                OffsetPos.Y -= (int)(_Size.Y / 2);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(font, _Label, _Position, Color.White);
        }
    }
}
