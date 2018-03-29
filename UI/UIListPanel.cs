using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.UI
{
    class UIListPanel : UIPanel
    {
        List<UIListPanel> ItemList;

        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].Draw(spriteBatch);
            }
        }
    }
}
