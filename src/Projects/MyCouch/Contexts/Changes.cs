﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyCouch.Extensions;
using MyCouch.Requests;
using MyCouch.Responses;

namespace MyCouch.Contexts
{
    public class Changes : ApiContextBase, IChanges
    {
        public Changes(IConnection connection) : base(connection) {}

        public virtual Task<ChangesResponse> GetAsync(ChangesFeed feed)
        {
            return GetAsync(new GetChangesRequest(feed));
        }

        public virtual async Task<ChangesResponse> GetAsync(GetChangesRequest cmd)
        {
            Ensure.That(cmd, "cmd").IsNotNull();

            using (var req = CreateRequest(cmd))
            {
                using (var res = await SendAsync(req).ForAwait())
                {
                    return ProcessHttpResponse(res);
                }
            }
        }

        protected virtual HttpRequestMessage CreateRequest(GetChangesRequest cmd)
        {
            throw new NotImplementedException();
        }

        protected virtual ChangesResponse ProcessHttpResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }
    }
}