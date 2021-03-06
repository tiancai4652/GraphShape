using System;
using System.Collections.Generic;
using GraphShape.Algorithms.Layout;
using GraphShape.Utils;
using NUnit.Framework;
using QuikGraph;

namespace GraphShape.Tests.Algorithms.Layout
{
    /// <summary>
    /// Tests for <see cref="StandardLayoutAlgorithmFactory{TVertex, TEdge, TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal class StandardLayoutAlgorithmFactoryTests
    {
        #region Test classes

        private class TestLayoutParameters : NotifierObject, ILayoutParameters
        {
            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        private class TestLayoutAlgorithm : LayoutAlgorithmBase<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>
        {
            public TestLayoutAlgorithm()
                : base(new BidirectionalGraph<TestVertex, Edge<TestVertex>>())
            {
            }

            protected override void InternalCompute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        [Test]
        public void StandardFactory()
        {
            var positions = new Dictionary<TestVertex, Point>();
            var sizes = new Dictionary<TestVertex, Size>();
            var borders = new Dictionary<TestVertex, Thickness>();
            var layoutTypes = new Dictionary<TestVertex, CompoundVertexInnerLayoutType>();
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var simpleContext = new LayoutContext<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(
                graph,
                positions,
                sizes,
                LayoutMode.Simple);
            var compoundContext1 = new LayoutContext<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(
                graph,
                positions,
                sizes,
                LayoutMode.Compound);
            var compoundContext2 = new CompoundLayoutContext<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(
                graph,
                positions,
                sizes,
                LayoutMode.Compound,
                borders,
                layoutTypes);
            var nullGraphContext = new LayoutContext<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(
                null,
                positions,
                sizes,
                LayoutMode.Simple);

            var factory = new StandardLayoutAlgorithmFactory<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>();
            CollectionAssert.AreEqual(
                new[]
                {
                    "Circular", "Tree", "FR", "BoundedFR", "KK",
                    "ISOM", "LinLog", "Sugiyama", "CompoundFDP",
                    "Random"
                },
                factory.AlgorithmTypes);


            Assert.IsNull(
                factory.CreateAlgorithm(
                    string.Empty,
                    simpleContext,
                    new CircularLayoutParameters()));

            Assert.IsNull(
                factory.CreateAlgorithm(
                    "NotExist",
                    simpleContext,
                    new CircularLayoutParameters()));

            Assert.IsNull(
                factory.CreateAlgorithm(
                    "Circular",
                    nullGraphContext,
                    new CircularLayoutParameters()));

            Assert.IsInstanceOf<CircularLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "Circular",
                    simpleContext,
                    new CircularLayoutParameters()));

            Assert.IsInstanceOf<SimpleTreeLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "Tree",
                    simpleContext,
                    new SimpleTreeLayoutParameters()));

