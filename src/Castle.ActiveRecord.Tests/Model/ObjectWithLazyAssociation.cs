﻿// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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

using System.Collections.Generic;

namespace Castle.ActiveRecord.Tests.Model {
    [ActiveRecord]
    public class ObjectWithLazyAssociation
    {
        private int id;
        private VeryLazyObject lazyObj;

        [PrimaryKey]
        public int Id 
        {
            get { return id; }
            set { id = value; }
        }

        [BelongsTo(Lazy = FetchWhen.OnInvoke)]
        public VeryLazyObject LazyObj 
        {
            get { return lazyObj; }
            set { lazyObj = value; }
        }

        [HasMany(Table = "numbers")]
        public IList<int> numbers { get; set; }
    }
}