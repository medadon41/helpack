using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;


IPAddress localAddress = IPAddress.Parse("127.0.0.1");

List<string>? messages = new();

XmlSerializer serializer = new XmlSerializer(typeof(List<string>));

Console.Write("Enter your username: ");
string? username = Console.ReadLine();

Console.Write("Enter message listener port: ");
if (!int.TryParse(Console.ReadLine(), out var localPort)) return;

Console.Write("Enter message sender port: ");
if (!int.TryParse(Console.ReadLine(), out var remotePort)) return;

Console.WriteLine();

TryLoadHistory();

AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);  

Task.Run(ReceiveMessageAsync);

await SendMessageAsync();

async Task SendMessageAsync()
{
    using Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    Console.WriteLine("Write a message and press [Enter] to send it.");

    while (true)
    {
        var message = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(message))
        {
            using FileStream fs = new FileStream($"{username}_s history.xml", FileMode.OpenOrCreate);
            serializer.Serialize(fs, messages);
            Console.WriteLine("Disconnected. Message history has been saved.");
            break;
        }

        message = $"{username}: {message}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        
        await sender.SendToAsync(data, new IPEndPoint(localAddress, remotePort));
        
        messages.Add(message);
    }
}

async Task ReceiveMessageAsync()
{
    byte[] data = new byte[65535];

    using Socket receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    receiver.Bind(new IPEndPoint(localAddress, localPort));
    while (true)
    {

        var result = await receiver.ReceiveFromAsync(data, new IPEndPoint(IPAddress.Any, 0));
        var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);

        Console.WriteLine(message);
        
        messages.Add(message);
    }
}

void TryLoadHistory()
{
    if (!File.Exists($"{username}_s history.xml")) return;
    using FileStream fs = new FileStream($"{username}_s history.xml", FileMode.Open);
    var msgs = serializer.Deserialize(fs) as List<string>;
    Console.WriteLine("Message history from previous session:");
    foreach (var message in msgs)
    {
        Console.WriteLine(message);
    }
}

void FlushHistory()
{
    using FileStream fs = new FileStream($"{username}_s history.xml", FileMode.OpenOrCreate);
    serializer.Serialize(fs, messages);
}

void CurrentDomain_ProcessExit(object? sender, EventArgs e)
{
    FlushHistory();
}
