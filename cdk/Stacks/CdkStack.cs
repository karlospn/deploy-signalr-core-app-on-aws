using Amazon.CDK;
using Constructs;
using FargateCdkStack.Constructs;

namespace FargateCdkStack.Stacks
{
    public class CdkStack : Stack
    {
        internal CdkStack(Construct scope,
            string id,
            IStackProps props = null)
            : base(scope, id, props)
        {
            var vpc = new VpcConstruct(this,
                "vpc-construct-signalr-core-demo");

            var fg = new FargateClusterConstruct(this,
                "fg-cluster-construct-signalr-core-demo", 
                vpc.Vpc);

            var publicAlb = new PublicLoadBalancerConstruct(this,
                "pub-alb-construct-signalr-core-demo",
                vpc.Vpc);

            var cache = new ElasticCacheConstruct(this,
                "redis-cache-construct-signalr-core-demo",
                vpc.Vpc);

            var service = new EcsServiceConstruct(this,
                "ecs-service-construct-signalr-core-demo",
                vpc.Vpc,
                fg.Cluster,
                publicAlb.Alb,
                cache.Redis,
                cache.RedisSg);

            _ = new TargetGroupConstruct(this,
                "alb-tg-construct-signalr-core-demo",
                vpc.Vpc,
                publicAlb.Alb,
                service.FargateService);

        }
    }
}
