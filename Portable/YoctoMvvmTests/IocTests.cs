using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoctoMvvm.Common;
using System.Diagnostics.CodeAnalysis;

namespace YoctoMvvmTests {
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class IocTests {
        #region Classes for tests
    public interface IForRegister {
    }
    private class IoCForTests : YoctoIoc {
        private bool _IsInDesignMode;
        public void SetDesignMode(bool value) {
            _IsInDesignMode = value;
        }
        protected override void RegisterAll() {
        }
        public override bool IsInDesignMode {
            get {
                return _IsInDesignMode;
            }
        }
    }
    public class ForRegister : IForRegister {

    }
    public class MultiConstructors {
        static MultiConstructors() {

        }
        public MultiConstructors() {

        }
        [Dummy]
        public MultiConstructors(IForRegister a) {

        }
    }
    private class DummyAttribute : Attribute { }

    public class MultiConstructorsWithIoCAttribue {
        [IocConstructor]
        public MultiConstructorsWithIoCAttribue(IForRegister a) {

        }
        public MultiConstructorsWithIoCAttribue() {

        }
    }
    private class WithParams {
        public WithParams(IForRegister b) {

        }
    }

    #endregion Classes for tests

        private IoCForTests _IoC = new IoCForTests();
        [TestCleanup]
        public void TestCleanup() {
            _IoC.ResetAll();
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        [ExpectedException(typeof(Exception))]
        public void IoC_Register_ResolveShouldThrowWhenNotRegistered() {
            _IoC.Resolve<YoctoIoc>();
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        [ExpectedException(typeof(ArgumentException))]
        public void IoC_Register_RegisterShouldThrowWhenAlreadyRegistered() {
            _IoC.Register<IForRegister, ForRegister>();
            _IoC.Register<IForRegister, ForRegister>();
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        public void IoC_Register_ResolveShouldSucceedWhenRegistered() {
            _IoC.Register<IForRegister, ForRegister>();
            Assert.IsNotNull(_IoC.Resolve<IForRegister>());
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        public void IoC_Register_ResolveShouldSucceedWhenRegisteredConcrete() {
            _IoC.Register<IForRegister, ForRegister>();
            _IoC.Register<WithParams>();
            Assert.IsNotNull(_IoC.Resolve<WithParams>());
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        [ExpectedException(typeof(Exception))]
        public void IoC_Register_ResolveShouldThrowWhenMultipleConstructors() {
            _IoC.Register<IForRegister, ForRegister>();
            _IoC.Register<MultiConstructors>();
            _IoC.Resolve<MultiConstructors>();
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - IOC")]
        public void IoC_Register_ResolveShouldSucceedWhenMultipleConstructorsWithOneAttribute() {
            _IoC.Register<IForRegister, ForRegister>();
            _IoC.Register<MultiConstructorsWithIoCAttribue>();
            var resolved = _IoC.Resolve<MultiConstructorsWithIoCAttribue>();
            Assert.IsNotNull(resolved);
        }
    }
}
