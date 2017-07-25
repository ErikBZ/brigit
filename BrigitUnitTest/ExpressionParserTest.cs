using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brigit.Attributes.ExpressionParser;
using Brigit.Attributes.Operators;
using Brigit.Attributes;

namespace Brigit.Test
{
	[TestClass]
	public class ExpressionParserTest
	{
		[TestMethod]
		public void ParseExpression_ValidExpressionPassPreprocessor()
		{
			// arrange
			string expression = "var1 | var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.Parser.Preprocess(expression);

			// assert
			Assert.AreEqual(true, parsedWell);	
		}

		[TestMethod]
		public void ParseExpression_InvalidExpressionFailsPreprocessor()
		{
			// arrange
			string expression = "var1 & var2 | (var1 & var3)";

			// act
			bool parsedWell = Attributes.ExpressionParser.Parser.Preprocess(expression);

			// assert
			Assert.AreEqual(false, parsedWell);	
		}

		[TestMethod]
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
			if (Attributes.ExpressionParser.Parser.Preprocess(expression))
			{
				exp = Attributes.ExpressionParser.Parser.Parse(expression);
			}

			result = exp.Evaluate(locals, globals);


			// assert
			Assert.AreEqual(expected, result);
		}
	}
}
