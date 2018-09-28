using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Brigit.Test
{
	[SetUpFixture]
	public class Config
	{
		public static string TomePath = "";

		[OneTimeSetUp]
		public void SetUp()
		{
            //			TomePath = Environment.GetEnvironmentVariable("brigit_test");
            TomePath = @"C:\Users\Brandon\Documents\Projects\brigit\Brigit.Tests\Tests";

        }
	}
}
