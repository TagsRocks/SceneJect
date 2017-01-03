﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SceneJect.Common
{
	public class DefaultGameObjectFactory : IGameObjectFactory
	{
		/// <summary>
		/// Service for resolving dependencies.
		/// </summary>
		private IResolver resolverService { get; }

		public DefaultGameObjectFactory(IResolver resolver)
		{
			if (resolver == null)
				throw new ArgumentNullException(nameof(resolver), $"Provided {nameof(IResolver)} service provided is null.");

			resolverService = resolver;
		}

		/// <summary>
		/// Creates an empty <see cref="GameObject"/>.
		/// </summary>
		/// <returns>A non-null empty <see cref="GameObject"/>.</returns>
		public GameObject Create()
		{
			//Default GameObjects don't require dependencies
			return new GameObject();
		}

		/// <summary>
		/// Creates an instance of the prefab <see cref="GameObject"/>.
		/// </summary>
		/// <param name="prefab">Prefab to create an instance of.</param>
		/// <returns>A non-null instance of the provided <see cref="GameObject"/> <paramref name="prefab"/>.</returns>
		public GameObject Create(GameObject prefab)
		{
			//Create the GameObject and inject dependencies
			return InjectDependencies(GameObject.Instantiate(prefab));
		}

		private GameObject InjectDependencies(GameObject obj)
		{
			//Now inject dependencies into its components
			InjecteeLocator<MonoBehaviour> injecteeLocator = new InjecteeLocator<MonoBehaviour>(obj);

			foreach (MonoBehaviour mb in injecteeLocator)
			{
				Injector injecter = new Injector(obj, resolverService);

				injecter.Inject();
			}

			return obj;
		}

		/// <summary>
		/// Creates an instance of the prefab <see cref="GameObject"/> with the provided position and rotation.
		/// </summary>
		/// <param name="prefab">Prefab to create an instance of.</param>
		/// <returns>A non-null instance of the provided <see cref="GameObject"/> <paramref name="prefab"/>.</returns>
		public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			return InjectDependencies(GameObject.Instantiate(prefab, position, rotation) as GameObject);
		}
	}
}
