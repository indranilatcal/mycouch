﻿using EnsureThat;
using MyCouch.Cloudant.Requests;
using MyCouch.Net;
using MyCouch.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace MyCouch.Cloudant.HttpRequestFactories
{
    public class IndexHttpRequestFactory
    {
        private const string JsonPropertyAppendFormat = ",\"{0}\": \"{1}\"";
        protected ISerializer Serializer { get; private set; }

        public IndexHttpRequestFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "Serializer").IsNotNull();

            Serializer = serializer;
        }

        public virtual HttpRequest Create(PostIndexRequest request)
        {
            Ensure.That(request, "request").IsNotNull();

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(GenerateRequestBody(request));
        }

        protected virtual string GenerateRequestBody(PostIndexRequest request)
        {
            Ensure.That(request.Fields, "request.Fields").HasItems();

            var sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("{0}", GenerateIndexContent(request.Fields));
            if (!string.IsNullOrWhiteSpace(request.DesignDocument))
                sb.AppendFormat(JsonPropertyAppendFormat, KeyNames.DesignDocument, request.DesignDocument);
            if (request.Type.HasValue)
                sb.AppendFormat(JsonPropertyAppendFormat, KeyNames.Type, request.Type.Value.ToString());
            if (!string.IsNullOrWhiteSpace(request.Name))
                sb.AppendFormat(JsonPropertyAppendFormat, KeyNames.Name, request.Name);

            sb.Append("}");

            return sb.ToString();
        }

        protected virtual string GenerateIndexContent(IList<IndexField> fields)
        {
            return string.Format("\"{0}\": {1}", KeyNames.Index,
                Serializer.Serialize(new { fields = fields.ToArray() }));
        }

        protected virtual string GenerateRelativeUrl(PostIndexRequest request)
        {
            return "/_index";
        }

        protected static class KeyNames
        {
            public const string Index = "index";
            public const string DesignDocument = "ddoc";
            public const string Type = "type";
            public const string Name = "name";
        }
    }
}