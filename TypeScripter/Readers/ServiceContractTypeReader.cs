﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using TypeScripter.TypeScript;

namespace TypeScripter.Readers
{
	public class ServiceContractTypeReader : DefaultTypeReader
	{
		#region Methods
		private bool IsServiceContract(Type type)
		{
			return type.GetCustomAttribute<ServiceContractAttribute>(true) != null;
        }

		private bool IsOperationContract(MethodInfo method)
		{
			return method.GetCustomAttribute<OperationContractAttribute>(true) != null;
		}
		#endregion

		#region ITypeReader
		public override IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return base.GetTypes(assembly)
				.Where(IsServiceContract);
		}

		public override IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return new PropertyInfo[0];
		}

		public override IEnumerable<MethodInfo> GetMethods(Type type)
		{
			return base.GetMethods(type)
				.Where(IsOperationContract);
		}

		public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
		{
			if (!IsOperationContract(method))
				return new ParameterInfo[0];
			return base.GetParameters(method);
		}
		#endregion
	}
}
