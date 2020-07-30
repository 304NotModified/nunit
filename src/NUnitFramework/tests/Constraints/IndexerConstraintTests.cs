// ***********************************************************************
// Copyright (c) 2020 Charlie Poole, Rob Prouse
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

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class IndexerConstraintTests
    {
        private static readonly string NL = Environment.NewLine;

        [Test]
        public void CanMatchIndexer()
        {
            var tester = new IndexerTester();
            
            Assert.That(tester, Has.Item(42));
            Assert.That(tester, Has.Item(string.Empty));
            Assert.That(tester, Has.Item(1, 2));
        }
        
        [Test]
        public void CanMatchIndexerEquality()
        {
            var tester = new IndexerTester();
            
            Assert.That(tester, Has.Item(42).EqualTo("Answer to the Ultimate Question of Life, the Universe, and Everything"));
            Assert.That(tester, Has.Item(41).EqualTo("Still calculating").And.Item(42).EqualTo("Answer to the Ultimate Question of Life, the Universe, and Everything"));

            Assert.That(tester, Has.Item(string.Empty).EqualTo("Second indexer"));
            
            Assert.That(tester, Has.Item(1, 2).EqualTo("Third indexer"));
        }

        [Test]
        public void DoesNotMatchMissingIndexer()
        {
            var expectedErrorMessage = $"  Expected: indexer [System.Double]{NL}  But was:  \"not found\"{NL}";
            
            var tester = new IndexerTester();
            
            var ex = Assert.Throws<AssertionException>(() => Assert.That(tester, Has.Item(42d)));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public void DoesNotMatchMissingIndexerEquality()
        {
            var expectedErrorMessage = $"  Expected string length 14 but was 13. Strings differ at index 0.{NL}  Expected: \"Second indexer\"{NL}  But was:  \"Third indexer\"{NL}  -----------^{NL}";
            
            var tester = new IndexerTester();

            var ex = Assert.Throws<AssertionException>(() => Assert.That(tester, Has.Item(4, 2).EqualTo("Second indexer")));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMessage));
        }

        [Test]
        public void CanMatchGenericIndexer()
        {
            var tester = new GenericIndexerTester<int>(100);

            Assert.That(tester, Has.Item(42));
            Assert.That(tester, Has.Item(42).EqualTo(100));
        }

        private class IndexerTester
        {
            public string this[int x] => x == 42 ? "Answer to the Ultimate Question of Life, the Universe, and Everything" : "Still calculating";

            public string this[string x] => "Second indexer";

            public string this[int x, int y] => "Third indexer";
        }

        private class GenericIndexerTester<T>
        {
            private readonly T _item;

            public GenericIndexerTester(T item)
            {
                _item = item;
            }

            public T this[int x] => _item;
        }
    }
}
