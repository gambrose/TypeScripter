﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

using NUnit.Framework;

using TypeScripter.TypeScript;
using TypeScripter.Tests;

namespace TypeScripter.Examples
{
	#region Example Constructs
	public class Animal
	{
		public string Age
		{
			get;
			set;
		}

		public void Sleep(int hours)
		{
		}
	}

	public class Mammal : Animal
	{
		public void WarmBlood()
		{
		}
	}

	public class Zoo<T> where T : Animal
	{
		public T[] GetAnimals()
		{
			return default(T[]);
		}
	}

	public class MammalZoo : Zoo<Mammal>
	{
	}

	[DataContract]
	public class ZooKeeper
	{
		[DataMember]
		public string Name
		{
			get;
			set;
		}
	}

	[ServiceContract]
	public class ZooService
	{
		[OperationContract]
		public ZooKeeper GetKeeper()
		{
			return default(ZooKeeper);
		}
	}

	public class PromiseFormatter : TsFormatter
	{
		public override string Format(TsType returnType)
		{
			return string.Format("ng.IPromise<{0}>", base.Format(returnType));
		}
	}
	#endregion

	[TestFixture]
	public class BasicUsage : Test
	{
		[Test]
		public void OutputTest()
		{
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddType(typeof(Animal))
				.ToString();

			ValidateTypeScript(output);
		}
	}

	[TestFixture]
	public class Inheritance : Test
	{
		[Test]
		public void OutputTest()
		{
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddType(typeof(Mammal))
				.ToString();

			ValidateTypeScript(output);
		}
	}

	[TestFixture]
	public class Generics : Test
	{
		[Test]
		public void OutputTest()
		{
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddType(typeof(MammalZoo))
				.ToString();

			ValidateTypeScript(output);
		}
	}

	[TestFixture]
	public class AssemblyOutput : Test
	{
		[Test]
		public void OutputTest()
		{
			var assembly = this.GetType().Assembly;
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.AddTypes(assembly)
				.ToString();

			ValidateTypeScript(output);
		}
	}

	[TestFixture]
	public class TypeReaders : Test
	{
		[Test]
		public void OutputTest()
		{
			var assembly = this.GetType().Assembly;
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.UsingTypeReader(
					new TypeScripter.Readers.CompositeTypeReader(
						new TypeScripter.Readers.DataContractTypeReader(),
                        new TypeScripter.Readers.ServiceContractTypeReader()
					)
				)
				.AddTypes(assembly)
				.ToString();

			ValidateTypeScript(output);
		}
	}

	[TestFixture]
	public class Formatters : Test
	{
		[Test]
		public void OutputTest()
		{
			var assembly = this.GetType().Assembly;
			var scripter = new TypeScripter.Scripter();
			var output = scripter
				.UsingTypeReader(
					new TypeScripter.Readers.CompositeTypeReader(
						new TypeScripter.Readers.DataContractTypeReader(),
						new TypeScripter.Readers.ServiceContractTypeReader()
					)
				)
				.UsingFormatter(new PromiseFormatter())
				.AddTypes(assembly)
				.ToString();

			ValidateTypeScript(output);
		}
	}

	public class Legacy
	{

		[Test]
		public void OutputTest()
		{
			// define the module
			var module = new TsModule(new TsName("Acme"));
			module.Types.Add(new TsEnum(new TsName("StatusCodes"), new Dictionary<string, long?>() {
				{ "Failure", 0 },
				{ "Success", 1 },
			}));

			// define the interface
			var fooInterface = new TsInterface(new TsName("IFoo"));
			fooInterface.Properties.Add(new TsProperty(new TsName("Name"), TsPrimitive.String));


			var tsFunction = new TsFunction(new TsName("ChangeName"));
			tsFunction.Parameters.Add(new TsParameter(new TsName("name"), TsPrimitive.String));
			fooInterface.Functions.Add(tsFunction);

			module.Types.Add(fooInterface);

			var output = new StringBuilder();
			output.Append(
				module.ToString()
			);
		}
	}
}

