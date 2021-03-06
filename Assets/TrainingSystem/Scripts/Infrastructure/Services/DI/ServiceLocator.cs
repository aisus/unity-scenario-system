﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = TrainingSystem.Scripts.Infrastructure.Utility.Logger;

namespace TrainingSystem.Scripts.Infrastructure.Services.DI
{
    /// <summary>
    /// Simple global dependency resolver
    /// </summary>
    public class ServiceLocator
    {
        public static  ServiceLocator Current => _instance ?? (_instance = new ServiceLocator());
        private static ServiceLocator _instance;

        private Dictionary<Type, IService> _registeredServices;

        private ServiceLocator()
        {
            _registeredServices = new Dictionary<Type, IService>();
        }

        /// <summary>
        /// Register object as a service of specified type
        /// </summary>
        /// <param name="service">Service instance</param>
        /// <typeparam name="T"></typeparam>
        public void RegisterService<T>(T service) where T : class, IService
        {
            if (_registeredServices.ContainsKey(typeof(T)))
            {
                _registeredServices[typeof(T)] = service;
            }
            else
            {
                _registeredServices.Add(typeof(T), service);
            }
        }

        /// <summary>
        /// Remove registered service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnregisterService<T>() where T : class, IService
        {
            if (_registeredServices.ContainsKey(typeof(T)))
                _registeredServices.Remove(typeof(T));
        }

        /// <summary>
        /// Get service of specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T ResolveDependency<T>()
        {
            if (_registeredServices.TryGetValue(typeof(T), out var service)) return (T) service;
            Logger.Log($"{typeof(T).Name} is not registered", LogType.Error);
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Finalize and remove all scene-specific services
        /// </summary>
        public void FinalizeSceneServices()
        {
            var sceneServices = _registeredServices.Where(x => x.Value is ISceneService).ToList();
            sceneServices.ForEach(x => (x.Value as ISceneService)?.OnSceneExit());
            _registeredServices = _registeredServices.Where(x => !(x.Value is ISceneService))
                                                     .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}