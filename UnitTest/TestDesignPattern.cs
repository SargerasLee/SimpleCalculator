﻿using System;
using DesignPattern.FactoryMethod;
using DesignPattern.SimpleFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OperationFM = DesignPattern.FactoryMethod.Operation;
using OperationSF = DesignPattern.SimpleFactory.Operation;

namespace UnitTest
{
	[TestClass]
	public class TestDesignPattern
	{
		[TestMethod]
		public void TestSimpleFactory()
		{
			OperationSF op = DesignPattern.SimpleFactory.OperationFactory.CreateInstance("+");
			op.NumA = 2.2;
			op.NumB = 3.3;
			double res = op.GetResult();
			Console.WriteLine(res);
		}

		[TestMethod]
		public void TestFactoryMethod()
		{
			DesignPattern.FactoryMethod.OperationFactory operationFactory = new DesignPattern.FactoryMethod.OperationFactory();
			OperationFM operation = operationFactory.CreateInstance();
			operation.NumA = 5.5;
			operation.NumA = 3.2;
			Console.WriteLine(operation.GetResult());
			OpAddFactory opAddFactory = new OpAddFactory();
			OperationFM opAdd = opAddFactory.CreateInstance();
			opAdd.NumA = 5.5;
			opAdd.NumB = 3.2;
			Console.WriteLine(opAdd.GetResult());
		}
	}
}
