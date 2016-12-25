using CasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheCasterTest.TestClasses;

namespace TheCasterTest
{
    [TestClass]
    public class TheCasterTest
    {

        [TestMethod]
        public void TestCastClassesWithMorePropertiesOneWay()
        {
            var classA = new ClassA() {DataA = 1234};
            var theCaster = new TheCaster();

            theCaster
                .InitializeMap(typeof(ClassA), typeof(ClassB))
                .AddPropertyName(nameof(ClassA.DataA), nameof(ClassB.DataB))
                .InitializeMap(typeof(ClassB), typeof(ClassA))
                .AddPropertyName(nameof(ClassB.DataB), nameof(ClassA.DataA));


            var target = theCaster.Cast<ClassB, ClassA>(classA);

            Assert.IsNotNull(target);
            Assert.AreEqual(1234, target.DataB);
        }

        [TestMethod]
        public void TestCastClassesWithComplexPropertiesTwoWay()
        {
            var classC = new ClassC()
            {
                ComplexCTypeClassA = new ClassA() {intProp = 1111, DataA = 2222},
                DataC = 3333
            };
            var theCaster = new TheCaster();

            theCaster
                .InitializeMap(typeof(ClassC), typeof(ClassD))
                .AddPropertyName(nameof(ClassC.ComplexCTypeClassA), nameof(ClassD.ComplexDTypeClassA))
                .AddPropertyName(nameof(ClassC.DataC), nameof(ClassD.DataD))
                .InitializeMap(typeof(ClassD), typeof(ClassC))
                .AddPropertyName(nameof(ClassD.ComplexDTypeClassA), nameof(ClassC.ComplexCTypeClassA))
                .AddPropertyName(nameof(ClassD.DataD), nameof(ClassC.DataC));


            var targetTypeD = theCaster.Cast<ClassD, ClassC>(classC);

            Assert.IsNotNull(targetTypeD);
            Assert.AreEqual(3333, targetTypeD.DataD);
            Assert.AreEqual(classC.ComplexCTypeClassA, targetTypeD.ComplexDTypeClassA);

            var targetTypeC = theCaster.Cast<ClassC, ClassD>(targetTypeD);

            Assert.IsNotNull(targetTypeC);
            Assert.AreEqual(3333, targetTypeC.DataC);
            Assert.AreEqual(targetTypeD.ComplexDTypeClassA, targetTypeC.ComplexCTypeClassA);
        }

        [TestMethod]
        public void TestCastClassesWithComplexPropertiesAndComplexChildrenOneWay()
        {
            var classE = new ClassE()
            {
                ComplexETypeClassB = new ClassB() { DataB = 1234, stringProp = "abcd"},
                DataE = 999
            };

            var theCaster = new TheCaster();

            theCaster
                .InitializeMap(typeof(ClassE), typeof(ClassD))
                .AddPropertyName(nameof(ClassE.DataE), nameof(ClassD.DataD))
                .InitializeMap(typeof(ClassB), typeof(ClassA))
                .AddPropertyName(nameof(ClassB.DataB), nameof(ClassA.DataA));


            var targetTypeD = theCaster.Cast<ClassD, ClassE>(classE);
            targetTypeD.ComplexDTypeClassA = theCaster.Cast<ClassA, ClassB>(classE.ComplexETypeClassB);

            Assert.IsNotNull(targetTypeD);
            Assert.AreEqual(999, targetTypeD.DataD);
            Assert.AreEqual(classE.ComplexETypeClassB.DataB, targetTypeD.ComplexDTypeClassA.DataA);

        }

        //Se crea un tipo anonimo que sigue la estrutura de una CLASS A, No he sabido añadir campos a un tipo anonimo de manera dinámica...
        [TestMethod]
        public void TestAnonymousSerializationNotNullOrEmpty()
        {

            var theCaster = new TheCaster();

            var targetJson = theCaster.AnonymousTypeSerialize(1234, 9999);

            Assert.IsNotNull(targetJson);
            Assert.AreNotEqual("", targetJson);
        }

        //SOLO FUNCIONA PARA CLASS A, No he sabido añadir campos a un tipo anonimo de manera dinámica...
        [TestMethod]
        public void TestAnonymousSerializationAndDeserializationToClass()
        {

            var theCaster = new TheCaster();

            var targetJson = theCaster.AnonymousTypeSerialize(1234, 9999);

            Assert.IsNotNull(targetJson);
            Assert.AreNotEqual("", targetJson);

            var targetClassA = theCaster.AnonymousTypeDeserialize<ClassA>(targetJson);
            Assert.IsNotNull(targetClassA);


        }
    }
}