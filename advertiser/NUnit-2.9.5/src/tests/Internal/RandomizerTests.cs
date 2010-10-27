﻿// ***********************************************************************
// Copyright (c) 2009 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Reflection;

namespace NUnit.Framework.Internal
{
    public class RandomizerTests
    {
        [Test]
        public void RandomSeedsAreUnique()
        {
            int[] seeds = new int[10];
            for (int i = 0; i < 10; i++)
                seeds[i] = Randomizer.RandomSeed;

            Assert.That(seeds, Is.Unique);
        }

        [Test]
        public void RandomIntsAreUnique()
        {
            Randomizer r = new Randomizer();

            int[] values = new int[10];
            for (int i = 0; i < 10; i++)
                values[i] = r.Next();

            Assert.That(values, Is.Unique);
        }

        [Test]
        public void RandomDoublesAreUnique()
        {
            Randomizer r = new Randomizer();

            double[] values = new double[10];
            for (int i = 0; i < 10; i++)
                values[i] = r.NextDouble();

            Assert.That(values, Is.Unique);
        }

        [Test]
        public void RandomizersWithSameSeedsReturnSameValues()
        {
            Randomizer r1 = new Randomizer(1234);
            Randomizer r2 = new Randomizer(1234);

            for (int i = 0; i < 10; i++)
                Assert.That(r1.NextDouble(), Is.EqualTo(r2.NextDouble()));
        }

        [Test]
        public void RandomizersWithDifferentSeedsReturnDifferentValues()
        {
            Randomizer r1 = new Randomizer(1234);
            Randomizer r2 = new Randomizer(4321);

            for (int i = 0; i < 10; i++)
                Assert.That(r1.NextDouble(), Is.Not.EqualTo(r2.NextDouble()));
        }

        [Test]
        public void ReturnsSameRandomizerForSameParameter()
        {
            ParameterInfo p = testMethod.GetParameters()[0];
            Randomizer r1 = Randomizer.GetRandomizer(p);
            Randomizer r2 = Randomizer.GetRandomizer(p);
            Assert.That(r1, Is.SameAs(r2));
        }

        [Test]
        public void ReturnsSameRandomizerForDifferentParametersOfSameMethod()
        {
            ParameterInfo p1 = testMethod.GetParameters()[0];
            ParameterInfo p2 = testMethod.GetParameters()[1];
            Randomizer r1 = Randomizer.GetRandomizer(p1);
            Randomizer r2 = Randomizer.GetRandomizer(p2);
            Assert.That(r1, Is.SameAs(r2));
        }

        [Test]
        public void ReturnsSameRandomizerForSameMethod()
        {
            Randomizer r1 = Randomizer.GetRandomizer(testMethod);
            Randomizer r2 = Randomizer.GetRandomizer(testMethod);
            Assert.That(r1, Is.SameAs(r2));
        }

        [Test]
        public void ReturnsDifferentRandomizersForDifferentMethods()
        {
            Randomizer r1 = Randomizer.GetRandomizer(testMethod);
            Randomizer r2 = Randomizer.GetRandomizer(MethodInfo.GetCurrentMethod());
            Assert.That(r1, Is.Not.SameAs(r2));
        }

        static readonly MethodInfo testMethod =
            typeof(RandomizerTests).GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
        private void TestMethod(int x, int y)
        {
        }
    }
}
