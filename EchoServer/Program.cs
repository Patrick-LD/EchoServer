// Vi skal bruge disse to "værktøjskasser" (namespaces).
// System.Net        -> indeholder IPAddress (som fortæller hvilken adresse vi lytter på).
// System.Net.Sockets -> indeholder TcpListener, TcpClient og NetworkStream (selve netværket).
using System.Net;
using System.Net.Sockets;
using System.Text; // bruges til at lave tekst om til bytes og tilbage igen.

// 1) Opret en TcpListener der lytter på port 5000.
//    IPAddress.Any betyder "lyt på alle netværkskort på denne computer".
TcpListener listener = new TcpListener(IPAddress.Any, 5000);

// 2) Start med at lytte efter forbindelser.
listener.Start();

Console.WriteLine("Echo Server started...");
Console.WriteLine("Waiting for client...");

// 3) Vent her, indtil en client forbinder, og accepter så forbindelsen.
//    AcceptTcpClient() blokerer (venter) indtil nogen forbinder.
TcpClient client = listener.AcceptTcpClient();

Console.WriteLine();
Console.WriteLine("Client connected!");

// 4) Hent forbindelsens NetworkStream.
//    Det er "røret" vi læser fra og skriver til.
NetworkStream stream = client.GetStream();

// 5) Modtag en besked fra clienten.
//    Vi laver en buffer (en spand) med plads til 1024 bytes.
byte[] buffer = new byte[1024];

//    Read() læser de bytes clienten sendte, og returnerer hvor mange der kom.
int antalBytes = stream.Read(buffer, 0, buffer.Length);

//    Lav de modtagne bytes om til almindelig tekst.
string besked = Encoding.UTF8.GetString(buffer, 0, antalBytes);

// 6) Udskriv beskeden i konsollen.
Console.WriteLine();
Console.WriteLine("Received: " + besked);

// 7) Send den SAMME besked tilbage til clienten (derfor hedder det "echo").
Console.WriteLine("Sending: " + besked);

//    Lav teksten om til bytes igen, og skriv dem ud i stream'en.
byte[] svar = Encoding.UTF8.GetBytes(besked);
stream.Write(svar, 0, svar.Length);

// 8) Luk pænt ned, når vi er færdige.
client.Close();
listener.Stop();
