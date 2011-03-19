// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Castle.ActiveRecord.Framework;

namespace Castle.ActiveRecord.ByteCode 
{
    using DynamicProxy;
    using NHibernate;
    using NHibernate.Intercept;
    using NHibernate.Util;

    public class LazyFieldInterceptor : IFieldInterceptorAccessor, DynamicProxy.IInterceptor 
    {
        public IFieldInterceptor FieldInterceptor 
        {
            get;
            set;
        }

        public void Intercept(IInvocation invocation) 
        {
            if (FieldInterceptor != null) 
            {
                ISession newSession = null;
                try 
                {
                    if (ReflectHelper.IsPropertyGet(invocation.Method)) 
                    {
                        invocation.Proceed(); // get the existing value

                        //If the session has been disconnected, reconnect and reattach before continuing with the initialization.
                        if (FieldInterceptor.Session == null || 
                            !FieldInterceptor.Session.IsOpen || 
                            !FieldInterceptor.Session.IsConnected)
                        {
                            newSession = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(FieldInterceptor.MappedClass);
                            FieldInterceptor.Session = newSession.GetSessionImplementation();

                            string proxyName = FieldInterceptor.MappedClass.UnderlyingSystemType.FullName;
                            newSession.Lock(proxyName, invocation.Proxy, LockMode.None);
                        }
                        var result = FieldInterceptor.Intercept(
                            invocation.InvocationTarget,
                            ReflectHelper.GetPropertyName(invocation.Method),
                            invocation.ReturnValue);

                        if (result != AbstractFieldInterceptor.InvokeImplementation) 
                        {
                            invocation.ReturnValue = result;
                        }
                    }
                    else if (ReflectHelper.IsPropertySet(invocation.Method)) 
                    {
                        //If the session has been disconnected, reconnect and reattach before continuing with the initialization.
                        if (FieldInterceptor.Session == null || 
                            !FieldInterceptor.Session.IsOpen || 
                            !FieldInterceptor.Session.IsConnected)
                        {
                            newSession = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(FieldInterceptor.MappedClass);
                            FieldInterceptor.Session = newSession.GetSessionImplementation();

                            string proxyName = FieldInterceptor.MappedClass.UnderlyingSystemType.FullName;
                            newSession.Lock(proxyName, invocation.Proxy, LockMode.None);
                        }
                        FieldInterceptor.MarkDirty();
                        FieldInterceptor.Intercept(invocation.InvocationTarget, ReflectHelper.GetPropertyName(invocation.Method), null);
                        invocation.Proceed();
                    }
                    else 
                    {
                        invocation.Proceed();
                    }
                } 
                finally 
                {
                    if (newSession != null) ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(newSession);
                }
            }
            else 
            {
                invocation.Proceed();
            }
        }
    }
}
