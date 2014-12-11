using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class QueryListHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public QueryListHttpRequestFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual HttpRequest Create(QueryListRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var httpRequest = request.HasKeys
                ? new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request)).SetJsonContent(GenerateRequestBody(request))
                : new HttpRequest(HttpMethod.Get, GenerateRelativeUrl(request));

            httpRequest.SetRequestTypeHeader(request.GetType());

            return httpRequest;
        }

        protected virtual string GenerateRelativeUrl(QueryListRequest request)
        {
            return string.Format("/_design/{0}/_list/{1}/{2}{3}",
                    request.ListIdentity.DesignDocument,
                    request.ListIdentity.Name,
                    request.ViewName,
                    GenerateRequestUrlQueryString(request));
        }

        protected virtual string GenerateRequestBody(QueryListRequest request)
        {
            return request.HasKeys
                ? Serializer.Serialize(new { keys = request.Keys })
                : "{}";
        }

        protected virtual string GenerateRequestUrlQueryString(QueryListRequest request)
        {
            var p = GenerateQueryStringParams(request);

            return string.IsNullOrWhiteSpace(p) ? string.Empty : string.Concat("?", p);
        }

        protected virtual string GenerateQueryStringParams(QueryListRequest request)
        {
            return string.Join("&", GenerateJsonCompatibleKeyValues(request)
                .Select(kv => string.Format("{0}={1}", kv.Key, UrlParam.Encode(kv.Value))));
        }

        /// <summary>
        /// Returns all configured options of <see cref="QueryListRequest"/> as key values.
        /// The values are formatted to JSON-compatible strings.
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, string> GenerateJsonCompatibleKeyValues(QueryListRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            var kvs = new Dictionary<string, string>();

            if (request.IncludeDocs.HasValue)
                kvs.Add(KeyNames.IncludeDocs, Serializer.ToJson(request.IncludeDocs.Value));

            if (request.Descending.HasValue)
                kvs.Add(KeyNames.Descending, Serializer.ToJson(request.Descending.Value));

            if (request.Reduce.HasValue)
                kvs.Add(KeyNames.Reduce, Serializer.ToJson(request.Reduce.Value));

            if (request.InclusiveEnd.HasValue)
                kvs.Add(KeyNames.InclusiveEnd, Serializer.ToJson(request.InclusiveEnd.Value));

            if (request.UpdateSeq.HasValue)
                kvs.Add(KeyNames.UpdateSeq, Serializer.ToJson(request.UpdateSeq.Value));

            if (request.Group.HasValue)
                kvs.Add(KeyNames.Group, Serializer.ToJson(request.Group.Value));

            if (request.GroupLevel.HasValue)
                kvs.Add(KeyNames.GroupLevel, Serializer.ToJson(request.GroupLevel.Value));

            if (request.Stale.HasValue)
                kvs.Add(KeyNames.Stale, request.Stale.Value.AsString());

            if (request.Key != null)
                kvs.Add(KeyNames.Key, Serializer.ToJson(request.Key));

            if (request.StartKey != null)
                kvs.Add(KeyNames.StartKey, Serializer.ToJson(request.StartKey));

            if (!string.IsNullOrWhiteSpace(request.StartKeyDocId))
                kvs.Add(KeyNames.StartKeyDocId, request.StartKeyDocId);

            if (request.EndKey != null)
                kvs.Add(KeyNames.EndKey, Serializer.ToJson(request.EndKey));

            if (!string.IsNullOrWhiteSpace(request.EndKeyDocId))
                kvs.Add(KeyNames.EndKeyDocId, request.EndKeyDocId);

            if (request.Limit.HasValue)
                kvs.Add(KeyNames.Limit, Serializer.ToJson(request.Limit.Value));

            if (request.Skip.HasValue)
                kvs.Add(KeyNames.Skip, Serializer.ToJson(request.Skip.Value));

            if(request.HasAdditionalQueryParameters)
            {
                foreach (var param in request.AdditionalQueryParameters)
                    kvs.Add(param.Key, Serializer.ToJson(param.Value));
            }

            return kvs;
        }

        protected static class KeyNames
        {
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Reduce = "reduce";
            public const string InclusiveEnd = "inclusive_end";
            public const string UpdateSeq = "update_seq";
            public const string Group = "group";
            public const string GroupLevel = "group_level";
            public const string Stale = "stale";
            public const string Key = "key";
            public const string StartKey = "startkey";
            public const string StartKeyDocId = "startkey_docid";
            public const string EndKey = "endkey";
            public const string EndKeyDocId = "endkey_docid";
            public const string Limit = "limit";
            public const string Skip = "skip";
        }
    }
}