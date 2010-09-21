using System;
using System.Collections.Generic;

namespace CSharpCompiler.Runtime.Messages
{
    public class BuildStatisticMessage : TeamCityServiceMessageWithAttributes
    {
        private readonly object key;
        private readonly int value;

        public static readonly ICollection<string> PredefinedStatisticsKeys = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
                                                                           {
                                                                               "ArtifactsSize",
                                                                               "BuildArtifactsPublishingTime",
                                                                               "BuildCheckoutTime",
                                                                               "BuildDuration",
                                                                               "CodeCoverageB",
                                                                               "CodeCoverageC",
                                                                               "CodeCoverageL",
                                                                               "CodeCoverageM",
                                                                               "CodeCoverageAbsLCovered",
                                                                               "CodeCoverageAbsMCovered",
                                                                               "CodeCoverageAbsCCovered",
                                                                               "CodeCoverageAbsLTotal",
                                                                               "CodeCoverageAbsMTotal",
                                                                               "CodeCoverageAbsCTotal",
                                                                               "DuplicatorStats",
                                                                               "FailedTestCount",
                                                                               "IgnoredTestCount",
                                                                               "InspectionStatsE",
                                                                               "InspectionStatsW",
                                                                               "PassedTestCount",
                                                                               "SuccessRate",
                                                                               "TestCount",
                                                                               "TimeSpentInQueue"
                                                                           };

        public BuildStatisticMessage(object key, int value) : base("buildStatisticValue")
        {
            if (value < 0)
                throw new InvalidOperationException("Statistic values should be positive integers");

            if (PredefinedStatisticsKeys.Contains(key.ToString()))
                throw new InvalidOperationException("Statistic keys should not interfere with default keys");

            this.key = key;
            this.value = value;
        }

        protected override IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
                return new Dictionary<string, object> { { "key", key }, { "value", value } };
            }
        }
    }
}
