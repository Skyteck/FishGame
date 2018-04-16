using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.UI
{
    class UIIcon : UIElement
    {
        public Texture2D _Texture;

        public UIIcon(UIManager uim) : base(uim)
        {

        }

        public override void LoadContent(string texName)
        {
            _Texture = _UIManager.GetTexture(texName);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_Texture, this._Position, Color.White);
        }
    }
}
