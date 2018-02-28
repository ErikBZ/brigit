using System;
using System.Collections.Generic;
using Brigit.Attributes;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;
using NUnit.Framework;

namespace Brigit.Test
{
	// TODO much like LinkedListTest I should try writing more tests
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void Parse_SingleVaribaleExpression()
        {
            // arrange
            string expression = "var1";

            //act
            bool parsedWell = BrigitExpressionParser.Preprocess(expression);
            var exp = BrigitExpressionParser.Parse(expression);
            var expected = new Variable("var1");

            // assert
            bool checker = parsedWell && expected.Equals(exp);
            Assert.AreEqual(true, checker);
        }

        [Test]
        public void Parse_TwoVariableExpresssionWithNoOperator_Fails()
        {
            // arrange
            string expression = "var1 var2";

            // act
            bool parsed = BrigitExpressionParser.Preprocess(expression);

            Assert.Throws<Exception>( () => BrigitExpressionParser.Parse(expression));
        }

		[Test]
		public void Preprocess_ValidExpression()
		{
			// arrange
			string expression = "var1 | var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.BrigitExpressionParser.Preprocess(expression);

			// assert
			Assert.AreEqual(true, parsedWell);	
		}

		[Test]
		public void Preprocess_InvalidExpression_ExpectException()
		{
			// arrange
			string expression = "var1 & var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.BrigitExpressionParser.Preprocess(expression);

			// assert
			Assert.AreEqual(false, parsedWell);	
		}

		[Test]
		public void Evalute_TwoVariablesAndedTogether()
		{
			// arrange
			string expression = "var1 & var2";
			Dictionary<String, Flag> locals = new Dictionary<String, Flag>();
			locals.Add("var1", Flag.True);
			locals.Add("var2", Flag.False);
			// nothing will be in globals for tests
			Dictionary<string, Flag> globals = new Dictionary<string, Flag>();
			Flag expected = Flag.False;
			Flag result = Flag.DontCare;

			// act
			IExpression exp = null;	
			if (Attributes.ExpressionParser.BrigitExpressionParser.Preprocess(expression))
			{
				exp = Attributes.ExpressionParser.BrigitExpressionParser.Parse(expression);
			}

			result = exp.Evaluate(locals, globals);

			// assert
			Assert.AreEqual(expected, result);
		}
	}
}
