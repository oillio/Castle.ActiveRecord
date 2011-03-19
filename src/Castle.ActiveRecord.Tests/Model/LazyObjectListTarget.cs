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

namespace Castle.ActiveRecord.Tests.Model
{
	[ActiveRecord(Lazy = true)]
	public class LazyObjectListTarget : ScopelessLazy
	{
		private int id;
		private string title;
        private ObjectWithLazyList owner;

		[PrimaryKey]
		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		[Property]
		public virtual string Title
		{
			get { return title; }
			set { title = value; }
		}

	    [BelongsTo(Lazy = FetchWhen.OnInvoke)]
	    public virtual ObjectWithLazyList Owner
        {
            get { return owner; }
            set { owner = value; }
	    }
	}
}