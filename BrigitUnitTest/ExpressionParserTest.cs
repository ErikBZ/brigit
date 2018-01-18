using System;
using System.Collections.Generic;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;
using Brigit.Attributes;
// i'll be using NUnit from now on
using NUnit.Framework;

namespace Brigit.Test
{
	[TestFixture]
	public class ExpressionParserTest
	{
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
