﻿using System;
using System.Net;
using System.Threading.Tasks;
using BinarySerialization;
using PipelineNet.Middleware;
using Texnomic.DNS.Abstractions;
using Texnomic.DNS.Protocols;
using Texnomic.DNS.Extensions;
using Texnomic.DNS.Models;

namespace Texnomic.DNS.Servers.Middlewares
{
    public class GoogleHTTPsMiddleware : HTTPs, IAsyncMiddleware<IMessage, IMessage>
    {
        private readonly BinarySerializer BinarySerializer;

        public GoogleHTTPsMiddleware() : base(IPAddress.Parse("8.8.8.8"),
            "3082010A0282010100968CDA0112B4D30B36AB5DE74D242F9ECFDB6E491CC70672E044ECD148AAB07430C5C3EE2BC81B0EA24405D163B6EFACE5C2693D3B3D7BEFF3B676460814B5AACDAA7ED94CADC9166403FA65951681C4C200B032C65D5CDE5798A86A6131E790EDB3CF02A73696121F40E8F71F3345BD9E2C39F443EE37ABF19ADA88E433F637653113ABB2FBDBB2FC5C83E72108117E6B0482FEE6F2C59ADFF151C4968DB0EAE69142D5159FD614D5C1B0660812EC6B577C074D1800EDDCA18AF125C4CF6A8EDB940014B8512A8BC881441A45799024AB9ECF22AF96666A7124BEE014E2A5BC0708E8310BE18DE910843EAC7EBCC2EC0AD727FF9E3180F7647B064659890AAD0203010001")
        {
            BinarySerializer = new BinarySerializer();
        }

        public async Task<IMessage> Run(IMessage Message, Func<IMessage, Task<IMessage>> Next)
        {
            //Using Binary Format Over HTTPs 

            var RequestBytes = await BinarySerializer.SerializeAsync(Message);

            var ResponseBytes = await ResolveAsync(RequestBytes);

            Message = await BinarySerializer.DeserializeAsync<Message>(ResponseBytes);

            return await Next(Message);
        }
    }
}
