using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame.UI
{
    class MainMenuPanel : UIPanel
    {
        public MainMenuPanel(UIManager uim) : base(uim)
        {
            this._Name = "MainMenu";
        }

        public override void Setup()
        {

            PlaceButton("Store", new Vector2(5, 5), new Vector2(40, 32), "Store");
            PlaceButton("Items", new Vector2(5, 45), new Vector2(40, 32), "Items");
            PlaceButton("Sale", new Vector2(5, 85), new Vector2(40, 32), "Sale");
            PlaceButton("???", new Vector2(5, 125), new Vector2(40, 32), "???");
            PlaceButton("Stats", new Vector2(5, 165), new Vector2(40, 32), "Stats");
            _Resizable = false;
            base.Setup();
        }
    }
}
