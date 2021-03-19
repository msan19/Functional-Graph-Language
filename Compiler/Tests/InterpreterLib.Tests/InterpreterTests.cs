using ASTLib.Nodes.ExpressionNodes;
using FluentAssertions;
using InterpreterLib.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterpreterLib.Tests
{
    [TestClass]
    public class InterpreterTests
    {
        
        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectListPassed()
        {
            List<Object> expected = new List<Object>() { 23, 2.334, null};
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            List<Object> res = null;
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Do<List<Object>>(x => res = x));

            refHandler.DispatchFunction(input1, expected);

            res.Should().BeEquivalentTo(expected);
        }
        
        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectFunctionCallExprPassed()
        {
            FunctionCallExpression expected = new FunctionCallExpression("", null, 0, 0);
            FunctionCallExpression input1 = expected;
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            FunctionCallExpression res = null;
            fhelper.FunctionCallFunction(Arg.Do<FunctionCallExpression>(x => res = x), Arg.Any<List<Object>>());

            refHandler.DispatchFunction(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void DispatchFunction_FunctionCallAndObjectList_CorrectValueReturned()
        {
            int expected = 17;
            FunctionCallExpression input1 = new FunctionCallExpression("", null, 0, 0);
            List<Object> input2 = new List<Object>() { 23, 2.334, null };
            IFunctionHelper fhelper = Substitute.For<IFunctionHelper>();
            IIntegerHelper  ihelper = Substitute.For<IIntegerHelper>();
            IRealHelper     rhelper = Substitute.For<IRealHelper>();
            Interpreter refHandler = new Interpreter(fhelper, ihelper, rhelper);
            fhelper.FunctionCallFunction(Arg.Any<FunctionCallExpression>(), Arg.Any<List<Object>>()).Returns(expected);

            int res = refHandler.DispatchFunction(input1, input2);

            Assert.AreEqual(expected, res);
        }



        /*
        [TestMethod]
        public void Dispatch_XXXXXXAndStringList_CorrectListPassed()
        {
            List<string> expected = new List<string>() { "id" };
            XXX input1 = new XXX;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            List<string> res = null;
            helper.VisitXXX(Arg.Any<XXX>(), Arg.Do<List<string>>(x => res = x));

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }
        
        [TestMethod]
        public void Dispatch_XXXXXAndStringList_CorrectXXXPassed()
        {
            XXX expected = new XXX;
            XXX input1 = expected;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);
            XXX res = null;
            helper.VisitXXX(Arg.Do<XXX>(x => res = x), Arg.Any<List<string>>());

            refHandler.Dispatch(input1, input2);

            res.Should().BeEquivalentTo(expected);
        }
        */
    }
}
