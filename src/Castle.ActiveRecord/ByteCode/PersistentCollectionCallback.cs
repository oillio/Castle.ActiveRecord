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
using NHibernate;
using NHibernate.Collection;
using NHibernate.Engine;

namespace Castle.ActiveRecord.ByteCode
{
    class PersistentCollectionCallback : IPersistentCollectionCallback 
    {
        //Only one instance of this class is required.
        private static readonly PersistentCollectionCallback _instance = new PersistentCollectionCallback();
        public static PersistentCollectionCallback Instance 
        {
            get { return _instance; }
        }

        private class CallbackDisposable : IDisposable 
        {
            private ISession _session;

            public CallbackDisposable(ISession session)
            {
                _session = session;
            }
            public void Dispose()
            {
                if (_session != null) ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(_session);
            }
        }
        
        public IDisposable SessionRequired(IPersistentCollection collection) 
        {
            ISession newSession = null;
            ISessionImplementor Session = collection.GetCurrentSession();

            //If the session has been disconnected, reconnect before continuing with the initialization.
            if (Session == null || 
                !Session.IsOpen ||
                Session.PersistenceContext.ContainsCollection(collection) ||
                !Session.IsConnected)
            {
                newSession = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(collection.Owner.GetType());
                collection.SetCurrentSession(newSession.GetSessionImplementation());
                newSession.Lock(collection.Owner, LockMode.None);
            }

            return new CallbackDisposable(newSession);
        }
    }
}