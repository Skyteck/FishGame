using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.UI
{
    class StoreMenuItem : UIListItem
    {
        public StoreMenuItem(UIManager uim) : base(uim)
        {

        }

        public void Setup(string lbl, string item)
        {
            UIIcon _CoinImg = new UIIcon(_UIManager);
            _CoinImg.LoadContent("Coin");
            _CoinImg.OffsetPos = new Vector2(5, 5);
            this.AddChild(_CoinImg);

            UILabel _CoinAmt = new UILabel(new Vector2(40,  5), _UIManager, "50");
            _CoinAmt.LoadContent("Fipps");
            this.AddChild(_CoinAmt);



            UILabel itemText = new UILabel(new Vector2( 5,  30), _UIManager, "Rawr");
            itemText.LoadContent("Fipps");
            this.AddChild(itemText);

        }



    }
}
