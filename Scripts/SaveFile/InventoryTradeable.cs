namespace Sands
{
    //this class holds the amount of each tradable in the inventory
    [System.Serializable]
    public class InventoryTradeable
    {
        Tradeable ownedTradeable;
        int count;

        public InventoryTradeable(Tradeable tradeable, int count){
            ownedTradeable = tradeable;
            this.count = count;
        }

        public Tradeable OwnedTradeable{
            get{
                return ownedTradeable;
            }
            set{
                ownedTradeable = value;
            }
        }

        public int Count{
            get{
                return count;
            }
            set{
                count = value;
            }
        }
    }
}