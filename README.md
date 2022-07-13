# How to deploy a SignalR Core application on AWS

This repository contains an example about how you can deploy a SignalR Core App on AWS.

The source code for the SignalR Core app is from here:
- https://github.com/aspnet/AzureSignalR-samples/tree/main/samples/ChatRoom

In the ``/cdk`` folder you can find an AWS CDK app that creates the following infrastructure on AWS:

- A VPC with ``10.55.0.0/16`` CIDR range.
- An Application Load Balancer.
- A Fargate service.
- A ElasticCache instance.
- A NAT Gateway.

![diagram](https://raw.githubusercontent.com/karlospn/deploy-signalr-core-app-on-aws/main/docs/signalr_core_application.png)
