from diagrams import Diagram
from diagrams import Cluster, Diagram
from diagrams.aws.compute import Fargate
from diagrams.aws.network import ALB
from diagrams.aws.database import ElasticacheForRedis
from diagrams.aws.network import NATGateway

with Diagram("SignalR Core Application", show=False):
    
    source = ALB("lb")
    
    workers = [Fargate("worker1"),
                Fargate("worker2"),
                Fargate("worker3")]

    cache = ElasticacheForRedis("backplane")

    source >> workers >> cache
