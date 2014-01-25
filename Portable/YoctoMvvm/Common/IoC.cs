
using System;
using System.Collections.Generic;
using System.Reflection;
namespace YoctoMvvm.Common {
    /// <summary>
    /// Cloned from https://gist.github.com/vcsjones/716137 and changed
    /// </summary>
    /// <author>
    /// Kevin Jones (original version)
    /// </author>
    /// <author>
    /// Erez A. Korn (changes)
    /// </author>
    public abstract class YoctoIoc {
        #region private members
        private static readonly Dictionary<Type, Type> _registration = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, object> _rot = new Dictionary<Type, object>();
        private static readonly object[] _emptyArguments = new object[0];
        private static readonly object _syncLock = new object();
        #endregion private members

        #region abstract methods
        /// <summary>
        /// Registeres all types. This must be overidden by the app itself, and provides a convenient place to perform all registrations.
        /// 
        /// If registration needs to happen in multiple locations, the Register functions can still be called directly by the app.
        /// </summary>
        protected abstract void RegisterAll();
        /// <summary>
        /// Checks whether the IoC container is currently running in design mode.
        /// 
        /// This should be overridden by a platform specific implementation of the YoctoMvvm framework.
        /// It can be used by the registrar to register mock implementations.
        /// </summary>
        public abstract bool IsInDesignMode {get;}
        #endregion abstract methods

        #region constructors
        /// <summary>
        /// Creates a new instance of the IoC Container and calls RegisterAll
        /// </summary>
        public YoctoIoc() {
            RegisterAll();
        }
        #endregion constructors

        #region public methods
        /// <summary>
        /// Clears IoC Registrations and Resolved instances.
        /// </summary>
        public void ResetAll() {
            _registration.Clear();
            _rot.Clear();
        }
        /// <summary>
        /// Resolves a previsouly registered Type or returns the previsouly resolved instance
        /// </summary>
        /// <param name="type">The registered type to resolve</param>
        /// <returns>An instance of the registered type</returns>
        protected object Resolve(Type type) {
            lock (_syncLock) {
                if (!_rot.ContainsKey(type)) {
                    if (!_registration.ContainsKey(type)) {
                        throw new Exception("Type not registered.");
                    }
                    Type resolveTo = _registration[type] ?? type;
                    var constructors = new List<ConstructorInfo>(resolveTo.GetTypeInfo().DeclaredConstructors);
                    // Remove type initializers (static constructor)
                    foreach (var constructorCandidate in constructors.ToArray()) {
                        if (!constructorCandidate.IsConstructor) {
                            constructors.Remove(constructorCandidate);
                        }
                    }
                    var constructor = constructors[0];
                    if (constructors.Count > 1) { //TODO Create consrtuctor attribute
                        var foundIocConstructor = false;
                        foreach (var constructorCandidate in constructors) {
                            if (foundIocConstructor) {
                                break;
                            }
                            var attributes = constructorCandidate.GetCustomAttributes();
                            foreach (var attribute in attributes) {
                                if (attribute.GetType().Equals(typeof(IocConstructorAttribute))) {
                                    constructor = constructorCandidate;
                                    foundIocConstructor = true;
                                    break;
                                }
                            }
                        }
                        if (!foundIocConstructor) {
                            throw new Exception("Cannot resolve a type that has more than one constructor.");
                        }
                    }
                    var parameterInfos = constructor.GetParameters();
                    if (parameterInfos.Length == 0) {
                        _rot[type] = constructor.Invoke(_emptyArguments);
                    } else {
                        var parameters = new object[parameterInfos.Length];
                        foreach (var parameterInfo in parameterInfos) {
                            parameters[parameterInfo.Position] = Resolve(parameterInfo.ParameterType);
                        }
                        _rot[type] = constructor.Invoke(parameters);
                    }
                }
                return _rot[type];
            }
        }
        /// <summary>
        /// Resolves a previsouly registered Type or returns the previsouly resolved instance
        /// </summary>
        /// <typeparam name="TRegisteredType">The registered type to resolve</typeparam>
        /// <returns>An instance of the registered type</returns>
        public TRegisteredType Resolve<TRegisteredType>() {
            return (TRegisteredType)Resolve(typeof(TRegisteredType));
        }
        /// <summary>
        /// Registers an implementation for an interface or base class.
        /// </summary>
        /// <typeparam name="TInterface">A type for an interface or base class</typeparam>
        /// <typeparam name="TClass">A concrete implementation of the type</typeparam>
        public void Register<TInterface, TClass>() where TClass : class, TInterface {
            lock (_syncLock) {
                _registration.Add(typeof(TInterface), typeof(TClass));
            }
        }
        /// <summary>
        /// Registers a specific implementation
        /// </summary>
        /// <typeparam name="TClass">A concrete implementation type</typeparam>
        public void Register<TClass>() where TClass : class {
            lock (_syncLock) {
                _registration.Add(typeof(TClass), null);
            }
        }
        #endregion public methods
    }

    /// <summary>
    /// Attribute used to denote an IoC construcor.
    /// 
    /// This allows a class to have multiple constructors, while still marking one of them for the IoC
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class IocConstructorAttribute : Attribute {

    }
}