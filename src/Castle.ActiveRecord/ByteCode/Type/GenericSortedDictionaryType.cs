﻿// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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
    using NHibernate.Persister.Collection;
    using NHibernate.Engine;
    using Castle.ActiveRecord.ByteCode.Collection.Generic;

    [Serializable]
    class GenericSortedDictionaryType<TKey, TValue> : NHibernate.Type.GenericSortedDictionaryType<TKey, TValue>
    {
        public GenericSortedDictionaryType(string role, string propertyRef, IComparer<TKey> comparer) :
            base(role, propertyRef,  comparer)
        { }

        public override NHibernate.Collection.IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key)
        {
            return new PersistentGenericMap<TKey, TValue>(session) { IsLazy = persister.IsLazy };
        }
    }
}
