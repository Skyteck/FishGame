using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.UI
{
    class UIListItem : UIElement
    {
        Texture2D _BG;

        public UIListItem(UIManager uim) : base(uim)
        {

        }

        public override void LoadContent(string texName)
        {
            _BG = _UIManager.GetTexture(texName);
            base.LoadContent(texName);
        }

        public override void Draw(SpriteBatch sb)
        {
            if(_BG != null)
            {
                sb.Draw(_BG, this._Position, Color.White);
            }
            base.Draw(sb);
        }
    }
}