            Assert.IsInstanceOf<FRLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "FR",
                    simpleContext,
                    new FreeFRLayoutParameters()));

            Assert.IsInstanceOf<FRLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "BoundedFR",
                    simpleContext,
                    new BoundedFRLayoutParameters()));

            Assert.IsInstanceOf<KKLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "KK",
                    simpleContext,
                    new KKLayoutParameters()));

            Assert.IsInstanceOf<ISOMLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "ISOM",
                    simpleContext,
                    new ISOMLayoutParameters()));

            Assert.IsInstanceOf<LinLogLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "LinLog",
                    simpleContext,
                    new LinLogLayoutParameters()));

            Assert.IsInstanceOf<SugiyamaLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "Sugiyama",
                    simpleContext,
                    new SugiyamaLayoutParameters()));

            Assert.IsInstanceOf<CompoundFDPLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "CompoundFDP",
                    simpleContext,
                    new CompoundFDPLayoutParameters()));

            Assert.IsInstanceOf<RandomLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "Random",
                    simpleContext,
                    new RandomLayoutParameters()));

            Assert.IsNull(
                factory.CreateAlgorithm(
                    "CompoundFDP",
                    compoundContext1,
                    new CompoundFDPLayoutParameters()));

            Assert.IsNull(
                factory.CreateAlgorithm(
                    "Circular",
                    compoundContext2,
                    new CompoundFDPLayoutParameters()));

            Assert.IsInstanceOf<CompoundFDPLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>>(
                factory.CreateAlgorithm(
                    "CompoundFDP",
                    compoundContext2,
                    new CompoundFDPLayoutParameters()));


            var testParameters = new TestLayoutParameters();
            var circularParameters = new CircularLayoutParameters();
            ILayoutParameters createdParameters = factory.CreateParameters(string.Empty, circularParameters);
            Assert.IsNull(createdParameters);

            createdParameters = factory.CreateParameters("NotExist", circularParameters);
            Assert.IsNull(createdParameters);

            createdParameters = factory.CreateParameters("Circular", null);
            Assert.IsInstanceOf<CircularLayoutParameters>(createdParameters);
            Assert.AreNotSame(circularParameters, createdParameters);

            createdParameters = factory.CreateParameters("Circular", testParameters);
            Assert.IsInstanceOf<CircularLayoutParameters>(createdParameters);
            Assert.AreNotSame(testParameters, createdParameters);

            createdParameters = factory.CreateParameters("Circular", circularParameters);
            Assert.IsInstanceOf<CircularLayoutParameters>(createdParameters);
            Assert.AreNotSame(circularParameters, createdParameters);

            var treeParameters = new SimpleTreeLayoutParameters();
            createdParameters = factory.CreateParameters("Tree", null);
            Assert.IsInstanceOf<SimpleTreeLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("Tree", testParameters);
            Assert.IsInstanceOf<SimpleTreeLayoutParameters>(createdParameters);
            Assert.AreNotSame(testParameters, createdParameters);

            createdParameters = factory.CreateParameters("Tree", treeParameters);
            Assert.IsInstanceOf<SimpleTreeLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            var frParameters = new FreeFRLayoutParameters();
            createdParameters = factory.CreateParameters("FR", null);
            Assert.IsInstanceOf<FreeFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(frParameters, createdParameters);

            createdParameters = factory.CreateParameters("FR", testParameters);
            Assert.IsInstanceOf<FreeFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("FR", frParameters);
            Assert.IsInstanceOf<FreeFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(frParameters, createdParameters);

            var boundedFrParameters = new BoundedFRLayoutParameters();
            createdParameters = factory.CreateParameters("BoundedFR", null);
            Assert.IsInstanceOf<BoundedFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(boundedFrParameters, createdParameters);

            createdParameters = factory.CreateParameters("BoundedFR", testParameters);
            Assert.IsInstanceOf<BoundedFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("BoundedFR", boundedFrParameters);
            Assert.IsInstanceOf<BoundedFRLayoutParameters>(createdParameters);
            Assert.AreNotSame(boundedFrParameters, createdParameters);

            var kkParameters = new KKLayoutParameters();
            createdParameters = factory.CreateParameters("KK", null);
            Assert.IsInstanceOf<KKLayoutParameters>(createdParameters);
            Assert.AreNotSame(kkParameters, createdParameters);

            createdParameters = factory.CreateParameters("KK", testParameters);
            Assert.IsInstanceOf<KKLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("KK", kkParameters);
            Assert.IsInstanceOf<KKLayoutParameters>(createdParameters);
            Assert.AreNotSame(kkParameters, createdParameters);

            var isomParameters = new ISOMLayoutParameters();
            createdParameters = factory.CreateParameters("ISOM", null);
            Assert.IsInstanceOf<ISOMLayoutParameters>(createdParameters);
            Assert.AreNotSame(isomParameters, createdParameters);

            createdParameters = factory.CreateParameters("ISOM", testParameters);
            Assert.IsInstanceOf<ISOMLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("ISOM", isomParameters);
            Assert.IsInstanceOf<ISOMLayoutParameters>(createdParameters);
            Assert.AreNotSame(isomParameters, createdParameters);

            var linLogParameters = new LinLogLayoutParameters();
            createdParameters = factory.CreateParameters("LinLog", null);
            Assert.IsInstanceOf<LinLogLayoutParameters>(createdParameters);
            Assert.AreNotSame(linLogParameters, createdParameters);

            createdParameters = factory.CreateParameters("LinLog", testParameters);
            Assert.IsInstanceOf<LinLogLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("LinLog", linLogParameters);
            Assert.IsInstanceOf<LinLogLayoutParameters>(createdParameters);
            Assert.AreNotSame(linLogParameters, createdParameters);

            var sugiyamaParameters = new SugiyamaLayoutParameters();
            createdParameters = factory.CreateParameters("Sugiyama", null);
            Assert.IsInstanceOf<SugiyamaLayoutParameters>(createdParameters);
            Assert.AreNotSame(sugiyamaParameters, createdParameters);

            createdParameters = factory.CreateParameters("Sugiyama", testParameters);
            Assert.IsInstanceOf<SugiyamaLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("Sugiyama", sugiyamaParameters);
            Assert.IsInstanceOf<SugiyamaLayoutParameters>(createdParameters);
            Assert.AreNotSame(sugiyamaParameters, createdParameters);

            var compoundFDPParameters = new CompoundFDPLayoutParameters();
            createdParameters = factory.CreateParameters("CompoundFDP", null);
            Assert.IsInstanceOf<CompoundFDPLayoutParameters>(createdParameters);
            Assert.AreNotSame(compoundFDPParameters, createdParameters);

            createdParameters = factory.CreateParameters("CompoundFDP", testParameters);
            Assert.IsInstanceOf<CompoundFDPLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("CompoundFDP", compoundFDPParameters);
            Assert.IsInstanceOf<CompoundFDPLayoutParameters>(createdParameters);
            Assert.AreNotSame(compoundFDPParameters, createdParameters);

            var randomParameters = new RandomLayoutParameters();
            createdParameters = factory.CreateParameters("Random", null);
            Assert.IsInstanceOf<RandomLayoutParameters>(createdParameters);
            Assert.AreNotSame(randomParameters, createdParameters);

            createdParameters = factory.CreateParameters("Random", testParameters);
            Assert.IsInstanceOf<RandomLayoutParameters>(createdParameters);
            Assert.AreNotSame(treeParameters, createdParameters);

            createdParameters = factory.CreateParameters("Random", randomParameters);
            Assert.IsInstanceOf<RandomLayoutParameters>(createdParameters);
            Assert.AreNotSame(randomParameters, createdParameters);


            Assert.IsFalse(factory.IsValidAlgorithm(null));
            Assert.IsFalse(factory.IsValidAlgorithm(string.Empty));
            Assert.IsTrue(factory.IsValidAlgorithm("Circular"));
            Assert.IsFalse(factory.IsValidAlgorithm("circular"));
            Assert.IsTrue(factory.IsValidAlgorithm("Tree"));
            Assert.IsTrue(factory.IsValidAlgorithm("FR"));
            Assert.IsTrue(factory.IsValidAlgorithm("BoundedFR"));
            Assert.IsTrue(factory.IsValidAlgorithm("KK"));
            Assert.IsTrue(factory.IsValidAlgorithm("ISOM"));
            Assert.IsTrue(factory.IsValidAlgorithm("LinLog"));
            Assert.IsTrue(factory.IsValidAlgorithm("Sugiyama"));
            Assert.IsTrue(factory.IsValidAlgorithm("CompoundFDP"));
            Assert.IsTrue(factory.IsValidAlgorithm("Random"));


            var algorithm1 = new TestLayoutAlgorithm();
            Assert.IsEmpty(factory.GetAlgorithmType(algorithm1));

            var algorithm2 = new CircularLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, sizes, circularParameters);
            Assert.AreEqual("Circular", factory.GetAlgorithmType(algorithm2));

            var algorithm3 = new SimpleTreeLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, sizes, treeParameters);
            Assert.AreEqual("Tree", factory.GetAlgorithmType(algorithm3));

            var algorithm4 = new FRLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, frParameters);
            Assert.AreEqual("FR", factory.GetAlgorithmType(algorithm4));

            var algorithm5 = new FRLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, boundedFrParameters);
            Assert.AreEqual("BoundedFR", factory.GetAlgorithmType(algorithm5));

            var algorithm6 = new KKLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, kkParameters);
            Assert.AreEqual("KK", factory.GetAlgorithmType(algorithm6));

            var algorithm7 = new ISOMLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, isomParameters);
            Assert.AreEqual("ISOM", factory.GetAlgorithmType(algorithm7));

            var algorithm8 = new LinLogLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph);
            Assert.AreEqual("LinLog", factory.GetAlgorithmType(algorithm8));

            var algorithm9 = new SugiyamaLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, positions, sizes, sugiyamaParameters);
            Assert.AreEqual("Sugiyama", factory.GetAlgorithmType(algorithm9));

            var algorithm10 = new CompoundFDPLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, sizes, borders, layoutTypes);
            Assert.AreEqual("CompoundFDP", factory.GetAlgorithmType(algorithm10));

            var algorithm11 = new RandomLayoutAlgorithm<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(graph, sizes, null, randomParameters);
            Assert.AreEqual("Random", factory.GetAlgorithmType(algorithm11));


            Assert.IsFalse(factory.NeedEdgeRouting(string.Empty));
            Assert.IsTrue(factory.NeedEdgeRouting("Circular"));
            Assert.IsTrue(factory.NeedEdgeRouting("Tree"));
            Assert.IsTrue(factory.NeedEdgeRouting("FR"));
            Assert.IsTrue(factory.NeedEdgeRouting("BoundedFR"));
            Assert.IsTrue(factory.NeedEdgeRouting("KK"));
            Assert.IsTrue(factory.NeedEdgeRouting("ISOM"));
            Assert.IsTrue(factory.NeedEdgeRouting("LinLog"));
            Assert.IsFalse(factory.NeedEdgeRouting("Sugiyama"));
            Assert.IsTrue(factory.NeedEdgeRouting("CompoundFDP"));
            Assert.IsTrue(factory.NeedEdgeRouting("Random"));


            Assert.IsFalse(factory.NeedOverlapRemoval(string.Empty));
            Assert.IsFalse(factory.NeedOverlapRemoval("Circular"));
            Assert.IsFalse(factory.NeedOverlapRemoval("Tree"));
            Assert.IsTrue(factory.NeedOverlapRemoval("FR"));
            Assert.IsTrue(factory.NeedOverlapRemoval("BoundedFR"));
            Assert.IsTrue(factory.NeedOverlapRemoval("KK"));
            Assert.IsTrue(factory.NeedOverlapRemoval("ISOM"));
            Assert.IsTrue(factory.NeedOverlapRemoval("LinLog"));
            Assert.IsFalse(factory.NeedOverlapRemoval("Sugiyama"));
            Assert.IsFalse(factory.NeedOverlapRemoval("CompoundFDP"));
            Assert.IsTrue(factory.NeedOverlapRemoval("Random"));
        }

        [Test]
        public void StandardFactory_Throws()
        {
            var positions = new Dictionary<TestVertex, Point>();
            var sizes = new Dictionary<TestVertex, Size>();
            var graph = new BidirectionalGraph<TestVertex, Edge<TestVertex>>();
            var context = new LayoutContext<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>(
                graph,
                positions,
                sizes,
                LayoutMode.Simple);

            var factory = new StandardLayoutAlgorithmFactory<TestVertex, Edge<TestVertex>, BidirectionalGraph<TestVertex, Edge<TestVertex>>>();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(null, context, new CircularLayoutParameters()));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(null, null, new CircularLayoutParameters()));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(null, context, null));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(string.Empty, null, new CircularLayoutParameters()));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(string.Empty, null, null));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateAlgorithm(null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => factory.CreateParameters(null, new CircularLayoutParameters()));
            Assert.Throws<ArgumentNullException>(
                () => factory.CreateParameters(null, null));

            Assert.Throws<ArgumentNullException>(() => factory.GetAlgorithmType(null));

            Assert.Throws<ArgumentNullException>(() => factory.NeedEdgeRouting(null));

            Assert.Throws<ArgumentNullException>(() => factory.NeedOverlapRemoval(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}