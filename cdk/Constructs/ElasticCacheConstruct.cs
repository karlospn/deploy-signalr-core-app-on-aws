using System.Linq;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElastiCache;
using Constructs;
using Protocol = Amazon.CDK.AWS.EC2.Protocol;

namespace FargateCdkStack.Constructs
{
    public class ElasticCacheConstruct : Construct
    {
        public CfnCacheCluster Redis { get; }
        public SecurityGroup RedisSg  { get; }

        public ElasticCacheConstruct(Construct scope, 
            string id,
            Vpc vpc) 
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

            RedisSg = new SecurityGroup(this,
                "scg-redis-cluster-signalr-core-demo",
                new SecurityGroupProps()
                {
                    Vpc = vpc,
                    AllowAllOutbound = true,
                    Description = "Security group for the signalR demo redis cluster",
                    SecurityGroupName = "scg-redis-cluster-signalr-core-demo"
                });

            Redis = new CfnCacheCluster(this,
                "redis-cluster-signalr-core-demo",
                new CfnCacheClusterProps
                {
                    Engine = "redis",
                    CacheNodeType = "cache.t3.micro",
                    NumCacheNodes = 1,
                    ClusterName = "redis-signalr-core-demo",
                    Port = 6379,
                    CacheSubnetGroupName = subnetGroup.CacheSubnetGroupName,
                    VpcSecurityGroupIds = new []{RedisSg.SecurityGroupId},
                });
        }
    }
}
