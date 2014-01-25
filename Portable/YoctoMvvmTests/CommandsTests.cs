using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoctoMvvm.Common;
using System.Diagnostics.CodeAnalysis;
using YoctoMvvm.Commands;
using System.Threading.Tasks;
using System.Threading;

namespace YoctoMvvmTests {
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommandsTests {
        [TestMethod]
        [TestCategory("YoctoMvvm - Commands")]
        public void Command_Execute_CommandShouldFireExecutingAndExecuted() {
            var fired = 0;
            CancelYoctoCommandEventHandler<object> executingHandler = (sender, ev) => {
                fired++;
            };
            CommandEventHandler<object> executedHandler = (sender, ev) => {
                fired++;
            };
            var command = new YoctoCommand((a) => { }, () => { return true; });
            command.Executing += executingHandler;
            command.Executed += executedHandler;
            command.Execute(null);
            EventHandler eventHandler = (sender, e) => {
                fired++;
            };
            command.CanExecuteChanged += eventHandler;
            command.RaiseCanExecuteChanged();
            Assert.AreEqual(3, fired);
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Commands")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Command_Execute_ShouldThrowWhenParameterNotCorrect() {
            var command = new YoctoCommand<int>((a) => { }, () => { return true; });
            command.Execute("test");
        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Commands")]
        public void Command_Execute_ShouldNotHappenWhenCommandDisabled() {
            var testValue = 0;
            var reset = new ManualResetEvent(false);
            var command = new AsyncYoctoCommand(
                async () => {
                    await Task.Delay(1);
                    testValue = 1;
                    reset.Set();
                },
                (a) => true);
            command.Disable(null);
            command.Execute(null);
            reset.WaitOne(15);
            Assert.AreEqual(0, testValue);

        }
        [TestMethod]
        [TestCategory("YoctoMvvm - Commands")]
        public void Command_Execute_ShouldHappenWhenCommandEnabled() {
            var testValue = 0;
            var reset = new ManualResetEvent(false);
            var command = new AsyncYoctoCommand(
                async () => {
                    await Task.Delay(1);
                    testValue = 1;
                    reset.Set();
                },
                (a) => false);
            command.Enable(null);
            command.Execute(null);
            reset.WaitOne(15);
            Assert.AreEqual(1, testValue);

        }
    }
}
