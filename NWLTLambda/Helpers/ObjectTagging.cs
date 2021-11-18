using System;
using System.Collections.Generic;
using System.Text;
using NWLTLambda.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;

namespace NWLTLambda.Helpers
{
    public class ObjectTagging
    {
        public bool tagObject(List<string> Tags)
        {
            PutObjectTaggingRequest request = new PutObjectTaggingRequest();
            TaggingModel tagModel = new TaggingModel();
            bool response = true;

            request.BucketName = "";
            request.Key = "";
            //request.Tagging = ;

            return response;
        }
    }
}
