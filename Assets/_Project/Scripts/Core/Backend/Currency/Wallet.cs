namespace _Project.Scripts.Core.Backend.Currency
{
    internal class Wallet
    {
        public int Coins { get; private set; }

        public Wallet(int coins)
        {
            Coins = coins;
        }
        
        internal void AddCoins(int amount)
        {
            Coins += amount;
        }
        
        public bool RemoveCoins(int amount)
        {
            if (Coins < amount) return false;
            
            Coins -= amount;
            return true;
        }
    }
}