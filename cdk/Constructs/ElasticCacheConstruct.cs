using System.Linq;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ElastiCache;
using Constructs;
using Protocol = Amazon.CDK.AWS.EC2.Protocol;

namespace FargateCdkStack.Constructs
{
    public class ElasticCacheConstruct : Construct
    {
        public ElasticCacheConstruct(Construct scope, 
            string id,
            Vpc vpc,
            FargateService service) 
            : base(scope, id)
        {

            var subnetGroup = new CfnSubnetGroup(this,
                "redis-subnet-group-signalr-core-demo",
                new CfnSubnetGroupProps
                {
                    Description = "Subnet group for the signalR demo redis cluster",
                    CacheSubnetGroupName = "redis-subnet-group-signalr-core-demo",
                    SubnetIds = vpc.PrivateSubnets.Select(x => x.SubnetId).ToArray(),
                });

            var securityGroup = new SecurityGroup(this,
                "scg-redis-cluster-signalr-core-demo",
                new SecurityGroupProps()
                {
                    Vpc = vpc,
                    AllowAllOutbound = true,
                    Description = "Security group for the signalR demo redis cluster",
                    SecurityGroupName = "scg-redis-cluster-signalr-core-demo"
                });

            securityGroup.Connections.AllowFrom(service.Connections, new Port(new PortProps
            {
                FromPort = 6379,
                ToPort = 6379,
                Protocol = Protocol.TCP,
                StringRepresentation = "Allow port 6379 from fargate app"
            }));


            var redisCache = new CfnCacheCluster(this,
                "redis-cluster-signalr-core-demo",
                new CfnCacheClusterProps
                {
                    Engine = "redis",
                    CacheNodeType = "cache.t3.micro",
                    NumCacheNodes = 1,
                    ClusterName = "redis-signalr-core-demo",
                    Port = 6379,
                    CacheSubnetGroupName = subnetGroup.CacheSubnetGroupName,
                    VpcSecurityGroupIds = new []{securityGroup.SecurityGroupId},
                });

        }

    }
}
