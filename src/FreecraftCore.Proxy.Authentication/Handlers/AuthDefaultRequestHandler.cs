﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using FreecraftCore.Packet.Auth;
using GladNet;
using JetBrains.Annotations;

namespace FreecraftCore
{
	public sealed class AuthDefaultRequestHandler : IPeerPayloadSpecificMessageHandler<AuthenticationClientPayload, AuthenticationServerPayload, IPeerSessionMessageContext<AuthenticationServerPayload>>
	{
		private ILog Logger { get; }

		/// <inheritdoc />
		public AuthDefaultRequestHandler([NotNull] ILog logger)
		{
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			Logger = logger;
		}

		/// <inheritdoc />
		public Task HandleMessage(IPeerSessionMessageContext<AuthenticationServerPayload> context, AuthenticationClientPayload payload)
		{
			if(Logger.IsWarnEnabled)
				Logger.Warn($"Recieved unproxied Payload: {payload.GetType().Name} ConnectionId: {context.Details.ConnectionId}");

			//TODO: We cannot implement the default behavior of the proxy because some information is lost when we recieve an unknown payload.
			//The information about the opcode is not exposed to the handler so we can just forward unknown messages.
			//Alternatives is to add a middleware/pipeline extension that forwards "uninteresting" opcodes without even
			//handling them.

			return Task.CompletedTask;
		}
	}
}