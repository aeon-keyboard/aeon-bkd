using RabbitMQ.Client;
using System.Text;

namespace Aeon.Infra.Messaging;

public class RabbitMqPublisher(IConnection conn)
{
    private readonly IConnection _conn = conn ?? throw new ArgumentNullException(nameof(conn), "RabbitMQ connection cannot be null");

    public void PublishKeymapGenerated(Guid id)
    {
        if (!_conn.IsOpen)
        {
            throw new InvalidOperationException("RabbitMQ connection is closed");
        }

        using var channel = _conn.CreateModel();
        var msg = Encoding.UTF8.GetBytes(id.ToString());
        channel.BasicPublish(exchange: "", routingKey: "KeymapGenerated", body: msg);
    }
}