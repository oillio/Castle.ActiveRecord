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
    class IdentifierBagType : NHibernate.Type.IdentifierBagType
    {
        public IdentifierBagType(string role, string propertyRef, bool isEmbeddedInXML) :
            base(role, propertyRef, isEmbeddedInXML)
        { }

        public override object Instantiate(int anticipatedSize) 
        {
            object ret = base.Instantiate(anticipatedSize);
            IPersistentCollection coll = ret as IPersistentCollection;
            if(coll != null) coll.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        /// <summary>
        /// Instantiates a new <see cref="IPersistentCollection"/> for the identifier bag.
        /// </summary>
        /// <param name="session">The current <see cref="ISessionImplementor"/> for the identifier bag.</param>
        /// <param name="persister"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key)
        {
            IPersistentCollection ret = base.Instantiate(session, persister, key);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        /// <summary>
        /// Wraps an <see cref="IList"/> in a <see cref="PersistentIdentifierBag"/>.
        /// </summary>
        /// <param name="session">The <see cref="ISessionImplementor"/> for the collection to be a part of.</param>
        /// <param name="collection">The unwrapped <see cref="IList"/>.</param>
        /// <returns>
        /// An <see cref="PersistentIdentifierBag"/> that wraps the non NHibernate <see cref="IList"/>.
        /// </returns>
        public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
        {
            IPersistentCollection ret = base.Wrap(session, collection);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }
    }
}
