﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    public class Account
    {

        public const int CHECKING = 0;
        public const int SAVINGS = 1;
        public const int MAXI_SAVINGS = 2;

        private readonly int accountType;
        public List<Transaction> transactions;

        public Account(int accountType) 
        {
            this.accountType = accountType;
            this.transactions = new List<Transaction>();
        }

        public void Deposit(double amount) 
        {
            if (amount <= 0) {
                throw new ArgumentException("amount must be greater than zero");
            } else {
                transactions.Add(new Transaction(amount));
            }
        }

        public void Withdraw(double amount) 
        {
            if (amount <= 0) {
                throw new ArgumentException("amount must be greater than zero");
            } else {
                transactions.Add(new Transaction(-amount));
            }
        }

        public double InterestEarned() 
        {
            double amount = sumTransactions();
            switch(accountType){
                case SAVINGS:
                    if (amount <= 1000)
                        return amount * 0.001;
                    else
                        return 1 + (amount-1000) * 0.002;
    //            case SUPER_SAVINGS:
    //                if (amount <= 4000)
    //                    return 20;
                case MAXI_SAVINGS:
                    if (CheckIfWithdrawalInLastDays(10))
                        return amount * 0.001;
                    return amount * 0.05;
                default:
                    return amount * 0.001;
            }
        }

        public double sumTransactions() {
           return CheckIfTransactionsExist(true);
        }

        private double CheckIfTransactionsExist(bool checkAll) 
        {
            double amount = 0.0;
            foreach (Transaction t in transactions)
                amount += t.amount;
            return amount;
        }

        private bool CheckIfWithdrawalInLastDays(int days)
        {
            DateTime testDate = DateProvider.getInstance().DaysBeforeNow(days);
            foreach (Transaction t in transactions)
                if (t.WithdrawalAfter(testDate))
                    return true;
            return false;
        }

        public int GetAccountType() 
        {
            return accountType;
        }

        public void TransferTo(Account targetAccount, double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("amount must be greater than zero");
            }
            else if (amount > sumTransactions())
            {
                throw new ArgumentException("amount cannot be greater than the balance in the account");
            }
            else
            {
                transactions.Add(new Transaction(-amount));
                targetAccount.Deposit(amount);
            }
        }

    }
}
