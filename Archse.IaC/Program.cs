using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.ServiceBus;
using Pulumi.AzureNative.ServiceBus.Inputs;

class MyStack : Stack
{
    public MyStack()
    {
        var config = new Config();
        var subscriptionId = config.Require("azure-native:subscriptionId");
    
        var resourceGroup = new ResourceGroup("my-resource-group", new ResourceGroupArgs
        {
            ResourceGroupName = "myResourceGroup",
            Location = "WestUS"
        });

        var serviceBusNamespace = new Namespace("my-servicebus-namespace", new NamespaceArgs
        {
            ResourceGroupName = resourceGroup.Name,
            NamespaceName = "myNamespace",
            Location = resourceGroup.Location,
            Sku = new SBSkuArgs
            {
                Name = SkuName.Standard,
                Tier = SkuTier.Standard
            }
        });

        var queue = new Queue("my-servicebus-queue", new QueueArgs
        {
            ResourceGroupName = resourceGroup.Name,
            NamespaceName = serviceBusNamespace.Name,
            QueueName = "myQueue"
        });
    }
}

class Program
{
    static Task<int> Main(string[] args)
    {
        return Pulumi.Deployment.RunAsync<MyStack>();
    }
}