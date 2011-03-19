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

    class ArrayType : NHibernate.Type.ArrayType
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="ArrayType"/> class for
        /// a specific role.
        /// </summary>
        /// <param name="role">The role the persistent collection is in.</param>
        /// <param name="propertyRef">The name of the property in the
        /// owner object containing the collection ID, or <see langword="null" /> if it is
        /// the primary key.</param>
        /// <param name="elementClass">The <see cref="System.Type"/> of the element contained in the array.</param>
        /// <param name="isEmbeddedInXML"></param>
        /// <remarks>
        /// This creates a bag that is non-generic.
        /// </remarks>
        public ArrayType(string role, string propertyRef, Type elementClass, bool isEmbeddedInXML) :
            base(role, propertyRef, elementClass, isEmbeddedInXML)
        { }

        public override object Instantiate(int anticipatedSize) 
        {
            IPersistentCollection ret = (IPersistentCollection)base.Instantiate(anticipatedSize);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        public override IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key) {
            IPersistentCollection ret = base.Instantiate(session, persister, key);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }

        /// <summary>
        /// Wraps a <see cref="System.Array"/> in a <see cref="PersistentArrayHolder"/>.
        /// </summary>
        /// <param name="session">The <see cref="ISessionImplementor"/> for the collection to be a part of.</param>
        /// <param name="array">The unwrapped array.</param>
        /// <returns>
        /// An <see cref="PersistentArrayHolder"/> that wraps the non NHibernate <see cref="System.Array"/>.
        /// </returns>
        public override IPersistentCollection Wrap(ISessionImplementor session, object array)
        {
            IPersistentCollection ret = base.Wrap(session, array);
            ret.SetCallback(PersistentCollectionCallback.Instance);
            return ret;
        }
    }
}
