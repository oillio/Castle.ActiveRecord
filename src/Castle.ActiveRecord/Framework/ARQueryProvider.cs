// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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


namespace Castle.ActiveRecord.Framework
{
    using System;
    using System.Linq.Expressions;
    using NHibernate;
    using NHibernate.Linq;

    class ARQueryProvider : NhQueryProvider
    {

        private readonly ISessionFactoryHolder holder;
        private readonly Type targetType;

        public ARQueryProvider(ISessionFactoryHolder holder, Type targetType) : base(null)
        {
            this.holder = holder;
            this.targetType = targetType;
        }

        public override object Execute(Expression expression)
        {
            ISession session = holder.CreateSession(targetType);

            try
            {
                IQuery query;
                NhLinqExpression nhQuery;
                NhLinqExpression nhLinqExpression = PrepareQuery(expression, session.GetSessionImplementation(), out query, out nhQuery);

                return ExecuteQuery(nhLinqExpression, query, nhQuery);
            }
            catch(Exception ex)
            {
                holder.FailSession(session);

                throw new ActiveRecordException("Cold not execute linq query for " + targetType.Name, ex);
            }
            finally
            {
                holder.ReleaseSession(session);
            }
        }

        public override object ExecuteFuture(Expression expression)
        {
            ISession session = holder.CreateSession(targetType);

            try
            {
                IQuery query;
                NhLinqExpression nhQuery;
                NhLinqExpression nhLinqExpression = PrepareQuery(expression, session.GetSessionImplementation(), out query, out nhQuery);
                return ExecuteFutureQuery(nhLinqExpression, query, nhQuery);
            }
            catch (Exception ex)
            {
                holder.FailSession(session);

                throw new ActiveRecordException("Cold not execute linq query for " + targetType.Name, ex);
            }
            finally
            {
                holder.ReleaseSession(session);
            }
        }
    }
}
