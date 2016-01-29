﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SceneJect
{
	public sealed class SceneJector : MonoBehaviour
	{
		[SerializeField]
		private List<DependencyTypePair> typePairs;

		[SerializeField]
		private List<NonBehaviourDependency> nonBehaviourDependencies;

		private AutoFacContainerWrapper container;

		private void Awake()
		{
			if(!VerifyTypePairs(typePairs))
				throw new NullReferenceException(nameof(SceneJector) + " has a malformed " + nameof(DependencyTypePair) +
						" registered. Must contain a valid MonoBehaviour and selected Type.");

			container = new AutoFacContainerWrapper(typePairs);

			InjectDependencies(container);
		}

		private bool VerifyTypePairs(IEnumerable<DependencyTypePair> pairs)
		{
			foreach (var dtp in typePairs)
			{
				if (dtp.Behaviour == null || dtp.SelectedType == null)
				{
					return false;
				}
			}

			return true;
		}

		private void InjectDependencies<T>(T containerService)
			where T : IResolver, IServiceRegister
		{
			//the IoC container visits each dependency registeration object
			//This allows the registeration logic to be handled differently
			foreach (var nbd in nonBehaviourDependencies)
				nbd.Register(containerService);

			InjecteeLocator<MonoBehaviour> behaviours = new InjecteeLocator<MonoBehaviour>();

			foreach(MonoBehaviour b in behaviours)
			{
				Injector injector = new Injector(b, containerService);

				injector.Inject();
			}
		}
	}
}
