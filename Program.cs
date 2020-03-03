using System;
using NUnit.Framework;

namespace CarPricer
{

    class Program
    {
        static void Main(string[] args)
        {
            UnitTests tests = new UnitTests();
            tests.CalculateCarValue();
            
            var priceDeterminator = new PriceDeterminator();
            Car car = new Car()
            {
                PurchaseValue = 20000,
                AgeInMonths = 24,
                NumberOfMiles = 30000,
                NumberOfCollisions = 1,
                NumberOfPreviousOwners = 2
            };

            decimal price = priceDeterminator.DetermineCarPrice(car);
            Console.WriteLine(price);
        }
    }

    public class Car
    {
        public decimal PurchaseValue { get; set; }
        public int AgeInMonths { get; set; }
        public int NumberOfMiles { get; set; }
        public int NumberOfPreviousOwners { get; set; }
        public int NumberOfCollisions { get; set; }
    }

    public class PriceDeterminator
    {
        public decimal DetermineCarPrice(Car car)
        {
            decimal currentValue = 0;

            // Age
            currentValue = car.PurchaseValue - (car.PurchaseValue * (car.AgeInMonths * 0.005m));

            // Miles
            var mileDecrement = car.NumberOfMiles / 1000m;
            mileDecrement = mileDecrement > 150 ? 150 : mileDecrement;
            currentValue = currentValue - (currentValue * (mileDecrement * 0.002m));

            // Previous Owner
            if (car.NumberOfPreviousOwners > 2)
            {
                currentValue = currentValue - (currentValue * 0.25m);
            }

            // Collision
            var collisions = car.NumberOfCollisions > 5 ? 5 : car.NumberOfCollisions;
            currentValue = currentValue - (currentValue * (0.02m * collisions));

            // Previous Owner
            if (car.NumberOfPreviousOwners == 0)
            {
                currentValue = currentValue + (currentValue * 0.1m);
            }

            return currentValue;
        }
    }

    public class UnitTests
    {
        [Test]
        public void CalculateCarValue()
        {
            AssertCarValue(25313.40m, 35000m, 3 * 12, 50000,  1, 1);
            AssertCarValue(19688.20m, 35000m, 3 * 12, 150000, 1, 1);
            AssertCarValue(19688.20m, 35000m, 3 * 12, 250000, 1, 1);
            AssertCarValue(20090.00m, 35000m, 3 * 12, 250000, 1, 0);
            AssertCarValue(21657.02m, 35000m, 3 * 12, 250000, 0, 1);
        }

        [Test]
        private static void AssertCarValue(decimal expectValue, decimal purchaseValue, 
        int ageInMonths, int numberOfMiles, int numberOfPreviousOwners, int numberOfCollisions)
        {
            Car car = new Car
                        {
                            AgeInMonths = ageInMonths,
                            NumberOfCollisions = numberOfCollisions,
                            NumberOfMiles = numberOfMiles,
                            NumberOfPreviousOwners = numberOfPreviousOwners,
                            PurchaseValue = purchaseValue
                        };
            PriceDeterminator priceDeterminator = new PriceDeterminator();
            var carPrice = priceDeterminator.DetermineCarPrice(car);
            Assert.AreEqual(expectValue, carPrice);
        }
    }
}