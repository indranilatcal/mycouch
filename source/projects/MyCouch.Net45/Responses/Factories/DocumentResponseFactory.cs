﻿using System.Net.Http;
using EnsureThat;
using MyCouch.Responses.Materializers;
using MyCouch.Serialization;

namespace MyCouch.Responses.Factories
{
    public class DocumentResponseFactory : ResponseFactoryBase<DocumentResponse>
    {
        protected readonly DocumentResponseMaterializer SuccessfulResponseMaterializer;
        protected readonly FailedResponseMaterializer FailedResponseMaterializer;

        public DocumentResponseFactory(ISerializer serializer)
        {
            Ensure.That(serializer, "serializer").IsNotNull();

            SuccessfulResponseMaterializer = new DocumentResponseMaterializer(serializer);
            FailedResponseMaterializer = new FailedResponseMaterializer(serializer);
        }

        protected override void MaterializeSuccessfulResponse(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            SuccessfulResponseMaterializer.Materialize(response, httpResponse);
        }

        protected override void MaterializeFailedResponse(DocumentResponse response, HttpResponseMessage httpResponse)
        {
            FailedResponseMaterializer.Materialize(response, httpResponse);
        }
    }
}