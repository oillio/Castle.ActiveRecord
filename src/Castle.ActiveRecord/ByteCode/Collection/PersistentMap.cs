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
    using NHibernate.Engine;
    using System.Collections;
    using NHibernate.Persister.Collection;

    class PersistentMap : NHibernate.Collection.PersistentMap
    {
        public PersistentMap() { }

		public PersistentMap(ISessionImplementor session) : base(session) { }

        public PersistentMap(ISessionImplementor session, IDictionary map) : base(session, map) { }

        public bool IsLazy;
        private bool sessionConfirmed;

        protected override bool ReadSize() {
            if (!IsLazy) return base.ReadSize();

            ISession newSession = null;
            if (!initialized) {
                if (cachedSize != -1 && !HasQueuedOperations) {
                    return true;
                }
                else newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            }
            var ret = base.ReadSize();
            this.cleanupSession(newSession, ref sessionConfirmed);
            return ret;
        }

        protected override bool? ReadIndexExistence(object index) {
            if (!IsLazy) return base.ReadIndexExistence(index);

            ISession newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            var ret = base.ReadIndexExistence(index);
            this.cleanupSession(newSession, ref sessionConfirmed);
            return ret;
        }

        protected override bool? ReadElementExistence(object element) {
            if (!IsLazy) return base.ReadElementExistence(element);

            ISession newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            var ret = base.ReadElementExistence(element);
            this.cleanupSession(newSession, ref sessionConfirmed);
            return ret;
        }

        protected override object ReadElementByIndex(object index) {
            if (!IsLazy) return base.ReadElementByIndex(index);

            ISession newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            var ret = base.ReadElementByIndex(index);
            this.cleanupSession(newSession, ref sessionConfirmed);
            return ret;
        }

        protected override void Initialize(bool writing) {
            if (!IsLazy) base.Initialize(writing);

            ISession newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            base.Initialize(writing);
            this.cleanupSession(newSession, ref sessionConfirmed);
        }

        public override void ForceInitialization() {
            if (!IsLazy) base.ForceInitialization();

            ISession newSession = this.checkSession(initialized, Session, ref sessionConfirmed);
            base.ForceInitialization();
            this.cleanupSession(newSession, ref sessionConfirmed);
        }
    }
}
