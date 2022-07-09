using Amazon.CDK.AWS.EC2;
using Constructs;

namespace FargateCdkStack.Constructs
{
    public class VpcConstruct : Construct
    {
        public Vpc Vpc { get; set; }
        public VpcConstruct(Construct scope, string id)
            : base(scope, id)
        {
            Vpc = new Vpc(this,
                "vpc-signalr-core-demo",
                new VpcProps
                {
                    Cidr = "10.55.0.0/16",
                    MaxAzs = 2,
                    NatGateways = 1,
                    VpcName = "vpc-signalr-core-demo",
                    SubnetConfiguration = new ISubnetConfiguration[]
                    {
                        new SubnetConfiguration
                        {
                            Name = "subnet-public-signalr-core-demo",
                            CidrMask = 24,
                            SubnetType = SubnetType.PUBLIC,
                        },
                        new SubnetConfiguration
                        {
                            Name = "subnet-private-signalr-core-demo",
                            CidrMask = 24,
                            SubnetType = SubnetType.PRIVATE_WITH_NAT,
                        }
                    }
                });
        }
    }
}
