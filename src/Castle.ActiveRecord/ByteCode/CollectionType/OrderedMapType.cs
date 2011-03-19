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
    class OrderedMapType : NHibernate.Type.OrderedMapType
    {
		/// <summary>
		/// Initializes a new instance of a <see cref="OrderedMapType"/> class.
		/// </summary>
		/// <param name="role">The role the persistent collection is in.</param>
		/// <param name="propertyRef"></param>
		/// <param name="isEmbeddedInXML"></param>
        public OrderedMapType(string role, string propertyRef, bool isEmbeddedInXML) :
            base(role, propertyRef, isEmbeddedInXML)
        { }

        public override object Instantiate(int anticipatedSize) 
        {
            object ret = base.Instantiate(anticipatedSize);
            IPersistentCollection coll = ret as IPersistentCollection;
            if(coll != null) coll.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        public override IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key)
        {
            IPersistentCollection ret = base.Instantiate(session, persister, key);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
        {
            IPersistentCollection ret = base.Wrap(session, collection);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }
    }
}
