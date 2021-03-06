using System.Linq;
using EnsureThat;

namespace MyCouch
{
    public class QueryParametersConfigurator
    {
        protected readonly IQueryParameters Parameters;

        public QueryParametersConfigurator(IQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Stale(Stale value)
        {
            Parameters.Stale = value;

            return this;
        }

        /// <summary>
        /// Include the full content of the documents in the return;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator IncludeDocs(bool value)
        {
            Parameters.IncludeDocs = value;

            return this;
        }

        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Descending(bool value)
        {
            Parameters.Descending = value;

            return this;
        }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetKey(value);
        }

        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key<T>(T value)
        {
            return SetKey(value);
        }

        /// <summary>
        /// Return only documents that match the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Key<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetKey(value);
        }

        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Keys<T>(params T[] value)
        {
            Ensure.That(value, "value").HasItems();

            Parameters.Keys = value as object[];

            return this;
        }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey<T>(T value)
        {
            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified complex-key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKey<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetStartKey(value);
        }

        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator StartKeyDocId(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.StartKeyDocId = value;

            return this;
        }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKey(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKey<T>(T value)
        {
            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified complex-key is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKey<T>(T[] value)
        {
            Ensure.That(value, "value").HasItems();

            return SetEndKey(value);
        }

        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator EndKeyDocId(string value)
        {
            Ensure.That(value, "value").IsNotNullOrWhiteSpace();

            Parameters.EndKeyDocId = value;

            return this;
        }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator InclusiveEnd(bool value)
        {
            Parameters.InclusiveEnd = value;

            return this;
        }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Skip(int value)
        {
            Parameters.Skip = value;

            return this;
        }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Limit(int value)
        {
            Parameters.Limit = value;

            return this;
        }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Reduce(bool value)
        {
            Parameters.Reduce = value;

            return this;
        }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator UpdateSeq(bool value)
        {
            Parameters.UpdateSeq = value;

            return this;
        }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator Group(bool value)
        {
            Parameters.Group = value;

            return this;
        }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual QueryParametersConfigurator GroupLevel(int value)
        {
            Parameters.GroupLevel = value;

            return this;
        }

        protected virtual QueryParametersConfigurator SetKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.Key = value;

            return this;
        }

        protected virtual QueryParametersConfigurator SetStartKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.StartKey = value;

            return this;
        }

        protected virtual QueryParametersConfigurator SetEndKey(object value)
        {
            Ensure.That(value, "value").IsNotNull();

            Parameters.EndKey = value;

            return this;
        }
    }
}