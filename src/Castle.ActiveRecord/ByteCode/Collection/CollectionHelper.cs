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

namespace Castle.ActiveRecord.ByteCode.Collection
{

    using NHibernate;
    using NHibernate.Collection;
    using NHibernate.Engine;

    /// <summary>
    /// Provides session management methods for collections.
    /// </summary>
    internal static class CollectionHelper
    {
        /// <summary>
        /// Checks if the collection is still associated with it's session and is not initialized.
        /// Creates a new one if it is not.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="initialized">Is the collection initialized yet.</param>
        /// <param name="Session">The session to which the collection is linked.</param>
        /// <param name="sessionConfirmed">Used to remember if this method has already created a new session for this collection.</param>
        /// <returns>The newly created session, if one is required.</returns>
        internal static ISession checkSession(this IPersistentCollection collection, bool initialized, ISessionImplementor Session, ref bool sessionConfirmed) {
            if (sessionConfirmed) return null;

            if (!initialized)
            {
                //If the session has been disconnected, reconnect before continuing with the initialization.
                if (Session == null ||
                    !Session.IsOpen ||
                    !Session.PersistenceContext.ContainsCollection(collection) ||
                    !Session.IsConnected)
                {
                    var ret = ActiveRecordMediator.GetSessionFactoryHolder().CreateSession(collection.Owner.GetType());
                    sessionConfirmed = true;
                    collection.SetCurrentSession(ret.GetSessionImplementation());
                    ret.Lock(collection.Owner, LockMode.None);
                    return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// Closes the passed in session.
        /// Intended to be used in conjunction with checkSession.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="session">The session to close</param>
        /// <param name="sessionConfirmed">Used to remember if a session created by checkSession is still open.</param>
        internal static void cleanupSession(this IPersistentCollection collection, ISession session, ref bool sessionConfirmed)
        {
            if (session != null)
            {
                sessionConfirmed = false;
                ActiveRecordMediator.GetSessionFactoryHolder().ReleaseSession(session);
            }
        }
    }
}
