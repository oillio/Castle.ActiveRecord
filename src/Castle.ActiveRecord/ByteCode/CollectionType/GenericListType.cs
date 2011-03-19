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

namespace Castle.ActiveRecord.ByteCode
{

    using System;
    using System.Collections.Generic;
    using NHibernate.Collection;
    using NHibernate.Engine;
    using NHibernate.Persister.Collection;

    [Serializable]
    class GenericListType<T> : NHibernate.Type.GenericListType<T>
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="GenericListType{T}"/> class for
        /// a specific role.
        /// </summary>
        /// <param name="role">The role the persistent collection is in.</param>
        /// <param name="propertyRef">The name of the property in the
        /// owner object containing the collection ID, or <see langword="null" /> if it is
        /// the primary key.</param>
        public GenericListType(string role, string propertyRef) :
            base(role, propertyRef)
        { }

        /// <summary>
        /// Instantiates a new <see cref="IPersistentCollection"/> for the list.
        /// </summary>
        /// <param name="session">The current <see cref="ISessionImplementor"/> for the list.</param>
        /// <param name="persister">The current <see cref="ICollectionPersister" /> for the list.</param>
        /// <param name="key"></param>
        public override IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key)
        {
            IPersistentCollection ret = base.Instantiate(session, persister, key);
            if (persister.IsLazy) ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }
    }
}
