using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Constructs;

namespace FargateCdkStack.Constructs
{
    public class FargateClusterConstruct : Construct
    {
        public Cluster Cluster { get; }

        public FargateClusterConstruct(Construct scope,
            string id,
            Vpc vpc)
            : base(scope, id)
        {
            Cluster = new Cluster(this,
                "fargate-cluster-signalr-core-demo",
                new ClusterProps
                {
                    Vpc = vpc,
                    ClusterName = "fargate-cluster-signalr-core-demo",
                });
        }
    }
}
