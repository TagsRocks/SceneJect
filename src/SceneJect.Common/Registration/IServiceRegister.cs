﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneJect.Common
{
	//TODO: Redesign to be more similar to Autofac and use extensions to pretty up API
	public interface IServiceRegister
	{
		void Register<TTypeToRegister>(TTypeToRegister instance, RegistrationType registerationFlags, Type registerAs = null)
			where TTypeToRegister : class;

		void Register<TTypeToRegister>(RegistrationType registerationFlags, Type registerAs = null)
			where TTypeToRegister : class;

		void Register(DependencyTypePair pair);
	}
}
