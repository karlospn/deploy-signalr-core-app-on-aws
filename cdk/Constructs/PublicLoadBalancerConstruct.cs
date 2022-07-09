using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Constructs;

namespace FargateCdkStack.Constructs
{
    public class PublicLoadBalancerConstruct : Construct
    {
        public ApplicationLoadBalancer Alb { get; }

        public PublicLoadBalancerConstruct(Construct scope,
            string id,
            Vpc vpc)
            : base(scope, id)
        {
            var securityGroup = new SecurityGroup(this,
                "scg-pub-alb-signalr-core-demo",
                new SecurityGroupProps()
                {
                    Vpc = vpc,
                    AllowAllOutbound = true,
                    Description = "Security group for the public ALB",
                    SecurityGroupName = "scg-pub-alb-ecs-profiling-dotnet-demo"
                });

            securityGroup.AddIngressRule(Peer.AnyIpv4(),
                Port.Tcp(80),
                "Allow port 80 ingress traffic");

            Alb = new ApplicationLoadBalancer(this,
                "alb-pub-signalr-core-demo",
                new ApplicationLoadBalancerProps
                {
                    InternetFacing = true,
                    Vpc = vpc,
                    VpcSubnets = new SubnetSelection
                    {
                        OnePerAz = true,
                        SubnetType = SubnetType.PUBLIC,
                    },
                    SecurityGroup = securityGroup,
                    LoadBalancerName = "alb-pub-signalr-core-demo"
                });

            _ = Alb.AddListener("alb-http-listener", new ApplicationListenerProps
            {
                Protocol = ApplicationProtocol.HTTP,
                LoadBalancer = Alb,
                DefaultAction = ListenerAction.FixedResponse(500),
            });
        }
    }
}
