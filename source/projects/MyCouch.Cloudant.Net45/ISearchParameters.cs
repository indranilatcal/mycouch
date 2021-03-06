﻿using System.Collections.Generic;

namespace MyCouch.Cloudant
{
    /// <summary>
    /// The different common search parameters that can be specified
    /// when performing a query against a Search-index.
    /// </summary>
    public interface ISearchParameters
    {
        /// <summary>
        /// Identitfies the Search index that this request will be
        /// performed against.
        /// </summary>
        SearchIndexIdentity IndexIdentity { get; }

        /// <summary>
        /// The Lucene expression that will be used to query the index.
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// Allow the results from a stale search index to be used.
        /// </summary>
        Stale? Stale { get; set; }

        /// <summary>
        /// A bookmark that was received from a previous search. This
        /// allows you to page through the results. If there are no more
        /// results after the bookmark, you will get a response with an
        /// empty rows array and the same bookmark. That way you can
        /// determine that you have reached the end of the result list.
        /// </summary>
        string Bookmark { get; set; }

        /// <summary>
        /// Sort expressions used to sort the output.
        /// </summary>
        IList<string> Sort { get; set; }

        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        bool? IncludeDocs { get; set; }

        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        int? Limit { get; set; } 
    }
}