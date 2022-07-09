using System.Collections.Generic;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Constructs;
using Protocol = Amazon.CDK.AWS.EC2.Protocol;

namespace FargateCdkStack.Constructs
{
    public class EcsServiceConstruct : Construct
    {
        public FargateService FargateService { get; }

        public EcsServiceConstruct(Construct scope,
            string id,
            Vpc vpc,
            Cluster cluster,
            ApplicationLoadBalancer pubAlb)
            : base(scope, id)
        {
            var task = CreateTaskDefinition();
            FargateService = CreateEcsService(vpc, cluster, pubAlb, task);
        }

        private FargateTaskDefinition CreateTaskDefinition()
        {
            var task = new FargateTaskDefinition(this,
                "task-definition-ecs-signalr-core-demo",
                new FargateTaskDefinitionProps
                {
                    Cpu = 512,
                    Family = "task-definition-ecs-signalr-core-demo",
                    MemoryLimitMiB = 1024
                });

            task.AddContainer("signalr-chatroom-app",
                new ContainerDefinitionOptions
                {
                    Cpu = 512,
                    MemoryLimitMiB = 1024,
                    Image = ContainerImage.FromAsset("../src/ChatRoom"),
                    Logging = LogDriver.AwsLogs(new AwsLogDriverProps
                    {
                        StreamPrefix = "ecs"
                    }),
                    PortMappings = new IPortMapping[]
                    {
                        new PortMapping
                        {
                            ContainerPort = 80
                        }
                    }
                });

            return task;
        }

        private FargateService CreateEcsService(Vpc vpc,
            Cluster cluster,
            ApplicationLoadBalancer pubAlb,
            FargateTaskDefinition task)
        {
            
            var sg = new SecurityGroup(this,
                "scg-svc-ecs-signalr-core-demo",
                new SecurityGroupProps
                {
                    SecurityGroupName = "scg-svc-ecs-signalr-core-demo",
                    Description = "Allow traffic from ALB to app",
                    AllowAllOutbound = true,
                    Vpc = vpc
                });

            sg.Connections.AllowFrom(pubAlb.Connections, new Port(new PortProps
            {
                FromPort = 80,
                ToPort = 80,
                Protocol = Protocol.TCP,
                StringRepresentation = string.Empty
            }));

            var service = new FargateService(this,
                "service-ecs-signalr-core-demo",
                new FargateServiceProps
                {
                    TaskDefinition = task,
                    Cluster = cluster,
                    DesiredCount = 2,
                    MinHealthyPercent = 100,
                    MaxHealthyPercent = 200,
                    AssignPublicIp = true,
                    VpcSubnets = new SubnetSelection
                    {
                        Subnets = vpc.PrivateSubnets
                    },
                    SecurityGroups = new ISecurityGroup[] { sg },
                    ServiceName = "service-ecs-signalr-core-demo",
                });

            return service;
        }
    }
}
