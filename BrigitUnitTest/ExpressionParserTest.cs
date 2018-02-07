﻿using System;
using System.Collections.Generic;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;
using Brigit.Attributes;
using Brigit.Parser.Stream;
// i'll be using NUnit from now on
using NUnit.Framework;

namespace Brigit.Test
{
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void Parse_Expression_Valid_Expression_Single_Variable()
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
        public void Parse_Expression_Invalid()
        {
            // arrange
            string expression = "var1 var2";

            // act
            bool parsed = BrigitExpressionParser.Preprocess(expression);

            Assert.Throws<Exception>( () => BrigitExpressionParser.Parse(expression));
        }

		[Test]
		public void ParseExpression_ValidExpressionPassPreprocessor()
		{
			// arrange
			string expression = "var1 | var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.BrigitExpressionParser.Preprocess(expression);

			// assert
			Assert.AreEqual(true, parsedWell);	
		}

		[Test]
		public void ParseExpression_InvalidExpressionFailsPreprocessor()
		{
			// arrange
			string expression = "var1 & var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.BrigitExpressionParser.Preprocess(expression);

			// assert
			Assert.AreEqual(false, parsedWell);	
		}

		[Test]
		public void ParseExpression_ValidExpressionShouldEvaluateCorrectly()
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
