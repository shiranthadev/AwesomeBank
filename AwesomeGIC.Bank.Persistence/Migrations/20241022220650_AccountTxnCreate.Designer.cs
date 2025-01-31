﻿// <auto-generated />
using System;
using AwesomeGIC.Bank.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AwesomeGIC.Bank.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241022220650_AccountTxnCreate")]
    partial class AccountTxnCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("AwesomeGIC.Bank.Domain.Entities.AccountTxn", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("BankAccountId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TxnAmount")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("TxnDate")
                        .HasColumnType("TEXT");

                    b.Property<char>("TxnType")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.ToTable("AccountTxns");
                });

            modelBuilder.Entity("AwesomeGIC.Bank.Domain.Entities.BankAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("AwesomeGIC.Bank.Domain.Entities.AccountTxn", b =>
                {
                    b.HasOne("AwesomeGIC.Bank.Domain.Entities.BankAccount", "BankAccount")
                        .WithMany("BankAccountTransactions")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("AwesomeGIC.Bank.Domain.Entities.BankAccount", b =>
                {
                    b.Navigation("BankAccountTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
