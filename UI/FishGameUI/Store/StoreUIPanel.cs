using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.UI
{
    class StoreUIPanel : UIPanel
    {
        UIListContainer ItemsListContainer;

        public StoreUIPanel(UIManager UIM) : base(UIM)
        {
            _Name = "Store";
        }

        public void Setup()
        {
            List<StoreMenuItem> items = new List<StoreMenuItem>();
            ItemsListContainer = new UIListContainer(_UIManager);
            ItemsListContainer._Name = "StorePanelListContainer";
            ItemsListContainer.LoadContent("Panel");
            ItemsListContainer.OffsetPos = new Vector2(5, 5);
            ItemsListContainer._Size = new Vector2(490, 290);
            ItemsListContainer.buffer = 70;
            this.AddChild(ItemsListContainer);

            for(int i = 0; i < 15; i++)
            {
                StoreMenuItem itemBox = new StoreMenuItem(_UIManager);
                itemBox._Size = new Vector2();
                itemBox.Setup("Blah", "MoreBlah");
                items.Add(itemBox);
            }

            ItemsListContainer.itemsList.AddRange(items);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this._Showing)
            {
                base.Draw(spriteBatch);

            }
        }
    }
}
