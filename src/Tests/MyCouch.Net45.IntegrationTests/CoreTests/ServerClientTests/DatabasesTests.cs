﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using MyCouch.Requests;
using MyCouch.Requests.Factories;
using MyCouch.Testing;
using MyCouch.Testing.TestData;

namespace MyCouch.IntegrationTests.CoreTests.ServerClientTests
{
    public class DatabasesTests : IntegrationTestsOf<IDatabases>
    {
        public DatabasesTests()
        {
            SUT = ServerClient.Databases;
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Head_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.HeadAsync(Environment.PrimaryDbName).Result;

            response.Should().Be(HttpMethod.Head);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_Get_of_existing_db_The_response_should_be_200()
        {
            var response = SUT.GetAsync(Environment.PrimaryDbName).Result;

            response.Should().BeAnyJson(HttpMethod.Get);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.CompactDbs)]
        public void When_Compact_of_existing_db_The_response_should_be_202()
        {
            var response = SUT.CompactAsync(Environment.TempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext)]
        public void When_ViewCleanup_and_db_exists_The_response_be()
        {
            var response = SUT.ViewCleanupAsync(Environment.TempDbName).Result;

            response.Should().BeOkJson(HttpMethod.Post, HttpStatusCode.Accepted);
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_no_changes_exists_The_response_indicates_success()
        {
            var response = SUT.ReplicateAsync(Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulButEmptyReplication();
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_and_changes_exists_The_response_indicates_success()
        {
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var response = SUT.ReplicateAsync(Environment.PrimaryDbName, Environment.SecondaryDbName).Result;

            response.Should().BeSuccessfulNonEmptyReplication();
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_using_proxy_and_changes_exists_The_response_indicates_success()
        {
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                Proxy = Environment.ServerUrl
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulNonEmptyReplication();
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void When_Replicate_using_doc_ids_and_changes_exists_The_response_indicates_success()
        {
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                DocIds = new[] { ClientTestData.Artists.Artist1Id }
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulNonEmptyReplication();
        }

        [MyFact(TestScenarios.DatabasesContext, TestScenarios.Replication)]
        public void Can_do_continuous_replication()
        {
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist1Json);
            DbClient.Documents.PostAsync(ClientTestData.Artists.Artist2Json);

            var request = new ReplicateDatabaseRequest(Environment.PrimaryDbName, Environment.SecondaryDbName)
            {
                Continuous = true
            };

            var response = SUT.ReplicateAsync(request).Result;

            response.Should().BeSuccessfulContinousReplication();

            request.Cancel = true;

            var cancellationResponse = SUT.ReplicateAsync(request).Result;
            cancellationResponse.Should().BeSuccessfulCancelledContinousReplication(response.LocalId);
        }
    }
}