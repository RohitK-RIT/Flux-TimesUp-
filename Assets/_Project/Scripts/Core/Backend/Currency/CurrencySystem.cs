using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Currency
{
    public class CurrencySystem : BaseSystem<CurrencySystem>
    {
        protected override bool IsPersistent => true;

        private const int StartingCoins = 2000;

        private readonly Dictionary<string, Wallet> _wallets = new();

        public string CreateWallet()
        {
            var wallet = new Wallet(StartingCoins);
            var walletID = wallet.GetHashCode().ToString();
            _wallets.Add(walletID, wallet);

            return walletID;
        }

        public void RemoveWallet(string walletID)
        {
            // Remove and destroy the wallet
            _wallets[walletID] = null;
            _wallets.Remove(walletID);
        }

        public void AddCoins(string walletID, int amount)
        {
            var wallet = GetWallet(walletID);
            wallet?.AddCoins(amount);
        }

        public bool RemoveCoins(string walletID, int amount)
        {
            var wallet = GetWallet(walletID);
            return wallet != null && wallet.RemoveCoins(amount);
        }


        private Wallet GetWallet(string walletID)
        {
            var wallet = _wallets.GetValueOrDefault(walletID);
            if (wallet != null)
                return wallet;

            Debug.LogError("Wallet not found!");
            return null;
        }

        public int GetCoins(string walletID)
        {
            var wallet = GetWallet(walletID);
            return wallet?.Coins ?? 0;
        }
    }
}