﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace TIKSN.Finance.Tests
{
	[TestClass]
	public class FixedRateCurrencyConverterTests
	{
		[TestMethod]
		public async Task ConvertCurrency001()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);

			Money Initial = new Money(USDollar, 100);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			Money Final = await converter.ConvertCurrencyAsync(Initial, PoundSterling, System.DateTime.Now);

			Assert.AreEqual(PoundSterling, Final.Currency);
			Assert.AreEqual(200m, Final.Amount);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ConvertCurrency002()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");
			RegionInfo Italy = new RegionInfo("IT");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);
			CurrencyInfo Euro = new CurrencyInfo(Italy);

			Money Initial = new Money(USDollar, 100);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			await converter.ConvertCurrencyAsync(Initial, Euro, DateTimeOffset.Now);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ConvertCurrency003()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");
			RegionInfo Italy = new RegionInfo("IT");
			RegionInfo Armenia = new RegionInfo("AM");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);
			CurrencyInfo Euro = new CurrencyInfo(Italy);
			CurrencyInfo ArmenianDram = new CurrencyInfo(Armenia);

			Money Initial = new Money(ArmenianDram, 100);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			await converter.ConvertCurrencyAsync(Initial, Euro, DateTimeOffset.Now);
		}

		[TestMethod]
		public async Task CurrencyPair001()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");
			RegionInfo Italy = new RegionInfo("IT");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);
			CurrencyInfo Euro = new CurrencyInfo(Italy);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			Assert.IsTrue(ReferenceEquals(converter.CurrencyPair.BaseCurrency, USDollar));
			Assert.IsTrue(object.ReferenceEquals(converter.CurrencyPair.CounterCurrency, PoundSterling));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task FixedRateCurrencyConverter001()
		{
			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(null, 0.5m);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task FixedRateCurrencyConverter002()
		{
			RegionInfo US = new RegionInfo("US");
			RegionInfo AM = new RegionInfo("AM");

			CurrencyInfo USD = new CurrencyInfo(US);
			CurrencyInfo AMD = new CurrencyInfo(AM);

			CurrencyPair pair = new CurrencyPair(USD, AMD);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(pair, -0.5m);
		}

		[TestMethod]
		public async Task GetCurrencyPairs001()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);

			var pair = new CurrencyPair(USDollar, PoundSterling);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(pair, 2m);

			Assert.AreEqual<CurrencyInfo>(USDollar, converter.CurrencyPair.BaseCurrency);
			Assert.AreEqual<CurrencyInfo>(PoundSterling, converter.CurrencyPair.CounterCurrency);

			Assert.IsTrue(object.ReferenceEquals(pair, (await converter.GetCurrencyPairsAsync(DateTimeOffset.Now)).Single()));
		}

		[TestMethod]
		public async Task GetExchangeRate001()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			Assert.AreEqual(2m, await converter.GetExchangeRateAsync(new CurrencyPair(USDollar, PoundSterling), DateTimeOffset.Now));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task GetExchangeRate002()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			await converter.GetExchangeRateAsync(new CurrencyPair(PoundSterling, USDollar), DateTimeOffset.Now);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task GetExchangeRate003()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");
			RegionInfo Italy = new RegionInfo("IT");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);
			CurrencyInfo Euro = new CurrencyInfo(Italy);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			await converter.GetExchangeRateAsync(new CurrencyPair(Euro, USDollar), DateTimeOffset.Now);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task GetExchangeRate004()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");
			RegionInfo Italy = new RegionInfo("IT");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);
			CurrencyInfo Euro = new CurrencyInfo(Italy);

			FixedRateCurrencyConverter converter = new FixedRateCurrencyConverter(new CurrencyPair(USDollar, PoundSterling), 2m);

			await converter.GetExchangeRateAsync(new CurrencyPair(USDollar, Euro), DateTimeOffset.Now);
		}

		[TestMethod]
		public async Task GetExchangeRate005()
		{
			RegionInfo UnitedStates = new RegionInfo("US");
			RegionInfo UnitedKingdom = new RegionInfo("GB");

			CurrencyInfo USDollar = new CurrencyInfo(UnitedStates);
			CurrencyInfo PoundSterling = new CurrencyInfo(UnitedKingdom);

			CurrencyPair pair = new CurrencyPair(PoundSterling, USDollar);

			var converter = new FixedRateCurrencyConverter(pair, 1.6m);

			var LastMonth = DateTimeOffset.Now.AddMonths(-1);
			var NextMonth = DateTimeOffset.Now.AddMonths(1);

			decimal RateInLastMonth = await converter.GetExchangeRateAsync(pair, LastMonth);
			decimal RateInNextMonth = await converter.GetExchangeRateAsync(pair, NextMonth);

			Assert.IsTrue(RateInLastMonth == RateInNextMonth);
		}
	}
}