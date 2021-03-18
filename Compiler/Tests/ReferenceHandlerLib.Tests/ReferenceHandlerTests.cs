using FluentAssertions;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ASTLib.Nodes.ExpressionNodes;

namespace ReferenceHandlerLib.Tests
{
    [TestClass]
    public class ReferenceHandlerTests
    {
        [TestMethod]
        //          FuncName_GivenInput                _CheckingFor
        public void Dispatch_IdentifierExpAndStringList_CorrectListPassedToVisitIdentifier()
        {   
            List<string> expected = new List<string>() { "id" };
            IdentifierExpression input1 = new IdentifierExpression("id", 1, 1);
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);

            List<string> res = new List<string>();
            refHandler.Dispatch(input1, input2);

            helper.VisitIdentifier(Arg.Any<IdentifierExpression>(), Arg.Do<List<string>>(x => res = x));
            res.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Dispatch_IdentifierExpAndStringList_CorrectIdentifierExpPassedToVisitIdentifier()
        {
            IdentifierExpression expected = new IdentifierExpression("id", 1, 1);
            IdentifierExpression input1 = expected;
            List<string> input2 = new List<string>() { "id" };
            IReferenceHelper helper = Substitute.For<IReferenceHelper>();
            ReferenceHandler refHandler = new ReferenceHandler(helper);

            IdentifierExpression res = null;
            refHandler.Dispatch(input1, input2);

            helper.VisitIdentifier(Arg.Do<IdentifierExpression>(x => res = x), Arg.Any<List<string>>());
            res.Should().BeEquivalentTo(expected);
        }
    }
}
